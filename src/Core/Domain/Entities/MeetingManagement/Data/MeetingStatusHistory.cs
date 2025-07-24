using Domain.Entities.Base;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.MeetingManagement
{
    /// <summary>
    /// Represents the status history of a meeting for audit trail purposes
    /// Tracks all status changes and actions performed on meetings
    /// Based on ResolutionStatusHistory pattern and Clean Architecture principles
    /// Follows the exact same structure as ResolutionStatusHistory
    /// </summary>
    public class MeetingStatusHistory : FullAuditedEntity
    {
        /// <summary>
        /// Meeting identifier that this history entry belongs to
        /// Foreign key reference to Meeting entity
        /// </summary>
        public int MeetingId { get; set; }

        /// <summary>
        /// Meeting status identifier (maps to MeetingStatusEnum)
        /// Represents the new status after the action
        /// </summary>
        public int MeetingStatusId { get; set; }

        /// <summary>
        /// Previous status before the change (optional)
        /// Used for tracking status transitions
        /// </summary>
        public MeetingStatusEnum? PreviousStatus { get; set; }

        /// <summary>
        /// New status after the change
        /// Maps to MeetingStatusEnum values
        /// </summary>
        public MeetingStatusEnum NewStatus { get; set; }

        /// <summary>
        /// Action performed that caused this status change
        /// Maps to MeetingActionEnum values
        /// </summary>
        public MeetingActionEnum Action { get; set; }

        /// <summary>
        /// Reason for the status change or action
        /// Free text field for additional context
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Rejection reason when status is changed to "Cancelled"
        /// Required when meeting is cancelled with specific reason
        /// </summary>
        public string? RejectionReason { get; set; }

        /// <summary>
        /// User role who performed the action
        /// Stores the role context for audit purposes
        /// </summary>
        public string? UserRole { get; set; }

        /// <summary>
        /// User identifier who performed the action
        /// Foreign key reference to User entity
        /// </summary>
        public int ChangedBy { get; set; }

        /// <summary>
        /// Timestamp when the action was performed
        /// Automatically set when status change occurs
        /// </summary>
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Additional comments or notes about the action
        /// Free text field for detailed information
        /// </summary>
        public string? Comments { get; set; }

        /// <summary>
        /// Localization key reference for the action performed
        /// Stores SharedResourcesKey constants (NOT translated text) following notification pattern
        /// Examples: "AuditActionMeetingCreation", "AuditActionMeetingStart"
        /// Localization occurs on retrieval using IAuditLocalizationService
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Additional details about the action in JSON format
        /// Can store complex action-specific data
        /// </summary>
        public string? ActionDetails { get; set; }

        /// <summary>
        /// Navigation property to Meeting entity
        /// Provides access to the parent meeting
        /// </summary>
        [ForeignKey("MeetingId")]
        public Meeting Meeting { get; set; } = null!;

        /// <summary>
        /// Navigation property to User entity (who changed the status)
        /// Provides access to the user who performed the action
        /// </summary>
        [ForeignKey("ChangedBy")]
        public User ChangedByUser { get; set; } = null!;
    }
}
