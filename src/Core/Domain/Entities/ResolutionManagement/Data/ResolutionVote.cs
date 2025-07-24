using Domain.Entities.Base;
using Domain.Entities.FundManagement;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Represents a vote cast by a board member on a resolution or resolution item
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Based on voting requirements for resolution management system
    /// </summary>
    public class ResolutionVote : FullAuditedEntity
    {
        /// <summary>
        /// Resolution identifier that this vote belongs to
        /// Foreign key reference to Resolution entity
        /// </summary>
        public int ResolutionId { get; set; }

        /// <summary>
        /// Resolution item identifier if this vote is for a specific item
        /// Foreign key reference to ResolutionItem entity
        /// Null if this is a vote on the resolution as a whole
        /// </summary>
        public int? ResolutionItemId { get; set; }

        /// <summary>
        /// Board member identifier who cast this vote
        /// Foreign key reference to BoardMember entity
        /// </summary>
        public int BoardMemberId { get; set; }

        /// <summary>
        /// The vote value (Approve, Reject, Abstain)
        /// </summary>
        public VoteValue VoteValue { get; set; }

        /// <summary>
        /// Date and time when the vote was cast
        /// Automatically set when vote is recorded
        /// </summary>
        //public DateTime VotingDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Optional comments or notes about the vote
        /// Can be used to explain the reasoning behind the vote
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Indicates if this vote is active
        /// Used for vote suspension scenarios - when resolution is edited during voting,
        /// existing votes are marked as inactive and new voting session starts
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Voting session identifier to group votes from the same voting period
        /// When votes are suspended and restarted, a new session ID is generated
        /// </summary>
        //public Guid VotingSessionId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// IP address from which the vote was cast (for audit purposes)
        /// Optional field for additional security tracking
        /// </summary>
        //public string? IpAddress { get; set; }

        /// <summary>
        /// User agent information from the voting session
        /// Optional field for additional audit information
        /// </summary>
        //public string? UserAgent { get; set; }

        /// <summary>
        /// Navigation property to Resolution entity
        /// Provides access to the resolution being voted on
        /// </summary>
        [ForeignKey("ResolutionId")]
        public  Resolution Resolution { get; set; } = null!;

        /// <summary>
        /// Navigation property to ResolutionItem entity
        /// Provides access to the specific item being voted on (if applicable)
        /// </summary>
        [ForeignKey("ResolutionItemId")]
        public  ResolutionItem? ResolutionItem { get; set; }

        /// <summary>
        /// Navigation property to BoardMember entity
        /// Provides access to the board member who cast the vote
        /// </summary>
        [ForeignKey("BoardMemberId")]
        public  BoardMember BoardMember { get; set; } = null!;

        /// <summary>
        /// Checks if this vote is for the resolution as a whole (not a specific item)
        /// </summary>
        /// <returns>True if voting on resolution, false if voting on specific item</returns>
        public bool IsResolutionVote()
        {
            return ResolutionItemId == null;
        }

        /// <summary>
        /// Checks if this vote is for a specific resolution item
        /// </summary>
        /// <returns>True if voting on specific item, false if voting on resolution</returns>
        public bool IsItemVote()
        {
            return ResolutionItemId.HasValue;
        }

        /// <summary>
        /// Gets a formatted description of the vote
        /// </summary>
        /// <returns>Human-readable vote description</returns>
        public string GetVoteDescription()
        {
            var target = IsItemVote() ? $"Item {ResolutionItemId}" : "Resolution";
            return $"{VoteValue} on {target} by Board Member {BoardMemberId}";
        }
    }
}
