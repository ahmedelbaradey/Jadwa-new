using Domain.Entities.Base;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MeetingManagement
{
    /// <summary>
    /// Represents a vote cast by a board member on a proposed meeting time
    /// Each board member can vote once per proposal
    /// Based on User Story 2 requirements from Meetings.md
    /// </summary>
    public class MeetingTimeVote : FullAuditedEntity
    {
        /// <summary>
        /// Meeting time proposal identifier that this vote belongs to
        /// Foreign key reference to MeetingTimeProposal entity
        /// </summary>
        public int ProposalId { get; set; }

        /// <summary>
        /// User identifier of the board member who cast this vote
        /// Foreign key reference to User entity
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Proposed date identifier that this vote is for
        /// Foreign key reference to ProposedDate entity
        /// </summary>
        public int ProposedDateId { get; set; }

        /// <summary>
        /// Timestamp when the vote was cast
        /// Automatically set when vote is submitted
        /// </summary>
        public DateTime VoteTimestamp { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Navigation property to MeetingTimeProposal entity
        /// Provides access to the proposal this vote belongs to
        /// </summary>
        [ForeignKey("ProposalId")]
        public MeetingTimeProposal Proposal { get; set; } = null!;

        /// <summary>
        /// Navigation property to User entity (voter)
        /// Provides access to the board member who cast this vote
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        /// <summary>
        /// Navigation property to ProposedDate entity
        /// Provides access to the specific proposed date this vote is for
        /// </summary>
        [ForeignKey("ProposedDateId")]
        public ProposedDate ProposedDate { get; set; } = null!;
    }
}
