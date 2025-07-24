using Abstraction.Base.Response;
using Application.Base.Abstracts;

namespace Application.Features.Shared.FileManagment.Commands.MinIODelete
{
    /// <summary>
    /// Command for deleting files from MinIO storage
    /// </summary>
    public record MinIODeleteCommand : ICommand<BaseResponse<MinIODeleteDto>>
    {
        /// <summary>
        /// Attachment ID to delete
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// Optional bucket name override (if not provided, will determine from attachment module)
        /// </summary>
        public string? BucketName { get; set; }

        /// <summary>
        /// Whether to also delete the database record (default: true)
        /// </summary>
        public bool DeleteDatabaseRecord { get; set; } = true;
    }

    /// <summary>
    /// DTO for MinIO delete response
    /// </summary>
    public record MinIODeleteDto
    {
        /// <summary>
        /// Attachment ID that was deleted
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Original file name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// File path that was deleted from storage
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// Whether the file was successfully deleted from storage
        /// </summary>
        public bool DeletedFromStorage { get; set; }

        /// <summary>
        /// Whether the database record was deleted
        /// </summary>
        public bool DeletedFromDatabase { get; set; }

        /// <summary>
        /// Deletion timestamp
        /// </summary>
        public DateTime DeletedAt { get; set; } = DateTime.UtcNow;
    }
}
