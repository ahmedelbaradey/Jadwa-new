using Abstraction.Base.Dto;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for ResolutionItem entity
    /// Contains properties for resolution items within a resolution
    /// Based on requirements in Sprint.md for resolution item management
    /// </summary>
    public record ResolutionItemDto : BaseDto
    {
        /// <summary>
        /// Resolution identifier that this item belongs to
        /// </summary>
        public int ResolutionId { get; set; }

        /// <summary>
        /// Auto-generated title (Item1, Item2, etc.)
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the resolution item (max 500 characters)
        /// </summary>
        public string? Description { get; set; } = null;

        /// <summary>
        /// Indicates if there is a conflict of interest with board members
        /// </summary>
        public bool HasConflict { get; set; }

        /// <summary>
        /// Display order for sorting resolution items
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Collection of board members with conflicts for this item
        /// </summary>
        public IEnumerable<ResolutionItemConflictDto> ConflictMembers { get; set; } = [];

        /// <summary>
        /// Count of conflicted members for display
        /// </summary>
        public int ConflictMembersCount { get; set; }
    }
}
