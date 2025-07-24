namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Response DTO for resolution list operations
    /// Contains collection of resolutions with metadata
    /// Based on requirements in Sprint.md for resolution listing
    /// </summary>
    public record ResolutionsListResponse
    {
        /// <summary>
        /// Collection of resolutions
        /// </summary>
        public IEnumerable<ResolutionDto> Resolutions { get; set; } = [];

        /// <summary>
        /// Total count of resolutions
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Count of draft resolutions
        /// </summary>
        public int DraftCount { get; set; }

        /// <summary>
        /// Count of approved resolutions
        /// </summary>
        public int ApprovedCount { get; set; }

        /// <summary>
        /// Count of pending resolutions
        /// </summary>
        public int PendingCount { get; set; }

        /// <summary>
        /// Fund name for context
        /// </summary>
        public string FundName { get; set; } = string.Empty;
    }
}
