using Domain.Entities.Base;
using Domain.Entities.FundManagement;
using Domain.Entities.Users;
using Domain.Entities.Shared;
using Domain.States.AssessmentStates;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.AssessmentManagement
{
    /// <summary>
    /// Represents an Assessment entity for gathering feedback from board members
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Properties are defined based on requirements in AssessmentStories.md
    /// </summary>
    public class Assessment : FullAuditedEntity
    {
        /// <summary>
        /// Foreign key reference to the Fund this assessment belongs to
        /// Required field as specified in user stories
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Title of the assessment
        /// Required field with maximum 255 characters as specified in user stories
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Type of assessment (Questionnaire or Attachment)
        /// Required field as specified in user stories
        /// </summary>
        public AssessmentType Type { get; set; }

        /// <summary>
        /// Current status of the assessment
        /// Required field for state management
        /// </summary>
        public AssessmentStatus Status { get; set; } = AssessmentStatus.Draft;

        /// <summary>
        /// Foreign key reference to the Attachment entity (required for Attachment type assessments)
        /// Optional field, required only when Type is Attachment
        /// </summary>
        public int? AttachmentId { get; set; }

        /// <summary>
        /// Comments from reviewer when assessment is rejected
        /// Optional field, used for rejection feedback
        /// Maximum 2000 characters as specified in user stories
        /// </summary>
        public string? ReviewerComments { get; set; }

        /// <summary>
        /// User ID of the reviewer who approved/rejected the assessment
        /// Optional field, set when assessment is reviewed
        /// </summary>
        public int? ReviewedBy { get; set; }

        /// <summary>
        /// Date when the assessment was reviewed (approved/rejected)
        /// Optional field, set when assessment is reviewed
        /// </summary>
        public DateTime? ReviewedDate { get; set; }

        /// <summary>
        /// Navigation property to the Fund this assessment belongs to
        /// Provides access to fund information
        /// </summary>
        [ForeignKey("FundId")]
        public virtual Fund Fund { get; set; } = null!;

        /// <summary>
        /// Navigation property to the User who reviewed the assessment
        /// Provides access to reviewer information
        /// </summary>
        [ForeignKey("ReviewedBy")]
        public virtual User? Reviewer { get; set; }

        /// <summary>
        /// Navigation property to the Attachment entity
        /// Provides access to attachment file information
        /// </summary>
        [ForeignKey("AttachmentId")]
        public virtual Attachment? Attachment { get; set; }

        /// <summary>
        /// Collection navigation property to AssessmentQuestion entities
        /// Represents all questions belonging to this assessment
        /// </summary>
        public virtual ICollection<AssessmentQuestion> Questions { get; set; } = new List<AssessmentQuestion>();

        /// <summary>
        /// Collection navigation property to AssessmentResponse entities
        /// Represents all responses submitted for this assessment
        /// </summary>
        public virtual ICollection<AssessmentResponse> Responses { get; set; } = new List<AssessmentResponse>();

        /// <summary>
        /// Collection of status history entries for comprehensive audit trail
        /// Navigation property for one-to-many relationship
        /// </summary>
        public virtual ICollection<AssessmentStatusHistory> StatusHistories { get; set; } = new List<AssessmentStatusHistory>();

        #region State Design Pattern Implementation

        /// <summary>
        /// Current state instance (not mapped to database)
        /// Computed from Status property using State Factory
        /// </summary>
        [NotMapped]
        private IAssessmentState? _currentState;

        /// <summary>
        /// Gets the current state instance
        /// Initializes state from Status if not already set
        /// </summary>
        [NotMapped]
        public IAssessmentState CurrentState => _currentState ??= CreateStateFromStatus(Status);

        /// <summary>
        /// Initializes the state from the current Status
        /// Should be called after loading from database
        /// </summary>
        public void InitializeState()
        {
            _currentState = CreateStateFromStatus(Status);
        }

        /// <summary>
        /// Creates a state instance from the given status
        /// Simple factory method for state creation
        /// </summary>
        /// <param name="status">Assessment status</param>
        /// <returns>Appropriate state instance</returns>
        private static IAssessmentState CreateStateFromStatus(AssessmentStatus status)
        {
            return status switch
            {
                AssessmentStatus.Draft => new DraftState(),
                AssessmentStatus.WaitingForApproval => new WaitingForApprovalState(),
                AssessmentStatus.Approved => new ApprovedState(),
                AssessmentStatus.Rejected => new RejectedState(),
                AssessmentStatus.Active => new ActiveState(),
                AssessmentStatus.Completed => new CompletedState(),
                _ => throw new ArgumentException($"Unsupported assessment status: {status}", nameof(status))
            };
        }

        /// <summary>
        /// Changes the assessment status using state pattern validation
        /// </summary>
        /// <param name="targetStatus">Target status to transition to</param>
        /// <param name="reason">Reason for the status change</param>
        /// <returns>True if transition was successful, false otherwise</returns>
        public bool ChangeStatus(AssessmentStatus targetStatus, string reason)
        {
            if (CurrentState.CanTransitionTo(targetStatus))
            {
                var success = CurrentState.TransitionTo(targetStatus, reason);
                if (success)
                {
                    Status = targetStatus;
                    _currentState = CreateStateFromStatus(targetStatus);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the assessment can transition to the target status
        /// </summary>
        /// <param name="targetStatus">Target status</param>
        /// <returns>True if transition is allowed, false otherwise</returns>
        public bool CanTransitionTo(AssessmentStatus targetStatus)
        {
            return CurrentState.CanTransitionTo(targetStatus);
        }

        /// <summary>
        /// Gets all allowed transitions from the current state
        /// </summary>
        /// <returns>List of allowed target statuses</returns>
        public List<AssessmentStatus> GetAllowedTransitions()
        {
            return CurrentState.GetAllowedTransitions();
        }

        /// <summary>
        /// Gets the available actions for the current state
        /// </summary>
        /// <returns>List of available action enums</returns>
        public List<AssessmentActionEnum> GetAvailableActions()
        {
            return CurrentState.GetAvailableActions();
        }

        /// <summary>
        /// Handles state-specific logic
        /// </summary>
        public void Handle()
        {
            CurrentState.Handle(this);
        }

        #endregion
    }
}
