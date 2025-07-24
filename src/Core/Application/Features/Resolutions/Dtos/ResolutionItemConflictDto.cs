using Abstraction.Base.Dto;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for resolution item conflict information
    /// Contains board member conflict details
    /// Based on requirements in Sprint.md for conflict management
    /// </summary>
    public record ResolutionItemConflictDto : BaseDto
    {
        /// <summary>
        /// Resolution item identifier
        /// </summary>
        public int ResolutionItemId { get; set; }

        /// <summary>
        /// Board member identifier
        /// </summary>
        public int BoardMemberId { get; set; }

        /// <summary>
        /// Optional notes about the nature of the conflict
        /// </summary>
        public string? ConflictNotes { get; set; } = null;

        /// <summary>
        /// Board member name for display
        /// </summary>
        public string BoardMemberName { get; set; } = string.Empty;

        /// <summary>
        /// Board member user name for display
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Board member type for display
        /// </summary>
        public string MemberType { get; set; } = string.Empty;
    }
}
