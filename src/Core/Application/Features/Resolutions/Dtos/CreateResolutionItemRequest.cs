namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for creating a new resolution item
    /// Used in AddResolutionItem command
    /// </summary>
    public record CreateResolutionItemRequest
    {
        /// <summary>
        /// Resolution identifier that this item will belong to
        /// </summary>
        public int ResolutionId { get; set; }

        /// <summary>
        /// Description of the resolution item (max 500 characters)
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Indicates if there is a conflict of interest with board members
        /// </summary>
        public bool HasConflict { get; set; }

        /// <summary>
        /// Collection of board member IDs with conflicts (if HasConflict is true)
        /// </summary>
        public IEnumerable<int> ConflictedMemberIds { get; set; } = new List<int>();

        /// <summary>
        /// Optional notes about the conflicts
        /// </summary>
        public string? ConflictNotes { get; set; }
    }
}
