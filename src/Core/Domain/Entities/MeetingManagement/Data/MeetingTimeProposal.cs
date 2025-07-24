using Domain.Entities.Base;
using Domain.Entities.FundManagement;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MeetingManagement
{
    /// <summary>
    /// Represents a meeting time proposal entity for voting on meeting schedules
    /// Allows Legal Counsel and Board Secretary to propose multiple time options
    /// Based on User Story 1 requirements from Meetings.md
    /// </summary>
    public class MeetingTimeProposal : FullAuditedEntity
    {
        /// <summary>
        /// Fund identifier that this proposal belongs to
        /// Foreign key reference to Fund entity
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Subject of the meeting proposal
        /// Required field, maximum 255 characters
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the meeting proposal
        /// Maximum 1000 characters
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Status of the proposal
        /// Values: "Under Voting", "Completed"
        /// </summary>
        public string Status { get; set; } = "Under Voting";

        /// <summary>
        /// User ID of the person who created this proposal
        /// Must be Legal Counsel or Board Secretary
        /// </summary>
        public int CreatedByUserId { get; set; }

        /// <summary>
        /// Navigation property to Fund entity
        /// Provides access to the fund this proposal belongs to
        /// </summary>
        [ForeignKey("FundId")]
        public Fund Fund { get; set; } = null!;

        /// <summary>
        /// Navigation property to User entity (creator)
        /// Provides access to the user who created this proposal
        /// </summary>
        [ForeignKey("CreatedByUserId")]
        public User CreatedByUser { get; set; } = null!;

        /// <summary>
        /// Collection navigation property to ProposedDate entities
        /// Represents all proposed date/time options for this proposal
        /// Minimum 1, Maximum 4 proposed dates allowed
        /// </summary>
        public virtual ICollection<ProposedDate> ProposedDates { get; set; } = new List<ProposedDate>();

        /// <summary>
        /// Collection navigation property to MeetingTimeVote entities
        /// Represents all votes cast on this proposal
        /// </summary>
        public virtual ICollection<MeetingTimeVote> Votes { get; set; } = new List<MeetingTimeVote>();

        /// <summary>
        /// Attachment ID for proposal attachments (PDFs only)
        /// Uses the established AttachmentId pattern instead of storing file paths
        /// </summary>
        public int? AttachmentId { get; set; }

        /// <summary>
        /// Navigation property to Attachment entity
        /// Provides access to attached files for this proposal
        /// </summary>
        [ForeignKey("AttachmentId")]
        public Domain.Entities.Shared.Attachment? Attachment { get; set; }
    }
}
