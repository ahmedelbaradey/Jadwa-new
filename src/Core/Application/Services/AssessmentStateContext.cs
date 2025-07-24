using Domain.Entities.AssessmentManagement;
using Domain.States.AssessmentStates;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository;
using Abstraction.Constants;
using Domain.Entities.FundManagement;

namespace Application.Services
{
    /// <summary>
    /// Context class for managing assessment state transitions
    /// Implements the State Pattern context for assessment lifecycle management
    /// Follows the exact structure and patterns of ResolutionStateContext
    /// </summary>
    public class AssessmentStateContext
    {
        private IAssessmentState _currentState;
        private readonly Assessment _assessment;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRepositoryManager _repository;

        /// <summary>
        /// Initializes a new instance of AssessmentStateContext
        /// </summary>
        /// <param name="assessment">The assessment entity to manage</param>
        /// <param name="localizer">String localizer for localized messages</param>
        /// <param name="currentUserService">Current user service for user context</param>
        /// <param name="repository">Repository manager for data access</param>
        public AssessmentStateContext(
            Assessment assessment,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService,
            IRepositoryManager repository)
        {
            _assessment = assessment ?? throw new ArgumentNullException(nameof(assessment));
            _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
            _currentUserService = currentUserService ?? throw new ArgumentNullException(nameof(currentUserService));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _currentState = AssessmentStateFactory.CreateState(assessment.Status);
        }

        /// <summary>
        /// Gets the current state
        /// </summary>
        public IAssessmentState CurrentState => _currentState;

        /// <summary>
        /// Gets the current assessment
        /// </summary>
        public Assessment Assessment => _assessment;

        #region State Transition Methods

        /// <summary>
        /// Transitions to a new state with validation
        /// </summary>
        /// <param name="targetStatus">Target status to transition to</param>
        /// <param name="action">Action being performed</param>
        /// <param name="reason">Reason for the transition (for audit trail)</param>
        /// <returns>True if transition was successful, false if not allowed</returns>
        public bool TransitionTo(AssessmentStatus targetStatus, AssessmentActionEnum action, string reason = "")
        {
            if (!_currentState.CanTransitionTo(targetStatus))
            {
                return false;
            }

            // Update the assessment status
            _assessment.Status = targetStatus;

            // Create new state instance
            _currentState = AssessmentStateFactory.CreateState(targetStatus);

            // Add status history entry if needed
            AddStatusHistoryEntry(targetStatus, action, reason);

            return true;
        }

        /// <summary>
        /// Validates if a transition is allowed
        /// </summary>
        /// <param name="targetStatus">Target status to validate</param>
        /// <returns>True if transition is allowed</returns>
        public bool CanTransitionTo(AssessmentStatus targetStatus)
        {
            return _currentState.CanTransitionTo(targetStatus);
        }

        /// <summary>
        /// Gets all allowed transitions from current state
        /// </summary>
        /// <returns>Collection of allowed target statuses</returns>
        public IEnumerable<AssessmentStatus> GetAllowedTransitions()
        {
            return _currentState.GetAllowedTransitions();
        }

        /// <summary>
        /// Validates if editing is allowed in current state
        /// </summary>
        /// <returns>True if editing is allowed</returns>
        public bool CanEdit()
        {
            return _currentState.CanEdit();
        }

        /// <summary>
        /// Validates if completion operations are allowed in current state
        /// </summary>
        /// <returns>True if completion is allowed</returns>
        public bool CanComplete()
        {
            return _currentState.CanComplete();
        }

        /// <summary>
        /// Validates if deletion is allowed in current state
        /// </summary>
        /// <returns>True if deletion is allowed</returns>
        public bool CanDelete()
        {
            return _currentState.CanDelete();
        }

        /// <summary>
        /// Gets state-specific validation messages
        /// </summary>
        /// <returns>Collection of validation messages</returns>
        public IEnumerable<string> GetValidationMessages()
        {
            return _currentState.GetValidationMessages();
        }

        /// <summary>
        /// Handles state-specific logic
        /// </summary>
        public void Handle()
        {
            _currentState.Handle(_assessment);
        }

        /// <summary>
        /// Transitions to a new state with comprehensive audit logging
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
        public bool TransitionToWithAudit(AssessmentStatus targetStatus, AssessmentActionEnum action,
            string reason, string localizedActionName, int userId, string userRole, string actionDetails = "", string rejectionReason = "")
        {
            if (!_currentState.CanTransitionTo(targetStatus))
            {
                return false;
            }

            var previousStatus = _assessment.Status;

            // Update the assessment status
            _assessment.Status = targetStatus;

            // Create new state instance
            _currentState = AssessmentStateFactory.CreateState(targetStatus);

            // Add comprehensive audit entry
            AddStatusHistoryEntryWithAudit(targetStatus, action, reason, localizedActionName,
                userId, userRole, actionDetails, previousStatus, targetStatus, rejectionReason);

            return true;
        }

        /// <summary>
        /// Initializes state from current assessment status
        /// Should be called after loading from database
        /// </summary>
        public void InitializeState()
        {
            _currentState = AssessmentStateFactory.CreateState(_assessment.Status);
        }

        #endregion

        #region Assessment State Factory (Embedded)

        /// <summary>
        /// Factory methods for creating assessment state instances
        /// Embedded within context following ResolutionStateContext pattern
        /// </summary>
        public static class AssessmentStateFactory
        {
            /// <summary>
            /// Creates an assessment state instance based on the status enum
            /// </summary>
            /// <param name="status">The assessment status enum value</param>
            /// <returns>Appropriate IAssessmentState implementation</returns>
            /// <exception cref="ArgumentException">Thrown when status is not supported</exception>
            public static IAssessmentState CreateState(AssessmentStatus status)
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
            /// Gets the default initial state for a new assessment
            /// </summary>
            /// <param name="saveAsDraft">Whether the assessment is being saved as draft</param>
            /// <returns>Initial assessment state (Draft)</returns>
            public static IAssessmentState GetInitialState(bool saveAsDraft = true)
            {
                return new DraftState();
            }

            /// <summary>
            /// Validates if a transition from current status to target status is allowed
            /// </summary>
            /// <param name="currentStatus">Current assessment status</param>
            /// <param name="targetStatus">Target assessment status</param>
            /// <returns>True if transition is allowed</returns>
            public static bool CanTransitionTo(AssessmentStatus currentStatus, AssessmentStatus targetStatus)
            {
                var currentState = CreateState(currentStatus);
                return currentState.CanTransitionTo(targetStatus);
            }

            /// <summary>
            /// Gets all allowed transitions from a given status
            /// </summary>
            /// <param name="currentStatus">Current assessment status</param>
            /// <returns>Collection of allowed target statuses</returns>
            public static IEnumerable<AssessmentStatus> GetAllowedTransitions(AssessmentStatus currentStatus)
            {
                var currentState = CreateState(currentStatus);
                return currentState.GetAllowedTransitions();
            }

            /// <summary>
            /// Gets available actions for a given status
            /// </summary>
            /// <param name="status">Current status</param>
            /// <returns>List of available action enums</returns>
            public static List<AssessmentActionEnum> GetAvailableActions(AssessmentStatus status)
            {
                var currentState = CreateState(status);
                return currentState.GetAvailableActions();
            }

            /// <summary>
            /// Validates an assessment against its current state business rules
            /// </summary>
            /// <param name="assessment">The assessment to validate</param>
            /// <param name="localizer">String localizer for localized messages</param>
            /// <returns>Validation result with success status and messages</returns>
            public static (bool IsValid, List<string> ValidationMessages) ValidateAssessment(Assessment assessment, IStringLocalizer<SharedResources> localizer)
            {
                var currentState = CreateState(assessment.Status);
                return currentState.ValidateState(assessment, localizer);
            }

            /// <summary>
            /// Validates if editing is allowed for the current status
            /// </summary>
            /// <param name="currentStatus">Current assessment status</param>
            /// <returns>True if editing is allowed</returns>
            public static bool CanEdit(AssessmentStatus currentStatus)
            {
                return currentStatus switch
                {
                    AssessmentStatus.Draft => true,
                    AssessmentStatus.Rejected => true,
                    _ => false
                };
            }

            /// <summary>
            /// Validates if completion operations are allowed for the current status
            /// </summary>
            /// <param name="currentStatus">Current assessment status</param>
            /// <returns>True if completion is allowed</returns>
            public static bool CanComplete(AssessmentStatus currentStatus)
            {
                return currentStatus == AssessmentStatus.Active;
            }

            /// <summary>
            /// Validates if deletion is allowed for the current status
            /// </summary>
            /// <param name="currentStatus">Current assessment status</param>
            /// <returns>True if deletion is allowed</returns>
            public static bool CanDelete(AssessmentStatus currentStatus)
            {
                return currentStatus switch
                {
                    AssessmentStatus.Draft => true,
                    AssessmentStatus.Rejected => true,
                    _ => false
                };
            }

            /// <summary>
            /// Validates if approval operations are allowed for the current status
            /// </summary>
            /// <param name="currentStatus">Current assessment status</param>
            /// <returns>True if approval is allowed</returns>
            public static bool CanApprove(AssessmentStatus currentStatus)
            {
                return currentStatus == AssessmentStatus.WaitingForApproval;
            }

            /// <summary>
            /// Validates if rejection operations are allowed for the current status
            /// </summary>
            /// <param name="currentStatus">Current assessment status</param>
            /// <returns>True if rejection is allowed</returns>
            public static bool CanReject(AssessmentStatus currentStatus)
            {
                return currentStatus == AssessmentStatus.WaitingForApproval;
            }

            /// <summary>
            /// Validates if distribution operations are allowed for the current status
            /// </summary>
            /// <param name="currentStatus">Current assessment status</param>
            /// <returns>True if distribution is allowed</returns>
            public static bool CanDistribute(AssessmentStatus currentStatus)
            {
                return currentStatus == AssessmentStatus.Approved;
            }
        }

        #endregion

        #region Audit Trail Methods

        /// <summary>
        /// Adds basic status history entry
        /// </summary>
        private void AddStatusHistoryEntry(AssessmentStatus targetStatus, AssessmentActionEnum action, string reason)
        {
            // Initialize collection if null
            _assessment.StatusHistories ??= new List<AssessmentStatusHistory>();

            var statusHistory = new AssessmentStatusHistory
            {
                AssessmentId = _assessment.Id,
                AssessmentStatusId = (int)targetStatus,
                Action = action,
                Reason = reason,
                CreatedBy = _currentUserService.UserId,
                CreatedAt = DateTime.UtcNow
            };

            _assessment.StatusHistories.Add(statusHistory);
        }

        /// <summary>
        /// Adds comprehensive status history entry with full audit information
        /// </summary>
        private void AddStatusHistoryEntryWithAudit(AssessmentStatus targetStatus, AssessmentActionEnum action,
            string reason, string localizedActionName, int userId, string userRole, string actionDetails,
            AssessmentStatus previousStatus, AssessmentStatus newStatus, string rejectionReason)
        {
            // Initialize collection if null
            _assessment.StatusHistories ??= new List<AssessmentStatusHistory>();

            var statusHistory = new AssessmentStatusHistory
            {
                AssessmentId = _assessment.Id,
                AssessmentStatusId = (int)targetStatus,
                Action = action,
                Reason = reason,
                RejectionReason = rejectionReason,
                ActionDetails = actionDetails,
                Notes = localizedActionName, // Localization key reference
                UserRole = userRole,
                PreviousStatus = previousStatus,
                NewStatus = newStatus,
                CreatedBy = userId,
                CreatedAt = DateTime.UtcNow
            };

            _assessment.StatusHistories.Add(statusHistory);
        }

        #endregion

        #region Localization and User Context Methods

        /// <summary>
        /// Gets localized action name from AssessmentActionEnum
        /// </summary>
        private string GetLocalizedActionName(AssessmentActionEnum action)
        {
            return action switch
            {
                AssessmentActionEnum.Submission => _localizer[SharedResourcesKey.AssessmentSubmitForApproval],
                AssessmentActionEnum.Approval => _localizer[SharedResourcesKey.AssessmentApprove],
                AssessmentActionEnum.Rejection => _localizer[SharedResourcesKey.AssessmentReject],
                AssessmentActionEnum.Distribution => _localizer[SharedResourcesKey.AssessmentDistribute],
                AssessmentActionEnum.Completion => _localizer[SharedResourcesKey.AssessmentComplete],
                AssessmentActionEnum.Edit => _localizer[SharedResourcesKey.AssessmentEdit],
                AssessmentActionEnum.ViewDetails => _localizer[SharedResourcesKey.AssessmentViewDetails],
                AssessmentActionEnum.Delete => _localizer[SharedResourcesKey.AssessmentDelete],
                AssessmentActionEnum.Save => _localizer[SharedResourcesKey.AssessmentSave],
                AssessmentActionEnum.Respond => _localizer[SharedResourcesKey.AssessmentRespond],
                AssessmentActionEnum.ViewRejectionReason => _localizer[SharedResourcesKey.AssessmentViewRejectionReason],
                AssessmentActionEnum.Resubmit => _localizer[SharedResourcesKey.AssessmentResubmit],
                AssessmentActionEnum.ViewResponses => _localizer[SharedResourcesKey.AssessmentViewResponses],
                AssessmentActionEnum.ViewResults => _localizer[SharedResourcesKey.AssessmentViewResults],
                AssessmentActionEnum.CompleteAssessment => _localizer[SharedResourcesKey.AssessmentCompleteAssessment],
                AssessmentActionEnum.ExportResults => _localizer[SharedResourcesKey.AssessmentExportResults],
                AssessmentActionEnum.ExportData => _localizer[SharedResourcesKey.AssessmentExportData],
                AssessmentActionEnum.Archive => _localizer[SharedResourcesKey.AssessmentArchive],
                _ => action.ToString()
            };
        }

        /// <summary>
        /// Gets localized available actions for the current assessment state
        /// </summary>
        /// <returns>List of localized action names</returns>
        public List<string> GetLocalizedAvailableActions()
        {
            var actions = _currentState.GetAvailableActions();
            return actions.Select(action => GetLocalizedActionName(action)).ToList();
        }

        /// <summary>
        /// Gets the current user role for audit logging following Resolution module pattern
        /// Determines the current user's role within the specific fund context
        /// </summary>
        private async Task<Roles> GetCurrentUserRole()
        {
            try
            {
                var currentUserId = _currentUserService.UserId;
                if (!currentUserId.HasValue)
                {
                    return Roles.None;
                }

                // Get fund details with all related entities
                var fundDetails = await _repository.Funds.ViewFundUsers(_assessment.FundId, trackChanges: false);
                if (fundDetails == null)
                {
                    return Roles.None;
                }

                var userRole = Roles.None;

                // 1. Check if user is Legal Council for the fund
                if (fundDetails.LegalCouncilId == currentUserId.Value)
                {
                    userRole = Roles.LegalCouncil;
                }

                // 2. Check if user is a Fund Manager for the fund
                if (fundDetails.FundManagers != null && fundDetails.FundManagers.Count > 0)
                {
                    var isFundManager = fundDetails.FundManagers.Any(fm => fm.UserId == currentUserId.Value);
                    if (isFundManager)
                    {
                        userRole = Roles.FundManager;
                    }
                }

                // 3. Check if user is a Board Secretary for the fund
                if (fundDetails.FundBoardSecretaries != null && fundDetails.FundBoardSecretaries.Count > 0)
                {
                    var isBoardSecretary = fundDetails.FundBoardSecretaries.Any(bs => bs.UserId == currentUserId.Value);
                    if (isBoardSecretary)
                    {
                        userRole = Roles.BoardSecretary;
                    }
                }

                // 4. Check if user is a Board Member for the fund
                if (fundDetails.BoardMembers != null && fundDetails.BoardMembers.Count > 0)
                {
                    var isBoardMember = fundDetails.BoardMembers.Any(bm => bm.UserId == currentUserId.Value);
                    if (isBoardMember)
                    {
                        userRole = Roles.BoardMember;
                    }
                }

                return userRole;
            }
            catch (Exception)
            {
                // Log error but don't throw to avoid breaking the main operation
                return Roles.None;
            }
        }

        #endregion
    }
}
