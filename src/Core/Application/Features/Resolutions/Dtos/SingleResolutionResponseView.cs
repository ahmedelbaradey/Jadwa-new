namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Single resolution response DTO following Clean DTOs template patterns
    /// Used for single entity API responses with display properties
    /// Based on requirements in Sprint.md for resolution display
    /// </summary>
    public record SingleResolutionResponseView : ResolutionDto
    {
        /// <summary>
        /// Fund name for display
        /// </summary>
        public string FundName { get; set; } = string.Empty;

        /// <summary>
        /// Resolution type information for display
        /// </summary>
        public ResolutionTypeDto? ResolutionType { get; set; }

        /// <summary>
        /// Number of resolution items
        /// </summary>
        public int ItemsCount { get; set; }

        /// <summary>
        /// Last update date for sorting and display
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        /// Resolution status identifier
        /// Numeric value of the resolution status enum
        /// </summary>
        public int StatusId { get; set; }

        /// <summary>
        /// Localized display text for resolution status
        /// </summary>
        public string StatusDisplay { get; set; } = string.Empty;

        /// <summary>
        /// Resolution status information with localization
        /// </summary>
        public ResolutionStatusDto? ResolutionStatus { get; set; }

        /// <summary>
        /// Localized display text for voting type
        /// </summary>
        public string VotingTypeDisplay { get; set; } = string.Empty;

        /// <summary>
        /// Localized display text for member voting result
        /// </summary>
        public string MemberVotingResultDisplay { get; set; } = string.Empty;

        // Enhanced properties for advanced statuses (JDWA-593, JDWA-589)
        /// <summary>
        /// Collection of resolution items with conflict information
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
        /// Collection of status history entries for audit trail
        /// </summary>
        //public List<ResolutionStatusHistoryDto> StatusHistory { get; set; } = new();

        /// <summary>
        /// Rejection reason for rejected resolutions
        /// </summary>
        public string? RejectionReason { get; set; }

        /// <summary>
        /// Parent resolution code for referral resolutions
        /// </summary>
        public string? ParentResolutionCode { get; set; }

        // Role-based action availability
        /// <summary>
        /// Indicates if current user can confirm the resolution
        /// </summary>
        public bool CanConfirm { get; set; }

        /// <summary>
        /// Indicates if current user can reject the resolution
        /// </summary>
        public bool CanReject { get; set; }

        /// <summary>
        /// Indicates if current user can edit the resolution
        /// </summary>
        public bool CanEdit { get; set; }

        /// <summary>
        /// Indicates if current user can cancel the resolution
        /// </summary>
        public bool CanCancel { get; set; }

        /// <summary>
        /// Indicates if current user can delete the resolution
        /// </summary>
        public bool CanDelete { get; set; }
       
        public bool CanView { get; set; }

        /// <summary>
        /// Indicates if current user can download attachments
        /// </summary>
        public bool CanDownloadAttachments { get; set; } = true;
    }
}
