using Domain.Entities.ResolutionManagement;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for adding a new resolution
    /// Inherits from ResolutionDto following Clean DTOs template patterns
    /// Used in CreateResolution command operations
    /// Based on requirements in Sprint.md (JDWA-511)
    /// </summary>
    public record AddResolutionDto : ResolutionDto
    {
        /// <summary>
        /// Indicates whether to save the resolution as draft (true) or send for review (false)
        /// Based on Sprint.md requirements for JDWA-511:
        /// - Save as Draft: Status = "draft"
        /// - Send: Status = "pending", sends notifications to legal council/board secretary
        /// </summary>
        public bool SaveAsDraft { get; set; } = false;

        /// <summary>
        /// Optional ID of the original resolution for Alternative 2 functionality
        /// Used when creating new resolution from approved/not approved resolution
        /// Based on Sprint.md requirements for JDWA-566, JDWA-567 Alternative 2
        /// </summary>
        public int? OriginalResolutionId { get; set; } = null;

        // All other properties inherited from ResolutionDto
        // Audit fields (CreatedAt, CreatedBy) are handled by the audit system
    }

}