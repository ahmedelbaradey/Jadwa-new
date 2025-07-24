using Abstraction.Base.Response;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using AutoMapper;
using Domain.Entities.Shared;
using Abstraction.Contract.Service;
using Application.Common.Configurations;
using Microsoft.Extensions.Options;
using Abstraction.Enums;
using Abstraction.Contracts.Service;
using Microsoft.Extensions.Localization;
using Resources;
using Application.Features.Shared.FileManagment.Dtos;

namespace Application.Features.Shared.FileManagment.Commands.MinIOPreview
{
    /// <summary>
    /// Handler for generating preview URLs for files in MinIO storage
    /// Provides secure, time-limited access to files for preview purposes
    /// </summary>
    public class MinIOPreviewCommandHandler : BaseResponseHandler, ICommandHandler<MinIOPreviewCommand, BaseResponse<MinIOPreviewDto>>
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
        public MinIOPreviewCommandHandler(
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
        public async Task<BaseResponse<MinIOPreviewDto>> Handle(MinIOPreviewCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request?.Id == null)
                    return BadRequest<MinIOPreviewDto>(_localizer[SharedResourcesKey.MinIOFileIdRequired]);

                if (!_minioConfig.Enabled)
                    return BadRequest<MinIOPreviewDto>(_localizer[SharedResourcesKey.MinIOStorageNotEnabled]);

                // Validate expiry time - allow very long expiry times for no-expiry behavior
                if (request.ExpiryInMinutes <= 0)
                    request.ExpiryInMinutes = _minioConfig.DefaultUrlExpiryMinutes;

                // Get file information from database
                var file = await _repository.Attachments.GetByIdAsync<Attachment>(request.Id.Value, false);
                if (file is null)
                    return BadRequest<MinIOPreviewDto>(_localizer[SharedResourcesKey.MinIOFileNotFound]);

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
                        bucketName = Enum.GetName(typeof(ModuleEnum), file.ModuleId)?.ToLowerInvariant() ?? Enum.GetName(typeof(ModuleEnum), ModuleEnum.Other)?.ToLowerInvariant() ?? "other";
                    }
                }

                // Check if file exists in MinIO
                var fileExists = await _serviceManager.StorageService.FileExistsAsync(file.Path, bucketName, cancellationToken);
                if (!fileExists)
                {
                    _logger.LogWarn($"File not found in MinIO storage: {file.Path} in bucket {bucketName}");
                    return BadRequest<MinIOPreviewDto>(_localizer[SharedResourcesKey.MinIOFileNotFoundInStorage]);
                }

                // Generate presigned URL
                var previewUrl = await _serviceManager.StorageService.GetPreviewUrlAsync(file.Path, bucketName, _minioConfig.DefaultUrlExpiryMinutes, cancellationToken);

                if (string.IsNullOrEmpty(previewUrl))
                {
                    _logger.LogError(null, $"Failed to generate preview URL for file: {file.Path}");
                    return BadRequest<MinIOPreviewDto>(_localizer[SharedResourcesKey.MinIOPreviewUrlGenerationFailed]);
                }

                var previewDto = new MinIOPreviewDto
                {
                    Id = file.Id,
                    FileName = file.FileName,
                    PreviewUrl = previewUrl,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(request.ExpiryInMinutes),
                    ContentType = file.ContentType,
                    FileSize = file.FileSize
                };

                _logger.LogInfo($"Preview URL generated successfully for file: {file.FileName} with ID {file.Id}, expires at {previewDto.ExpiresAt}");

                return Success(previewDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while generating preview URL for file: ID {request?.Id}");
                return ServerError<MinIOPreviewDto>(ex.Message);
            }
        }
        #endregion
    }
}
