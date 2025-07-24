using Abstraction.Base.Response;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using Application.Features.Shared.FileManagment.Dtos;
using AutoMapper;
using Domain.Entities.Shared;
using Abstraction.Contract.Service;
using Application.Common.Configurations;
using Microsoft.Extensions.Options;
using Abstraction.Contracts.Service;
using Abstraction.Enums;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Shared.FileManagment.Commands.MinIODownload
{
    /// <summary>
    /// Handler for downloading files from MinIO storage
    /// Provides MinIO-specific file download functionality while preserving existing local storage handlers
    /// </summary>
    public class MinIODownloadCommandHandler : BaseResponseHandler, ICommandHandler<MinIODownloadCommand, BaseResponse<DownloadAttachmentDTO>>
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
        public MinIODownloadCommandHandler(
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
        public async Task<BaseResponse<DownloadAttachmentDTO>> Handle(MinIODownloadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request?.Id == null)
                    return BadRequest<DownloadAttachmentDTO>(_localizer[SharedResourcesKey.MinIOFileIdRequired]);

                if (!_minioConfig.Enabled)
                    return BadRequest<DownloadAttachmentDTO>(_localizer[SharedResourcesKey.MinIOStorageNotEnabled]);

                // Get file information from database
                var file = await _repository.Attachments.GetByIdAsync<Attachment>(request.Id.Value, false);
                if (file is null)
                    return BadRequest<DownloadAttachmentDTO>(_localizer[SharedResourcesKey.MinIOFileNotFound]);

                // Determine bucket name
                string bucketName;
                if (!string.IsNullOrEmpty(request.BucketName))
                {
                    bucketName = request.BucketName.ToLowerInvariant();
                }
                else
                {
                    // Determine bucket from module ID
                    bucketName = Enum.GetName(typeof(ModuleEnum), ModuleEnum.Other)?.ToLowerInvariant() ?? "other";
                    if (Enum.IsDefined(typeof(ModuleEnum), file.ModuleId))
                    {
                        bucketName = Enum.GetName(typeof(ModuleEnum), file.ModuleId)?.ToLowerInvariant() ?? Enum.GetName(typeof(ModuleEnum), ModuleEnum.Other) ?? "other";
                    }
                }

                // Download from MinIO
                var downloadResult = await _serviceManager.StorageService.DownloadFileAsync(file.Path, bucketName, cancellationToken);
                
                if (!downloadResult.Success || downloadResult.FileStream == null)
                {
                    _logger.LogError(null, $"Failed to download file from MinIO: {downloadResult.ErrorMessage}");
                    return BadRequest<DownloadAttachmentDTO>(downloadResult.ErrorMessage ?? _localizer[SharedResourcesKey.MinIOFileNotFoundInStorage]);
                }

                // Convert stream to byte array
                using var memoryStream = new MemoryStream();
                await downloadResult.FileStream.CopyToAsync(memoryStream, cancellationToken);
                var fileBytes = memoryStream.ToArray();
                
                // Dispose the original stream
                downloadResult.FileStream.Dispose();

                var attachment = new DownloadAttachmentDTO
                {
                    Id = file.Id,
                    FileName = file.FileName,
                    ContentType = downloadResult.ContentType,
                    FileBytes = fileBytes,
                    Path = file.Path,
                };

                _logger.LogInfo($"File downloaded successfully from MinIO: {file.FileName} with ID {file.Id}");

                return Success(attachment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while downloading file from MinIO: ID {request?.Id}");
                return ServerError<DownloadAttachmentDTO>(ex.Message);
            }
        }
        #endregion
    }
}
