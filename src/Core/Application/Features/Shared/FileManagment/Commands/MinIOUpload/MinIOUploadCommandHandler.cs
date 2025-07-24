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

namespace Application.Features.Shared.FileManagment.Commands.MinIOUpload
{
    /// <summary>
    /// Handler for uploading files to MinIO storage
    /// Provides MinIO-specific file upload functionality while preserving existing local storage handlers
    /// </summary>
    public class MinIOUploadCommandHandler : BaseResponseHandler, ICommandHandler<MinIOUploadCommand, BaseResponse<AttachmentDTO>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        //private readonly IStorageService _storageService;
        private readonly IServiceManager _serviceManager;
        private readonly MinIOConfiguration _minioConfig;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public MinIOUploadCommandHandler(
            IRepositoryManager repository,
            IMapper mapper,
            ILoggerManager logger,
            //IStorageService storageService,
            IServiceManager serviceManager,
            IOptions<MinIOConfiguration> minioConfig,
            IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            //_storageService = storageService;
            _serviceManager = serviceManager;
            _minioConfig = minioConfig.Value;
            _localizer = localizer;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<AttachmentDTO>> Handle(MinIOUploadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<AttachmentDTO>(_localizer[SharedResourcesKey.MinIORequestCannotBeBlank]);

                if (request.File == null || request.File.Length == 0)
                    return BadRequest<AttachmentDTO>(_localizer[SharedResourcesKey.MinIOFileMissingOrEmpty]);

                if (!_minioConfig.Enabled)
                    return BadRequest<AttachmentDTO>(_localizer[SharedResourcesKey.MinIOStorageNotEnabled]);

                // Get module name from enum safely
                string? sourceModuleFolder = Enum.IsDefined(typeof(ModuleEnum), request.ModuleId)
                    ? Enum.GetName(typeof(ModuleEnum), request.ModuleId)
                    : Enum.GetName(typeof(ModuleEnum), ModuleEnum.Other);

                // Extract extension safely
                string? extension = Path.GetExtension(request.File.FileName)?.TrimStart('.');
                if (string.IsNullOrWhiteSpace(extension))
                    return BadRequest<AttachmentDTO>(_localizer[SharedResourcesKey.MinIOInvalidFileNameOrExtension]);
                
                var bucketName = Enum.GetName(request.BucketName?? ModuleEnum.Other).ToLowerInvariant() ?? sourceModuleFolder.ToLowerInvariant();

                // Upload to MinIO
                var storageResult = await _serviceManager.StorageService.UploadFileAsync(
                    request.File, 
                    request.FileName, 
                    bucketName, 
                    cancellationToken);

                if (!storageResult.Success)
                {
                    _logger.LogError(null, $"Failed to upload file to MinIO: {storageResult.ErrorMessage}");
                    return BadRequest<AttachmentDTO>(_localizer[SharedResourcesKey.MinIOFileUploadFailed] + ": " + storageResult.ErrorMessage);
                }

                // Create DTO
                var attachment = new AttachmentDTO
                {
                    FileName = storageResult.FileName,
                    Folder = sourceModuleFolder,
                    Extension = extension,
                    ServerFileName = Path.GetFileName(storageResult.FilePath),
                    URL = storageResult.PreviewUrl,
                    PreviewUrl = storageResult.PreviewUrl
                };

                // Save attachment to database
                var file = _mapper.Map<Attachment>(request);
                file.Path = storageResult.FilePath;
                file.ContentType = storageResult.ContentType;
                file.FileSize = storageResult.FileSize;
                
                var result = await _repository.Attachments.AddAsync(file);
                if (result is null)
                {
                    _logger.LogError(null, "Failed to save attachment to database");
                    return BadRequest<AttachmentDTO>(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);
                }

                attachment.Id = result.Id;

                _logger.LogInfo($"{_localizer[SharedResourcesKey.MinIOFileUploadedSuccessfully]}: {storageResult.FileName} with ID {result.Id}");

                return Success(attachment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while uploading file to MinIO: {request?.FileName}");
                return ServerError<AttachmentDTO>(ex.Message);
            }
        }
        #endregion
    }
}
