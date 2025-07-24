using Abstraction.Base.Response;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Abstraction.Enums;
using Application.Base.Abstracts;
using Application.Features.Shared.FileManagment.Dtos;
using AutoMapper;
using Domain.Entities.Shared;
using Abstraction.Contract.Service;
using Application.Common.Configurations;
using Microsoft.Extensions.Options;
using Abstraction.Contracts.Service;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Shared.FileManagment.Commands.MinIOUploadMultiple
{
    /// <summary>
    /// Handler for uploading multiple files to MinIO storage
    /// Provides batch file upload functionality with error handling and progress tracking
    /// </summary>
    public class MinIOUploadMultipleCommandHandler : BaseResponseHandler, ICommandHandler<MinIOUploadMultipleCommand, BaseResponse<MinIOUploadMultipleDto>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IServiceManager _serviceManager;
        private readonly MinIOConfiguration _minioConfig;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public MinIOUploadMultipleCommandHandler(
            IRepositoryManager repository,
            IMapper mapper,
            ILoggerManager logger,
            IServiceManager serviceManager,
            IOptions<MinIOConfiguration> minioConfig,
            IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _serviceManager = serviceManager;
            _minioConfig = minioConfig.Value;
            _localizer = localizer;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<MinIOUploadMultipleDto>> Handle(MinIOUploadMultipleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<MinIOUploadMultipleDto>(_localizer[SharedResourcesKey.MinIORequestCannotBeBlank]);

                if (request.Files == null || !request.Files.Any())
                    return BadRequest<MinIOUploadMultipleDto>(_localizer[SharedResourcesKey.MinIONoFilesProvided]);

                if (!_minioConfig.Enabled)
                    return BadRequest<MinIOUploadMultipleDto>(_localizer[SharedResourcesKey.MinIOStorageNotEnabled]);

                // Validate file count
                if (request.Files.Count > request.MaxFileCount)
                    return BadRequest<MinIOUploadMultipleDto>(_localizer[SharedResourcesKey.MinIOTooManyFiles, request.MaxFileCount]);

                // Validate file names if provided
                if (request.FileNames != null && request.FileNames.Count != request.Files.Count)
                    return BadRequest<MinIOUploadMultipleDto>(_localizer[SharedResourcesKey.MinIOFileNameCountMismatch]);

                // Get module name from enum safely
                string? sourceModuleFolder = Enum.IsDefined(typeof(ModuleEnum), request.ModuleId)
                    ? Enum.GetName(typeof(ModuleEnum), request.ModuleId)
                    : Enum.GetName(typeof(ModuleEnum), ModuleEnum.Other);

                // Determine bucket name
                var bucketName = !string.IsNullOrEmpty(request.BucketName)
                    ? request.BucketName.ToLowerInvariant()
                    : sourceModuleFolder?.ToLowerInvariant() ?? Enum.GetName(typeof(ModuleEnum), ModuleEnum.Other)?.ToLowerInvariant();

                var result = new MinIOUploadMultipleDto
                {
                    TotalFiles = request.Files.Count
                };

                // Process each file
                for (int i = 0; i < request.Files.Count; i++)
                {
                    var file = request.Files[i];
                    var fileName = request.FileNames?.ElementAtOrDefault(i) ?? file.FileName;

                    try
                    {
                        // Validate individual file
                        if (file == null || file.Length == 0)
                        {
                            result.FailedUploads.Add(new FailedUploadDto
                            {
                                FileName = fileName,
                                FileSize = file?.Length ?? 0,
                                ErrorMessage = _localizer[SharedResourcesKey.MinIOFileNullOrEmpty],
                                FileIndex = i
                            });
                            continue;
                        }

                        if (file.Length > _minioConfig.MaxFileSize)
                        {
                            result.FailedUploads.Add(new FailedUploadDto
                            {
                                FileName = fileName,
                                FileSize = file.Length,
                                ErrorMessage = _localizer[SharedResourcesKey.MinIOFileSizeExceedsLimit, _minioConfig.MaxFileSize],
                                FileIndex = i
                            });
                            continue;
                        }

                        // Extract extension safely
                        string? extension = Path.GetExtension(file.FileName)?.TrimStart('.');
                        if (string.IsNullOrWhiteSpace(extension))
                        {
                            result.FailedUploads.Add(new FailedUploadDto
                            {
                                FileName = fileName,
                                FileSize = file.Length,
                                ErrorMessage = _localizer[SharedResourcesKey.MinIOInvalidFileNameOrExtension],
                                FileIndex = i
                            });
                            continue;
                        }

                        // Upload to MinIO
                        var storageResult = await _serviceManager.StorageService.UploadFileAsync(file, fileName, bucketName, cancellationToken);

                        if (!storageResult.Success)
                        {
                            result.FailedUploads.Add(new FailedUploadDto
                            {
                                FileName = fileName,
                                FileSize = file.Length,
                                ErrorMessage = storageResult.ErrorMessage,
                                FileIndex = i
                            });

                            if (!request.ContinueOnError)
                                break;
                            
                            continue;
                        }

                        // Save attachment to database
                        var attachment = new Attachment
                        {
                            FileName = fileName,
                            Path = storageResult.FilePath,
                            ContentType = storageResult.ContentType,
                            FileSize = storageResult.FileSize,
                            ModuleId = request.ModuleId
                        };

                        var savedAttachment = await _repository.Attachments.AddAsync(attachment);
                        if (savedAttachment != null)
                        {
                            var attachmentDto = new AttachmentDTO
                            {
                                Id = savedAttachment.Id,
                                FileName = storageResult.FileName,
                                Folder = sourceModuleFolder,
                                Extension = extension,
                                ServerFileName = Path.GetFileName(storageResult.FilePath),
                                URL = storageResult.Url,
                                PreviewUrl = storageResult.PreviewUrl
                            };

                            result.SuccessfulUploads.Add(attachmentDto);
                            result.TotalSizeBytes += storageResult.FileSize;
                        }
                        else
                        {
                            // If database save failed, try to clean up the uploaded file
                            await _serviceManager.StorageService.DeleteFileAsync(storageResult.FilePath, bucketName, cancellationToken);
                            
                            result.FailedUploads.Add(new FailedUploadDto
                            {
                                FileName = fileName,
                                FileSize = file.Length,
                                ErrorMessage = "Failed to save attachment information to database",
                                FileIndex = i
                            });

                            if (!request.ContinueOnError)
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error uploading file: {fileName} at index {i}");
                        
                        result.FailedUploads.Add(new FailedUploadDto
                        {
                            FileName = fileName,
                            FileSize = file?.Length ?? 0,
                            ErrorMessage = $"Unexpected error: {ex.Message}",
                            FileIndex = i
                        });

                        if (!request.ContinueOnError)
                            break;
                    }
                }

                // Update counts
                result.SuccessCount = result.SuccessfulUploads.Count;
                result.FailureCount = result.FailedUploads.Count;

                _logger.LogInfo($"Multiple file upload completed: {result.SuccessCount} successful, {result.FailureCount} failed out of {result.TotalFiles} total files");

                // Determine overall success
                if (result.SuccessCount > 0)
                {
                    return Success(result);
                }
                else
                {
                    return BadRequest<MinIOUploadMultipleDto>("All file uploads failed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during multiple file upload");
                return ServerError<MinIOUploadMultipleDto>(ex.Message);
            }
        }
        #endregion
    }
}
