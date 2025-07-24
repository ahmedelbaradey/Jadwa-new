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

namespace Application.Features.Shared.FileManagment.Commands.MinIODelete
{
    /// <summary>
    /// Handler for deleting files from MinIO storage
    /// Provides secure file deletion with optional database record removal
    /// </summary>
    public class MinIODeleteCommandHandler : BaseResponseHandler, ICommandHandler<MinIODeleteCommand, BaseResponse<MinIODeleteDto>>
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
        public MinIODeleteCommandHandler(
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
        public async Task<BaseResponse<MinIODeleteDto>> Handle(MinIODeleteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request?.Id == null)
                    return BadRequest<MinIODeleteDto>(_localizer[SharedResourcesKey.MinIOFileIdRequired]);

                if (!_minioConfig.Enabled)
                    return BadRequest<MinIODeleteDto>(_localizer[SharedResourcesKey.MinIOStorageNotEnabled]);

                // Get file information from database
                var file = await _repository.Attachments.GetByIdAsync<Attachment>(request.Id.Value, false);
                if (file is null)
                    return BadRequest<MinIODeleteDto>(_localizer[SharedResourcesKey.MinIOFileNotFound]);

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
                        bucketName = Enum.GetName(typeof(ModuleEnum), file.ModuleId)?.ToLowerInvariant() ?? Enum.GetName(ModuleEnum.Other) ?? "other";
                    }
                }

                var deleteDto = new MinIODeleteDto
                {
                    Id = file.Id,
                    FileName = file.FileName,
                    FilePath = file.Path,
                    DeletedFromStorage = false,
                    DeletedFromDatabase = false
                };

                // Delete from MinIO storage
                var storageDeleted = await _serviceManager.StorageService.DeleteFileAsync(file.Path, bucketName, cancellationToken);
                deleteDto.DeletedFromStorage = storageDeleted;

                if (!storageDeleted)
                {
                    _logger.LogWarn($"Failed to delete file from MinIO storage: {file.Path} in bucket {bucketName}");
                    // Continue with database deletion even if storage deletion failed
                }

                // Delete from database if requested
                if (request.DeleteDatabaseRecord)
                {
                    try
                    {
                        await _repository.Attachments.DeleteAsync(file);
                        deleteDto.DeletedFromDatabase = true;
                        _logger.LogInfo($"File record deleted from database: {file.FileName} with ID {file.Id}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Failed to delete file record from database: ID {file.Id}");
                        deleteDto.DeletedFromDatabase = false;
                    }
                }

                // Determine overall success
                bool overallSuccess = deleteDto.DeletedFromStorage && 
                                    (!request.DeleteDatabaseRecord || deleteDto.DeletedFromDatabase);

                if (overallSuccess)
                {
                    _logger.LogInfo($"{_localizer[SharedResourcesKey.MinIOFileDeletedSuccessfully]}: {file.FileName} with ID {file.Id}");
                    return Success(deleteDto);
                }
                else
                {
                    string errorMessage = _localizer[SharedResourcesKey.MinIOFileDeleteFailed] + ". ";
                    if (!deleteDto.DeletedFromStorage)
                        errorMessage += _localizer[SharedResourcesKey.MinIOFileDeleteFailed] + " (Storage). ";
                    if (request.DeleteDatabaseRecord && !deleteDto.DeletedFromDatabase)
                        errorMessage += _localizer[SharedResourcesKey.MinIOFileDeleteFailed] + " (Database). ";

                    return BadRequest<MinIODeleteDto>(errorMessage.Trim());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while deleting file: ID {request?.Id}");
                return ServerError<MinIODeleteDto>(ex.Message);
            }
        }
        #endregion
    }
}
