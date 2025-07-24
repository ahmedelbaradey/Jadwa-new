using Abstraction.Contract.Service.Storage;
using Abstraction.Contracts.Service;
using Abstraction.Enums;
using Application.Common.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Infrastructure.Service.Storage
{
    /// <summary>
    /// Implementation of preview URL helper service
    /// </summary>
    public class PreviewUrlHelper : IPreviewUrlHelper
    {
        private readonly IServiceManager _serviceManager;
        private readonly MinIOConfiguration _minioConfig;
        private readonly ILogger<PreviewUrlHelper> _logger;

        public PreviewUrlHelper(
            IServiceManager serviceManager,
            IOptions<MinIOConfiguration> minioConfig,
            ILogger<PreviewUrlHelper> logger)
        {
            _serviceManager = serviceManager;
            _minioConfig = minioConfig.Value;
            _logger = logger;
        }

        /// <summary>
        /// Generates a preview URL for a file path using module ID to determine bucket
        /// </summary>
        public async Task<string> GeneratePreviewUrlAsync(string? filePath, int moduleId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;

            if (!_minioConfig.Enabled)
                return filePath; // Return original path if MinIO is disabled

            try
            {
                // Determine bucket name from module ID
                var bucketName = GetBucketNameFromModuleId(moduleId);
                return await GeneratePreviewUrlAsync(filePath, bucketName, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating preview URL for file: {FilePath} with moduleId: {ModuleId}", filePath, moduleId);
                return filePath; // Return original path on error
            }
        }

        /// <summary>
        /// Generates a preview URL for a file path with explicit bucket name
        /// </summary>
        public async Task<string> GeneratePreviewUrlAsync(string? filePath, string bucketName, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(filePath))
                return string.Empty;

            if (!_minioConfig.Enabled)
                return filePath; // Return original path if MinIO is disabled

            try
            {
                // Check if file exists in MinIO
                var fileExists = await _serviceManager.StorageService.FileExistsAsync(filePath, bucketName, cancellationToken);
                if (!fileExists)
                {
                    _logger.LogWarning("File not found in MinIO storage: {FilePath} in bucket {BucketName}", filePath, bucketName);
                    return filePath; // Return original path if file doesn't exist
                }

                // Generate preview URL
                var previewUrl = await _serviceManager.StorageService.GetPreviewUrlAsync(
                    filePath,
                    bucketName,
                    _minioConfig.DefaultUrlExpiryMinutes,
                    cancellationToken);

                if (string.IsNullOrEmpty(previewUrl))
                {
                    _logger.LogError("Failed to generate preview URL for file: {FilePath} in bucket {BucketName}", filePath, bucketName);
                    return filePath; // Return original path if URL generation fails
                }

                return previewUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating preview URL for file: {FilePath} in bucket {BucketName}", filePath, bucketName);
                return filePath; // Return original path on error
            }
        }

        /// <summary>
        /// Determines bucket name from module ID
        /// </summary>
        private string GetBucketNameFromModuleId(int moduleId)
        {
            if (Enum.IsDefined(typeof(ModuleEnum), moduleId))
            {
                var moduleName = Enum.GetName(typeof(ModuleEnum), moduleId);
                return moduleName?.ToLowerInvariant() ?? "other";
            }

            return "other";
        }
    }
}
