using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MeetingManagement
{
    /// <summary>
    /// Represents a proposed date and time option for a meeting time proposal
    /// Each proposal can have 1-4 proposed date/time options
    /// Based on User Story 1 requirements from Meetings.md
    /// </summary>
    public class ProposedDate : FullAuditedEntity
    {
        /// <summary>
        /// Meeting time proposal identifier that this date belongs to
        /// Foreign key reference to MeetingTimeProposal entity
        /// </summary>
        public int ProposalId { get; set; }

        /// <summary>
        /// Proposed date and time for the meeting
        /// Must be in the future
        /// </summary>
        public DateTime ProposedDateTime { get; set; }

        /// <summary>
        /// Navigation property to MeetingTimeProposal entity
        /// Provides access to the parent proposal
        /// </summary>
        [ForeignKey("ProposalId")]
        public MeetingTimeProposal Proposal { get; set; } = null!;

        /// <summary>
        /// Collection navigation property to MeetingTimeVote entities
        /// Represents all votes cast for this specific proposed date
        /// </summary>
        public virtual ICollection<MeetingTimeVote> Votes { get; set; } = new List<MeetingTimeVote>();
    }
}
