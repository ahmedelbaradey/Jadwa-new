using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Application.Features.Shared.FileManagment.Dtos;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Shared.FileManagment.Commands.MinIOUploadMultiple
{
    /// <summary>
    /// Command for uploading multiple files to MinIO storage
    /// </summary>
    public record MinIOUploadMultipleCommand : ICommand<BaseResponse<MinIOUploadMultipleDto>>
    {
        /// <summary>
        /// List of files to upload
        /// </summary>
        public List<IFormFile> Files { get; set; } = new();

        /// <summary>
        /// Module ID for categorizing files
        /// </summary>
        public int ModuleId { get; set; }

        /// <summary>
        /// Optional bucket name override (if not provided, will use module-based bucket)
        /// </summary>
        public string? BucketName { get; set; }

        /// <summary>
        /// Optional custom file names (must match Files count if provided)
        /// </summary>
        public List<string>? FileNames { get; set; }

        /// <summary>
        /// Whether to continue uploading remaining files if one fails (default: true)
        /// </summary>
        public bool ContinueOnError { get; set; } = true;

        /// <summary>
        /// Maximum number of files allowed in a single upload (default: 10)
        /// </summary>
        public int MaxFileCount { get; set; } = 10;
    }

    /// <summary>
    /// DTO for multiple file upload response
    /// </summary>
    public record MinIOUploadMultipleDto
    {
        /// <summary>
        /// List of successfully uploaded files
        /// </summary>
        public List<AttachmentDTO> SuccessfulUploads { get; set; } = new();

        /// <summary>
        /// List of failed uploads with error details
        /// </summary>
        public List<FailedUploadDto> FailedUploads { get; set; } = new();

        /// <summary>
        /// Total number of files processed
        /// </summary>
        public int TotalFiles { get; set; }

        /// <summary>
        /// Number of successful uploads
        /// </summary>
        public int SuccessCount { get; set; }

        /// <summary>
        /// Number of failed uploads
        /// </summary>
        public int FailureCount { get; set; }

        /// <summary>
        /// Overall upload completion time
        /// </summary>
        public DateTime CompletedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Total size of all uploaded files in bytes
        /// </summary>
        public long TotalSizeBytes { get; set; }
    }

    /// <summary>
    /// DTO for failed upload information
    /// </summary>
    public record FailedUploadDto
    {
        /// <summary>
        /// Original file name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Error message describing why the upload failed
        /// </summary>
        public string ErrorMessage { get; set; } = string.Empty;

        /// <summary>
        /// Index of the file in the original upload list
        /// </summary>
        public int FileIndex { get; set; }
    }
}
