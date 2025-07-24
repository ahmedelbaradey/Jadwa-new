using System.ComponentModel.DataAnnotations;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for editing resolution attachments
    /// Used in EditResolutionAttachments command operations
    /// Based on requirements in Sprint.md (JDWA-568)
    /// </summary>
    public record EditResolutionAttachmentsDto
    {
        /// <summary>
        /// Resolution identifier
        /// </summary>
        [Required]
        public int ResolutionId { get; set; }

        /// <summary>
        /// Collection of attachment IDs to be associated with the resolution
        /// Maximum 10 attachments allowed
        /// </summary>
        public List<int> AttachmentIds { get; set; } = new List<int>();

        /// <summary>
        /// Indicates whether to save the resolution as draft (true) or send for confirmation (false)
        /// Based on Sprint.md requirements for JDWA-568:
        /// - Save: Status = "completing data"
        /// - Send: Status = "waiting for confirmation", sends notifications
        /// </summary>
        public bool SaveAsDraft { get; set; } = false;
    }
}
