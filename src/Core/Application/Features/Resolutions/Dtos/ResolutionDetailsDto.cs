namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for resolution details response
    /// Includes additional information and related entities
    /// Based on requirements in Sprint.md for detailed resolution views
    /// </summary>
    public record ResolutionDetailsDto : ResolutionDto
    {
        /// <summary>
        /// Collection of resolution items
        /// </summary>
        public IEnumerable<ResolutionItemDto> Items { get; set; } = [];

        /// <summary>
        /// Collection of additional attachments
        /// </summary>
        public IEnumerable<AttachmentDto> OtherAttachments { get; set; } = [];

        /// <summary>
        /// Collection of status history entries
        /// </summary>
        public IEnumerable<ResolutionStatusHistoryDto> StatusHistory { get; set; } = [];

        /// <summary>
        /// Collection of votes (if applicable)
        /// </summary>
        public IEnumerable<ResolutionVoteDto> Votes { get; set; } = [];

        /// <summary>
        /// Parent resolution information (if applicable)
        /// </summary>
        public ResolutionDto? ParentResolution { get; set; }

        /// <summary>
        /// Child resolutions (if applicable)
        /// </summary>
        public IEnumerable<ResolutionDto> ChildResolutions { get; set; } = [];

        /// <summary>
        /// Created by user information
        /// </summary>
        public string CreatedByUser { get; set; } = string.Empty;

        /// <summary>
        /// Creation date
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }
}
