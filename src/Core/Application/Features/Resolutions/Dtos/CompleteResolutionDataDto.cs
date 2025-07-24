using System.ComponentModel.DataAnnotations;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for completing resolution data (basic info)
    /// Used in CompleteResolutionData command operations
    /// Based on requirements in Sprint.md (JDWA-506)
    /// </summary>
    public record CompleteResolutionDataDto : ResolutionDto
    {
        /// <summary>
        /// Indicates whether to save the resolution as draft (true) or send for confirmation (false)
        /// Based on Sprint.md requirements for JDWA-506:
        /// - Save: Status = "completing data"
        /// - Send: Status = "waiting for confirmation", sends notifications
        /// </summary>
        public bool SaveAsDraft { get; set; } = false;
    }
}
