namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Consolidated Data Transfer Object for editing an existing resolution
    /// Combines basic resolution info, items, and attachments editing capabilities
    /// Inherits from ResolutionDto following Clean DTOs template patterns
    /// Used in consolidated EditResolution command operations
    /// Based on requirements in Sprint.md user stories:
    /// - JDWA-505: Complete Resolution Attachments
    /// - JDWA-506: Complete Resolution Basic Information
    /// - JDWA-507: Complete Resolution Resolution Items
    /// - JDWA-566: Edit Resolution Resolution Items
    /// - JDWA-567: Edit Resolution Basic Information
    /// - JDWA-568: Edit Resolution Attachments
    /// </summary>
    public record EditResolutionDto : ResolutionDto
    {
        /// <summary>
        /// Indicates whether to save the resolution as draft (true) or send for review (false)
        /// Based on Sprint.md requirements:
        /// - Save as Draft: Status = "draft" or "completing data"
        /// - Send: Status = "pending" or "waiting for confirmation", sends notifications
        /// </summary>
        public bool SaveAsDraft { get; set; } = false;

        /// <summary>
        /// Collection of resolution items to be updated
        /// Supports add, edit, and delete operations for resolution items
        /// Based on JDWA-507 and JDWA-566 requirements
        /// </summary>
        public List<ResolutionItemDto> ResolutionItems { get; set; } = new();

        /// <summary>
        /// Collection of attachment IDs to be associated with the resolution
        /// Supports add and remove operations for resolution attachments
        /// Maximum 10 attachments allowed per Sprint.md requirements
        /// Based on JDWA-505 and JDWA-568 requirements
        /// </summary>
        public List<int> AttachmentIds { get; set; } = new();

        // All other properties inherited from ResolutionDto (basic resolution info)
        // Id property from BaseDto is used for identifying the entity to update
        // Audit fields (UpdatedAt, UpdatedBy) are handled by the audit system
    }
}
