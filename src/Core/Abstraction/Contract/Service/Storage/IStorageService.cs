using Microsoft.AspNetCore.Http;

namespace Abstraction.Contract.Service.Storage
{
    /// <summary>
    /// Interface for file storage operations supporting multiple storage providers (local, MinIO, etc.)
    /// Provides methods for upload, download, delete, and preview operations
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// Uploads a file to the storage provider
        /// </summary>
        /// <param name="file">The file to upload</param>
        /// <param name="fileName">The desired file name</param>
        /// <param name="bucketName">The bucket/container name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Storage result containing file path and metadata</returns>
        Task<StorageResult> UploadFileAsync(IFormFile file, string fileName, string bucketName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Downloads a file from the storage provider
        /// </summary>
        /// <param name="filePath">The path to the file in storage</param>
        /// <param name="bucketName">The bucket/container name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>File stream and metadata</returns>
        Task<FileDownloadResult> DownloadFileAsync(string filePath, string bucketName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Deletes a file from the storage provider
        /// </summary>
        /// <param name="filePath">The path to the file in storage</param>
        /// <param name="bucketName">The bucket/container name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if deletion was successful</returns>
        Task<bool> DeleteFileAsync(string filePath, string bucketName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates a presigned URL for file preview/access
        /// </summary>
        /// <param name="filePath">The path to the file in storage</param>
        /// <param name="bucketName">The bucket/container name</param>
        /// <param name="expiryInMinutes">URL expiry time in minutes (default: 60)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Presigned URL for file access</returns>
        Task<string> GetPreviewUrlAsync(string filePath, string bucketName, int expiryInMinutes = 60, CancellationToken cancellationToken = default);

        /// <summary>
        /// Checks if a file exists in the storage provider
        /// </summary>
        /// <param name="filePath">The path to the file in storage</param>
        /// <param name="bucketName">The bucket/container name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>True if file exists</returns>
        Task<bool> FileExistsAsync(string filePath, string bucketName, CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets file metadata without downloading the file
        /// </summary>
        /// <param name="filePath">The path to the file in storage</param>
        /// <param name="bucketName">The bucket/container name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>File metadata</returns>
        Task<FileMetadata> GetFileMetadataAsync(string filePath, string bucketName, CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Result of file upload operation
    /// </summary>
    public class StorageResult
    {
        public bool Success { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        /// <summary>
        /// Presigned URL for file preview/access (generated automatically on upload)
        /// </summary>
        public string PreviewUrl { get; set; } = string.Empty;
        /// <summary>
        /// Expiry date/time for the preview URL
        /// </summary>
        public DateTime? PreviewUrlExpiresAt { get; set; }
    }

    /// <summary>
    /// Result of file download operation
    /// </summary>
    public class FileDownloadResult
    {
        public bool Success { get; set; }
        public Stream? FileStream { get; set; }
        public string FileName { get; set; } = string.Empty;
        public string ContentType { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    /// <summary>
    /// File metadata information
    /// </summary>
    public class FileMetadata
    {
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ContentType { get; set; } = string.Empty;
        public DateTime LastModified { get; set; }
        public string ETag { get; set; } = string.Empty;
    }
}
