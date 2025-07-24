using Domain.Entities.Base;
using Domain.Entities.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Represents a resolution attachment entity for managing multiple file attachments per resolution
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Based on requirements in Sprint.md for resolution attachment management (JDWA-568, JDWA-505)
    /// </summary>
    public class ResolutionAttachment : FullAuditedEntity
    {
        /// <summary>
        /// Resolution identifier that this attachment belongs to
        /// Foreign key reference to Resolution entity
        /// </summary>
        public int ResolutionId { get; set; }
        public int AttachmentId { get; set; }

        /// <summary>
        /// Navigation property to Resolution entity
        /// Provides access to the parent resolution
        /// </summary>
        [ForeignKey("ResolutionId")]
        public Resolution Resolution { get; set; } = null!;

        [ForeignKey("AttachmentId")]
        public Attachment Attachment { get; set; }

        ///// <summary>
        ///// Original file name as uploaded by the user
        ///// Required field for display purposes
        ///// </summary>
        //public string FileName { get; set; } = string.Empty;

        ///// <summary>
        ///// File path where the attachment is stored
        ///// Required field for file access
        ///// </summary>
        //public string FilePath { get; set; } = string.Empty;

        ///// <summary>
        ///// File size in bytes
        ///// Used for display and validation (max 10MB per file)
        ///// </summary>
        //public long FileSizeBytes { get; set; }

        ///// <summary>
        ///// File content type (MIME type)
        ///// Used for validation and proper file handling
        ///// </summary>
        //public string ContentType { get; set; } = string.Empty;

        ///// <summary>
        ///// File extension (e.g., .pdf, .doc, .xlsx)
        ///// Extracted from original file name for validation
        ///// </summary>
        //public string FileExtension { get; set; } = string.Empty;

        ///// <summary>
        ///// Display order for sorting attachments
        ///// Used for maintaining consistent attachment ordering
        ///// </summary>
        //public int DisplayOrder { get; set; }

        ///// <summary>
        ///// Indicates if this attachment is active
        ///// Used for soft deletion of attachments
        ///// </summary>
        //public bool IsActive { get; set; } = true;

        ///// <summary>
        ///// Optional description or notes about the attachment
        ///// Can be used to describe the attachment purpose
        ///// </summary>
        //public string? Description { get; set; }



        //    /// <summary>
        //    /// Gets the file size in a human-readable format
        //    /// </summary>
        //    /// <returns>Formatted file size string</returns>
        //    public string GetFormattedFileSize()
        //    {
        //        const long KB = 1024;
        //        const long MB = KB * 1024;
        //        const long GB = MB * 1024;

        //        if (FileSizeBytes >= GB)
        //            return $"{FileSizeBytes / (double)GB:F2} GB";
        //        else if (FileSizeBytes >= MB)
        //            return $"{FileSizeBytes / (double)MB:F2} MB";
        //        else if (FileSizeBytes >= KB)
        //            return $"{FileSizeBytes / (double)KB:F2} KB";
        //        else
        //            return $"{FileSizeBytes} bytes";
        //    }

        //    /// <summary>
        //    /// Checks if the file size is within the allowed limit (10MB)
        //    /// </summary>
        //    /// <returns>True if file size is valid, false otherwise</returns>
        //    public bool IsFileSizeValid()
        //    {
        //        const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10MB
        //        return FileSizeBytes <= MaxFileSizeBytes;
        //    }
        }
    }
