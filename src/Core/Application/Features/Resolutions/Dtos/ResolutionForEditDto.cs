using System.Text.Json.Serialization;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Lightweight Data Transfer Object for resolution edit screen
    /// Contains only the essential fields needed for editing a resolution
    /// Minimizes response size compared to full SingleResolutionResponseView
    /// Based on EditResolutionDto requirements but optimized for API response
    /// </summary>
    public record ResolutionForEditDto : ResolutionDto
    {
        /// <summary>
        /// Collection of current resolution items for editing
        /// Includes items with their conflicts for the edit screen
        /// </summary>
        public List<ResolutionItemDto> ResolutionItems { get; set; } = new();

        /// <summary>
        /// Main attachment for the resolution
        /// Contains the primary document details
        /// </summary>
        public AttachmentDto? Attachment { get; set; } = null;

        /// <summary>
        /// Collection of other attachments associated with the resolution
        /// Contains additional supporting documents
        /// </summary>
        public List<AttachmentDto> OtherAttachments { get; set; } = new();

        /// <summary>
        /// Exclude AttachmentId from JSON serialization since we provide full attachment details
        /// </summary>
        [JsonIgnore]
        public new int AttachmentId { get; set; }

        /// <summary>
        /// Indicates if current user can download attachments
        /// </summary>
        [JsonIgnore]
        public bool CanDownloadAttachments { get; set; } = true;
        public DateTime FundInitiationDate { get; set; }
        public string StatusName { get; set; }
        public string ParentResolutionCode { get; set; }

        // All other core resolution properties inherited from ResolutionDto:
        // Id, Code, ResolutionDate, Description, ResolutionTypeId, NewType,
        // VotingType, MemberVotingResult, Status, FundId,
        // ParentResolutionId, OldResolutionCode

        // Note: This DTO excludes display-only properties like:
        // - FundName (frontend can get from FundId if needed)
        // - ResolutionType object (frontend can get from ResolutionTypeId)
        // - Computed properties like ItemsCount, LastUpdated
        // - Permission flags (can be computed separately if needed)
        // But includes separate main attachment and other attachments for edit screen functionality
    }
}
