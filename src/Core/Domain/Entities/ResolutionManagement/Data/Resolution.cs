using Domain.Entities.Base;
using Domain.Entities.FundManagement;
using Domain.Entities.Shared;
using Domain.Entities.ResolutionManagement.State;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Represents a resolution entity for fund resolution management
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Based on requirements in Sprint.md for resolution management (JDWA-511, JDWA-588, etc.)
    /// </summary>
    public class Resolution : FullAuditedEntity
    {
        #region Entity Properties
        /// <summary>
        /// Auto-generated resolution code
        /// Format: fund code/resolution year/resolution no.
        /// Example: FUND001/2024/001
        /// </summary>
        public string Code { get; set; } = string.Empty;

        /// <summary>
        /// Date of the resolution
        /// Must be between fund initiation date and today
        /// Supports both Gregorian and Hijri calendars
        /// </summary>
        public DateTime ResolutionDate { get; set; }

        /// <summary>
        /// Description of the resolution
        /// Optional field, maximum 500 characters
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Resolution type identifier
        /// Foreign key reference to ResolutionType entity
        /// </summary>
        public int ResolutionTypeId { get; set; }

        /// <summary>
        /// Custom resolution type name when "Other" is selected
        /// Required when ResolutionType is "Other"
        /// </summary>
        public string? NewType { get; set; }

        public int AttachmentId { get; set; }

        /// <summary>
        /// Voting methodology for the resolution
        /// Uses existing VotingType enum (AllMembers or Majority)
        /// </summary>
        public VotingType VotingType { get; set; }

        /// <summary>
        /// How member voting results are calculated
        /// Default is MajorityOfItems
        /// </summary>
        public MemberVotingResult MemberVotingResult { get; set; } = MemberVotingResult.MajorityOfItems;

        /// <summary>
        /// Current status of the resolution
        /// Tracks the resolution through its lifecycle
        /// </summary>
        public ResolutionStatusEnum Status { get; set; } = ResolutionStatusEnum.Draft;

        /// <summary>
        /// Fund identifier that this resolution belongs to
        /// Foreign key reference to Fund entity
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Parent resolution identifier for tracking resolution relationships
        /// Used when editing approved/not approved resolutions creates new related resolutions
        /// Foreign key reference to another Resolution entity
        /// </summary>
        public int? ParentResolutionId { get; set; }

        /// <summary>
        /// Old resolution code when this resolution is created as an update to an existing one
        /// Used to maintain reference to the original resolution
        /// </summary>
        public string? OldResolutionCode { get; set; }

        /// <summary>
        /// Rejection reason when resolution status is "Rejected"
        /// Required when fund manager rejects a resolution
        /// </summary>
        //public string? RejectionReason { get; set; }

        /// <summary>
        /// Navigation property to Fund entity
        /// Provides access to the fund this resolution belongs to
        /// </summary>
        [ForeignKey("FundId")]
        public  Fund Fund { get; set; } = null!;

        /// <summary>
        /// Navigation property to ResolutionType entity
        /// Provides access to the resolution type information
        /// </summary>
        [ForeignKey("ResolutionTypeId")]
        public  ResolutionType ResolutionType { get; set; } = null!;

        /// <summary>
        /// Navigation property to parent Resolution entity
        /// Provides access to the parent resolution if this is a related resolution
        /// </summary>
        [ForeignKey("ParentResolutionId")]
        public  Resolution? ParentResolution { get; set; }

        /// <summary>
        /// Collection navigation property to child Resolution entities
        /// Represents resolutions that are related to this resolution
        /// </summary>
        public  ICollection<Resolution> ChildResolutions { get; set; } = new List<Resolution>();

        /// <summary>
        /// Collection navigation property to ResolutionItem entities
        /// Represents all items/agenda points for this resolution
        /// </summary>
        public  ICollection<ResolutionItem> ResolutionItems { get; set; } = new List<ResolutionItem>();

        [ForeignKey("AttachmentId")]
        public Attachment Attachment { get; set; }
        public List<ResolutionAttachment> OtherAttachments { get; set; } = new List<ResolutionAttachment>();

        /// <summary>
        /// Collection navigation property to ResolutionHistory entities
        /// Represents all history entries for this resolution
        /// </summary>
        public ICollection<ResolutionStatusHistory> ResolutionStatusHistories { get; set; } = new List<ResolutionStatusHistory>();

        /// <summary>
        /// Collection navigation property to ResolutionVote entities
        /// Represents all votes cast on this resolution and its items
        /// </summary>
        public  ICollection<ResolutionVote> Votes { get; set; } = new List<ResolutionVote>();
        #endregion


        public ResolutionStatus ResolutionStatus
        {
            get => CurrentStatus();
        }
        private ResolutionStatus CurrentStatus()
        {
            return ResolutionStatusHistories?.OrderByDescending(x => x.CreatedAt)?.FirstOrDefault()?.ResolutionStatus ?? new();
        }

        #region State Pattern Implementation

        [NotMapped]
        private ResolutionStateContext _stateContext;

        [NotMapped]
        public ResolutionStateContext StateContext => _stateContext ??= new ResolutionStateContext(this);

        /// <summary>
        /// Initializes the state pattern context
        /// Should be called after loading from database
        /// </summary>
        public void InitializeState()
        {
            _stateContext = new ResolutionStateContext(this);
        }

        /// <summary>
        /// Transitions to a new status using the State Pattern
        /// Validates the transition before applying it
        /// </summary>
        /// <param name="targetStatus">Target status to transition to</param>
        /// <param name="action">Action being performed</param>
        /// <param name="reason">Reason for the transition (for audit trail)</param>
        /// <returns>True if transition was successful, false if not allowed</returns>
        public bool ChangeStatus(ResolutionStatusEnum targetStatus, ResolutionActionEnum action, string reason = "")
        {
            return StateContext.TransitionTo(targetStatus, action, reason);
        }

        /// <summary>
        /// Transitions to a new status with comprehensive audit logging
        /// </summary>
        /// <param name="targetStatus">Target status to transition to</param>
        /// <param name="action">Action being performed</param>
        /// <param name="reason">Reason for the transition</param>
        /// <param name="localizedActionName">Localized action name for audit trail</param>
        /// <param name="userId">User ID performing the action</param>
        /// <param name="userRole">User role performing the action</param>
        /// <param name="actionDetails">Additional action details</param>
        /// <returns>True if transition was successful, false if not allowed</returns>
        public bool ChangeStatusWithAudit(ResolutionStatusEnum targetStatus, ResolutionActionEnum action,
            string reason, string localizedActionName, int userId, string userRole, string actionDetails = "",string rejectionReason = "")
        {
            return StateContext.TransitionToWithAudit(targetStatus, action, reason, localizedActionName,
                userId, userRole, actionDetails, rejectionReason);
        }

        /// <summary>
        /// Adds an audit entry for non-status-changing actions
        /// </summary>
        /// <param name="action">Action being performed</param>
        /// <param name="reason">Reason for the action</param>
        /// <param name="localizedActionName">Localized action name for audit trail</param>
        /// <param name="userId">User ID performing the action</param>
        /// <param name="userRole">User role performing the action</param>
        /// <param name="actionDetails">Additional action details</param>
        public void AddAuditEntry(ResolutionActionEnum action, string reason, string localizedActionName,
            int userId, string userRole, string actionDetails = "")
        {
            StateContext.AddAuditEntry(action, reason, localizedActionName, userId, userRole, actionDetails);
        }

        /// <summary>
        /// Validates if a transition is allowed from current state
        /// </summary>
        /// <param name="targetStatus">Target status to validate</param>
        /// <returns>True if transition is allowed</returns>
        public bool CanTransitionTo(ResolutionStatusEnum targetStatus)
        {
            return StateContext.CanTransitionTo(targetStatus);
        }

        /// <summary>
        /// Gets all allowed transitions from current state
        /// </summary>
        /// <returns>Collection of allowed target statuses</returns>
        public IEnumerable<ResolutionStatusEnum> GetAllowedTransitions()
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
        /// Validates if cancellation is allowed in current state
        /// </summary>
        /// <returns>True if cancellation is allowed</returns>
        public bool CanCancel()
        {
            return StateContext.CanCancel();
        }

        /// <summary>
        /// Gets validation messages for current state
        /// </summary>
        /// <returns>Collection of validation messages</returns>
        public IEnumerable<string> GetStateValidationMessages()
        {
            return StateContext.GetValidationMessages();
        }

        #endregion
    }
}
