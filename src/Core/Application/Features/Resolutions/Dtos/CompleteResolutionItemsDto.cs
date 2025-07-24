using System.ComponentModel.DataAnnotations;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for completing resolution items and conflicts
    /// Used in CompleteResolutionItems command operations
    /// Based on requirements in Sprint.md (JDWA-507)
    /// </summary>
    public record CompleteResolutionItemsDto
    {
        /// <summary>
        /// Resolution identifier
        /// </summary>
        [Required]
        public int ResolutionId { get; set; }

        /// <summary>
        /// Collection of resolution items to be added/updated
        /// </summary>
        public List<ResolutionItemDto> ResolutionItems { get; set; } = new List<ResolutionItemDto>();

        /// <summary>
        /// Indicates whether to save the resolution as draft (true) or send for confirmation (false)
        /// Based on Sprint.md requirements for JDWA-507:
        /// - Save: Status = "completing data"
        /// - Send: Status = "waiting for confirmation", sends notifications
        /// </summary>
        public bool SaveAsDraft { get; set; } = false;
    }
}
