using Abstraction.Base.Response;
using Application.Base.Abstracts;

namespace Application.Features.Shared.FileManagment.Commands.MinIOPreview
{
    /// <summary>
    /// Command for generating preview URLs for files in MinIO storage
    /// </summary>
    public record MinIOPreviewCommand : ICommand<BaseResponse<MinIOPreviewDto>>
    {
        /// <summary>
        /// Attachment ID to generate preview for
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Optional bucket name override (if not provided, will determine from attachment module)
        /// </summary>
        public string? BucketName { get; set; }

        /// <summary>
        /// URL expiry time in minutes (default: 10080 minutes = 7 days for effectively no expiry)
        /// </summary>
        public int ExpiryInMinutes { get; set; } = 10080;
    }

    /// <summary>
    /// DTO for MinIO preview response
    /// </summary>
    public record MinIOPreviewDto
    {
        /// <summary>
        /// Attachment ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Original file name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// Presigned URL for file preview/access
        /// </summary>
        public string PreviewUrl { get; set; } = string.Empty;

        /// <summary>
        /// URL expiry date and time
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// File content type
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }
    }
}
