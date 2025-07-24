using Domain.Entities.Base;
using Domain.Entities.Users;

namespace Domain.Entities.AssessmentManagement
{
    /// <summary>
    /// Represents the status history of an assessment for comprehensive audit trail
    /// Follows the same pattern as ResolutionStatusHistory for consistency
    /// Provides detailed tracking of all assessment state transitions and actions
    /// </summary>
    public class AssessmentStatusHistory : CreationAuditedEntity
    {
        /// <summary>
        /// Foreign key reference to the Assessment entity
        /// </summary>
        public int AssessmentId { get; set; }

        /// <summary>
        /// Assessment status ID at the time of this history entry
        /// </summary>
        public int AssessmentStatusId { get; set; }

        /// <summary>
        /// Action that was performed (enum value)
        /// </summary>
        public AssessmentActionEnum Action { get; set; }

        /// <summary>
        /// Reason for the action/transition
        /// </summary>
        public string? Reason { get; set; }

        /// <summary>
        /// Rejection reason (specific for rejection actions)
        /// </summary>
        public string? RejectionReason { get; set; }

        /// <summary>
        /// Comprehensive description of the operation performed
        /// </summary>
        public string? ActionDetails { get; set; }

        /// <summary>
        /// Localization key reference (NOT translated text) for retrieval-time localization
        /// Follows notification pattern for multilingual support
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// User role performing the action
        /// </summary>
        public string? UserRole { get; set; }

        /// <summary>
        /// Previous status (for status changes)
        /// </summary>
        public AssessmentStatus? PreviousStatus { get; set; }

        /// <summary>
        /// New status (for status changes)
        /// </summary>
        public AssessmentStatus? NewStatus { get; set; }

        /// <summary>
        /// Navigation property to the Assessment entity
        /// </summary>
        public virtual Assessment Assessment { get; set; } = null!;

        /// <summary>
        /// Navigation property to the User who created this history entry
        /// </summary>
        public virtual User? CreatedByUser { get; set; }
    }

    /// <summary>
    /// Enum for assessment actions for audit logging
    /// Matches the AssessmentActionEnum in AssessmentStateContext
    /// </summary>
    public enum AssessmentActionEnum
    {
        Creation = 1,
        Submission = 2,
        Approval = 3,
        Rejection = 4,
        Distribution = 5,
        ResponseSubmission = 6,
        Completion = 7,
        StatusChange = 8,
        Edit = 9,
        Delete = 10,
        Archive = 11
    }
}
