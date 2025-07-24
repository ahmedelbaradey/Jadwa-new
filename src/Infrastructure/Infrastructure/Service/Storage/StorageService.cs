using Abstraction.Contract.Service.Storage;
using Application.Common.Configurations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.Security.Cryptography;
using System.Text;

namespace Infrastructure.Service.Storage
{
    /// <summary>
    /// MinIO implementation of the storage service
    /// Provides file upload, download, delete, and preview operations using MinIO object storage
    /// </summary>
    public class StorageService : IStorageService
    {
        private readonly IMinioClient _minioClient;
        private readonly MinIOConfiguration _config;
        private readonly ILogger<StorageService> _logger;

        public StorageService(IMinioClient minioClient, IOptions<MinIOConfiguration> config, ILogger<StorageService> logger)
        {
            _minioClient = minioClient;
            _config = config.Value;
            _logger = logger;
        }

        /// <summary>
        /// Uploads a file to MinIO storage
        /// </summary>
        public async Task<StorageResult> UploadFileAsync(IFormFile file, string fileName, string bucketName, CancellationToken cancellationToken = default)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new StorageResult { Success = false, ErrorMessage = "File is null or empty" };
                }

                if (file.Length > _config.MaxFileSize)
                {
                    return new StorageResult { Success = false, ErrorMessage = $"File size exceeds maximum allowed size of {_config.MaxFileSize} bytes" };
                }

                // Ensure bucket exists
                await EnsureBucketExistsAsync(bucketName, cancellationToken);

                // Generate unique file path
                var extension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}{extension}";
                var filePath = $"{uniqueFileName}";
                //var filePath = $"{DateTime.UtcNow:yyyy/MM/dd}/{uniqueFileName}";

                // Upload file
                using var stream = file.OpenReadStream();
                var putObjectArgs = new PutObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filePath)
                    .WithStreamData(stream)
                    .WithObjectSize(file.Length)
                    .WithContentType(file.ContentType);

                await _minioClient.PutObjectAsync(putObjectArgs, cancellationToken);

                _logger.LogInformation("File uploaded successfully: {FilePath} to bucket {BucketName}", filePath, bucketName);

                // Generate preview URL automatically after successful upload
                var previewUrl = await GetPreviewUrlAsync(filePath, bucketName, _config.DefaultUrlExpiryMinutes, cancellationToken);
                var previewExpiresAt = DateTime.UtcNow.AddMinutes(_config.DefaultUrlExpiryMinutes);

                return new StorageResult
                {
                    Success = true,
                    FilePath = filePath,
                    FileName = fileName,
                    FileSize = file.Length,
                    ContentType = file.ContentType,
                    Url = $"/{bucketName}/{filePath}",
                    PreviewUrl = previewUrl,
                    PreviewUrlExpiresAt = previewExpiresAt
                };
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, "MinIO error occurred while uploading file: {FileName}", fileName);
                return new StorageResult { Success = false, ErrorMessage = $"Storage error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while uploading file: {FileName}", fileName);
                return new StorageResult { Success = false, ErrorMessage = $"Unexpected error: {ex.Message}" };
            }
        }

        /// <summary>
        /// Downloads a file from MinIO storage
        /// </summary>
        public async Task<FileDownloadResult> DownloadFileAsync(string filePath, string bucketName, CancellationToken cancellationToken = default)
        {
            try
            {
                // Check if file exists
                var exists = await FileExistsAsync(filePath, bucketName, cancellationToken);
                if (!exists)
                {
                    return new FileDownloadResult { Success = false, ErrorMessage = "File not found" };
                }

                // Get file metadata
                var metadata = await GetFileMetadataAsync(filePath, bucketName, cancellationToken);

                // Download file
                var memoryStream = new MemoryStream();
                var getObjectArgs = new GetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filePath)
                    .WithCallbackStream(stream => stream.CopyTo(memoryStream));

                await _minioClient.GetObjectAsync(getObjectArgs, cancellationToken);
                memoryStream.Position = 0;

                _logger.LogInformation("File downloaded successfully: {FilePath} from bucket {BucketName}", filePath, bucketName);

                return new FileDownloadResult
                {
                    Success = true,
                    FileStream = memoryStream,
                    FileName = metadata.FileName,
                    ContentType = metadata.ContentType,
                    FileSize = metadata.FileSize
                };
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, "MinIO error occurred while downloading file: {FilePath}", filePath);
                return new FileDownloadResult { Success = false, ErrorMessage = $"Storage error: {ex.Message}" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while downloading file: {FilePath}", filePath);
                return new FileDownloadResult { Success = false, ErrorMessage = $"Unexpected error: {ex.Message}" };
            }
        }

        /// <summary>
        /// Deletes a file from MinIO storage
        /// </summary>
        public async Task<bool> DeleteFileAsync(string filePath, string bucketName, CancellationToken cancellationToken = default)
        {
            try
            {
                var removeObjectArgs = new RemoveObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filePath);

                await _minioClient.RemoveObjectAsync(removeObjectArgs, cancellationToken);

                _logger.LogInformation("File deleted successfully: {FilePath} from bucket {BucketName}", filePath, bucketName);
                return true;
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, "MinIO error occurred while deleting file: {FilePath}", filePath);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while deleting file: {FilePath}", filePath);
                return false;
            }
        }

        /// <summary>
        /// Generates a presigned URL for file preview/access
        /// </summary>
        public async Task<string> GetPreviewUrlAsync(string filePath, string bucketName, int expiryInMinutes = 60, CancellationToken cancellationToken = default)
        {
            try
            {
                var presignedGetObjectArgs = new PresignedGetObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filePath)
                    .WithExpiry(expiryInMinutes * 60); // Convert to seconds

                var url = await _minioClient.PresignedGetObjectAsync(presignedGetObjectArgs);

                _logger.LogInformation("Presigned URL generated for file: {FilePath} from bucket {BucketName}", filePath, bucketName);
                return url;
            }
            catch (MinioException ex)
            {
                _logger.LogError(ex, "MinIO error occurred while generating preview URL: {FilePath}", filePath);
                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while generating preview URL: {FilePath}", filePath);
                return string.Empty;
            }
        }

        /// <summary>
        /// Checks if a file exists in MinIO storage
        /// </summary>
        public async Task<bool> FileExistsAsync(string filePath, string bucketName, CancellationToken cancellationToken = default)
        {
            try
            {
                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filePath);

                await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);
                return true;
            }
            catch (ObjectNotFoundException)
            {
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if file exists: {FilePath}", filePath);
                return false;
            }
        }

        /// <summary>
        /// Gets file metadata without downloading the file
        /// </summary>
        public async Task<FileMetadata> GetFileMetadataAsync(string filePath, string bucketName, CancellationToken cancellationToken = default)
        {
            try
            {
                var statObjectArgs = new StatObjectArgs()
                    .WithBucket(bucketName)
                    .WithObject(filePath);

                var objectStat = await _minioClient.StatObjectAsync(statObjectArgs, cancellationToken);

                return new FileMetadata
                {
                    FileName = Path.GetFileName(filePath),
                    FileSize = objectStat.Size,
                    ContentType = objectStat.ContentType,
                    LastModified = objectStat.LastModified,
                    ETag = objectStat.ETag
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting file metadata: {FilePath}", filePath);
                return new FileMetadata();
            }
        }

        /// <summary>
        /// Ensures that the specified bucket exists, creates it if it doesn't
        /// </summary>
        private async Task EnsureBucketExistsAsync(string bucketName, CancellationToken cancellationToken = default)
        {
            try
            {
                var bucketExistsArgs = new BucketExistsArgs().WithBucket(bucketName);
                var exists = await _minioClient.BucketExistsAsync(bucketExistsArgs, cancellationToken);

                if (!exists)
                {
                    var makeBucketArgs = new MakeBucketArgs().WithBucket(bucketName);
                    await _minioClient.MakeBucketAsync(makeBucketArgs, cancellationToken);
                    _logger.LogInformation("Created bucket: {BucketName}", bucketName);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ensuring bucket exists: {BucketName}", bucketName);
                throw;
            }
        }
    }
}
