namespace Abstraction.Contract.Service.Storage
{
    /// <summary>
    /// Helper service for generating MinIO preview URLs for file paths
    /// Centralizes the logic for converting file paths to preview URLs
    /// </summary>
    public interface IPreviewUrlHelper
    {
        /// <summary>
        /// Generates a preview URL for a file path
        /// </summary>
        /// <param name="filePath">The file path in storage</param>
        /// <param name="moduleId">The module ID to determine bucket name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Preview URL or original file path if generation fails</returns>
        Task<string> GeneratePreviewUrlAsync(string? filePath, int moduleId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Generates a preview URL for a file path with explicit bucket name
        /// </summary>
        /// <param name="filePath">The file path in storage</param>
        /// <param name="bucketName">The bucket name</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Preview URL or original file path if generation fails</returns>
        Task<string> GeneratePreviewUrlAsync(string? filePath, string bucketName, CancellationToken cancellationToken = default);
    }
}
