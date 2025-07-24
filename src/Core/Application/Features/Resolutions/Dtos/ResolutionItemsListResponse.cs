namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Response DTO for resolution item list operations
    /// Contains collection of resolution items with metadata
    /// </summary>
    public record ResolutionItemsListResponse
    {
        /// <summary>
        /// Collection of resolution items
        /// </summary>
        public IEnumerable<ResolutionItemDto> Items { get; set; } = new List<ResolutionItemDto>();

        /// <summary>
        /// Total count of items
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Count of items with conflicts
        /// </summary>
        public int ConflictItemsCount { get; set; }

        /// <summary>
        /// Resolution code for context
        /// </summary>
        public string ResolutionCode { get; set; } = string.Empty;

        /// <summary>
        /// Resolution status for context
        /// </summary>
        public string ResolutionStatus { get; set; } = string.Empty;
    }
}
