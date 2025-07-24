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

        /// <summary>
        /// Gets the current assessment status following Resolution pattern
        /// Returns the current AssessmentStatus value
        /// </summary>
        public AssessmentStatus AssessmentStatus
        {
            get => CurrentStatus();
        }

        /// <summary>
        /// Private method to get current status following Resolution.CurrentStatus() pattern
        /// Returns the current status from the Status property
        /// </summary>
        private AssessmentStatus CurrentStatus()
        {
            return Status;
        }

        #region State Pattern Implementation

        [NotMapped]
        private Application.Services.AssessmentStateContext _stateContext;

        [NotMapped]
        public Application.Services.AssessmentStateContext StateContext => _stateContext;

        /// <summary>
        /// Initializes the state pattern context
        /// Should be called after loading from database with proper dependencies
        /// </summary>
        /// <param name="localizer">String localizer for localized messages</param>
        /// <param name="currentUserService">Current user service for user context</param>
        /// <param name="repository">Repository manager for data access</param>
        public void InitializeState(Microsoft.Extensions.Localization.IStringLocalizer<Resources.SharedResources> localizer,
            Abstraction.Contract.Service.ICurrentUserService currentUserService,
            Abstraction.Contracts.Repository.IRepositoryManager repository)
        {
            _stateContext = new Application.Services.AssessmentStateContext(this, localizer, currentUserService, repository);
        }

        /// <summary>
        /// Transitions to a new status using the State Pattern
        /// Validates the transition before applying it
        /// </summary>
        /// <param name="targetStatus">Target status to transition to</param>
        /// <param name="action">Action being performed</param>
        /// <param name="reason">Reason for the transition (for audit trail)</param>
        /// <returns>True if transition was successful, false if not allowed</returns>
        public bool ChangeStatus(AssessmentStatus targetStatus, AssessmentActionEnum action, string reason = "")
        {
            return StateContext.TransitionTo(targetStatus, action, reason);
        }

        /// <summary>
        /// Transitions to a new status with comprehensive audit logging
        /// Follows the exact same pattern as Resolution.ChangeStatusWithAudit
        /// </summary>
        /// <param name="targetStatus">Target status to transition to</param>
        /// <param name="action">Action being performed</param>
        /// <param name="reason">Reason for the transition</param>
        /// <param name="localizedActionName">Localized action name for audit trail</param>
        /// <param name="userId">User ID performing the action</param>
        /// <param name="userRole">User role performing the action</param>
        /// <param name="actionDetails">Additional action details</param>
        /// <param name="rejectionReason">Rejection reason if applicable</param>
        /// <returns>True if transition was successful, false if not allowed</returns>
        public bool ChangeStatusWithAudit(AssessmentStatus targetStatus, AssessmentActionEnum action,
            string reason, string localizedActionName, int userId, string userRole, string actionDetails = "", string rejectionReason = "")
        {
            return StateContext.TransitionToWithAudit(targetStatus, action, reason, localizedActionName,
                userId, userRole, actionDetails, rejectionReason);
        }

        /// <summary>
        /// Validates if a transition is allowed from current state
        /// </summary>
        /// <param name="targetStatus">Target status to validate</param>
        /// <returns>True if transition is allowed</returns>
        public bool CanTransitionTo(AssessmentStatus targetStatus)
        {
            return StateContext.CanTransitionTo(targetStatus);
        }

        /// <summary>
        /// Gets all allowed transitions from current state
        /// </summary>
        /// <returns>Collection of allowed target statuses</returns>
        public IEnumerable<AssessmentStatus> GetAllowedTransitions()
        {
            return StateContext.GetAllowedTransitions();
        }

        /// <summary>
        /// Validates if editing is allowed in current state
        /// </summary>
        /// <returns>True if editing is allowed</returns>
        public bool CanEdit()
        {
            return StateContext.CanEdit();
        }

        /// <summary>
        /// Validates if completion operations are allowed in current state
        /// </summary>
        /// <returns>True if completion is allowed</returns>
        public bool CanComplete()
        {
            return StateContext.CanComplete();
        }

        /// <summary>
        /// Validates if deletion is allowed in current state
        /// </summary>
        /// <returns>True if deletion is allowed</returns>
        public bool CanDelete()
        {
            return StateContext.CanDelete();
        }

        /// <summary>
        /// Handles state-specific logic
        /// </summary>
        public void Handle()
        {
            StateContext.Handle();
        }

        #endregion
    }
}
