namespace Application.Common.Configurations
{
    /// <summary>
    /// Configuration settings for MinIO object storage
    /// </summary>
    public class MinIOConfiguration
    {
        public const string Section = "MinIOConfiguration";

        /// <summary>
        /// MinIO server endpoint URL
        /// </summary>
        public string Endpoint { get; set; } = string.Empty;

        /// <summary>
        /// Access key for MinIO authentication
        /// </summary>
        public string AccessKey { get; set; } = string.Empty;

        /// <summary>
        /// Secret key for MinIO authentication
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Whether to use SSL/TLS for connections
        /// </summary>
        public bool UseSSL { get; set; } = true;

        /// <summary>
        /// Default bucket name for file storage
        /// </summary>
        public string DefaultBucket { get; set; } = "other";

        /// <summary>
        /// Region for MinIO (optional)
        /// </summary>
        public string Region { get; set; } = string.Empty;

        /// <summary>
        /// Default presigned URL expiry in minutes (10080 = 7 days, effectively no expiry for most use cases)
        /// </summary>
        public int DefaultUrlExpiryMinutes { get; set; } = 10080;

        /// <summary>
        /// Maximum file size allowed in bytes (default: 10MB)
        /// </summary>
        public long MaxFileSize { get; set; } = 10 * 1024 * 1024;

        /// <summary>
        /// Whether to enable MinIO storage (fallback to local storage if false)
        /// </summary>
        public bool Enabled { get; set; } = true;
    }
}
