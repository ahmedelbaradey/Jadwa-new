using Abstraction.Base.Dto;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.ResolutionManagement;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for resolution vote information
    /// Contains board member voting details
    /// Based on requirements in Sprint.md for voting management
    /// </summary>
    public record ResolutionVoteDto : BaseDto
    {
        /// <summary>
        /// Resolution identifier
        /// </summary>
        public int ResolutionId { get; set; }

        /// <summary>
        /// Board member identifier who cast the vote
        /// </summary>
        public int BoardMemberId { get; set; }

        /// <summary>
        /// Vote value (Approve, Reject, Abstain)
        /// </summary>
        public VoteValue VoteValue { get; set; }

        /// <summary>
        /// Optional comments about the vote
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Date and time when the vote was cast
        /// </summary>
        public DateTime VotedAt { get; set; }

        /// <summary>
        /// Board member name for display
        /// </summary>
        public string BoardMemberName { get; set; } = string.Empty;

        /// <summary>
        /// User name for display
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// User full name for display
        /// </summary>
        public string UserFullName { get; set; } = string.Empty;

        /// <summary>
        /// Board member type for display
        /// </summary>
        public string MemberType { get; set; } = string.Empty;

        /// <summary>
        /// Vote value display name (localized)
        /// </summary>
        public string VoteValueDisplay { get; set; } = string.Empty;

        /// <summary>
        /// Formatted vote date for display
        /// </summary>
        public string FormattedVoteDate { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if this member is the chairman
        /// </summary>
        public bool IsChairman { get; set; }
    }
}
