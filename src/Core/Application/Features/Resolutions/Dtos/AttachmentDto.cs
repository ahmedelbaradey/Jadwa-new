using Abstraction.Base.Dto;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for Attachment entity
    /// Contains attachment properties for resolution documents
    /// Based on requirements in Sprint.md for attachment management
    /// </summary>
    public record AttachmentDto : BaseDto
    {
        /// <summary>
        /// Original file name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// File path on storage
        /// </summary>
        public string FilePath { get; set; } = string.Empty;

        /// <summary>
        /// File size in bytes
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// MIME content type
        /// </summary>
        public string ContentType { get; set; } = string.Empty;

        /// <summary>
        /// Upload date
        /// </summary>
        public DateTime UploadedDate { get; set; }

        /// <summary>
        /// User who uploaded the file
        /// </summary>
        public string UploadedBy { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the attachment
        /// </summary>
        public string? Description { get; set; }
    }
}
