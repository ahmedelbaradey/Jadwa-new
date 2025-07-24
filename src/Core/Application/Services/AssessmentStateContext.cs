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
    /// Context class for managing assessment state transitions with comprehensive audit logging and notifications
    /// Follows the same pattern as FundStateContext and ResolutionStateContext for consistency
    /// Based on requirements for assessment lifecycle management
    /// Integrates with audit logging, notification system, and localization
    /// </summary>
    public class AssessmentStateContext
    {
        private readonly Assessment _assessment;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IRepositoryManager _repository;
        private readonly ICurrentUserService _currentUserService;

        public AssessmentStateContext(
            Assessment assessment,
            IStringLocalizer<SharedResources> localizer,
            IRepositoryManager repository,
            ICurrentUserService currentUserService)
        {
            _assessment = assessment;
            _localizer = localizer;
            _repository = repository;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Changes assessment status with comprehensive audit logging and notifications
        /// Integrates with audit logging using localization keys and user context
        /// </summary>
        /// <param name="newState">New state to transition to</param>
        /// <param name="action">Action being performed</param>
        /// <param name="actionDetails">Detailed description of the action</param>
        /// <param name="localizationKey">Localization key for the action</param>
        /// <param name="sendNotifications">Whether to send notifications for this transition</param>
        /// <returns>True if transition was successful, false otherwise</returns>
        public async Task<bool> ChangeStatusWithAudit(
            IAssessmentState newState,
            AssessmentActionEnum action,
            string actionDetails,
            string localizationKey,
            bool sendNotifications = true)
        {
            try
            {
                var currentStatus = _assessment.Status;
                var newStatus = newState.Status;

                // Validate transition is allowed
                if (!_assessment.CurrentState.CanTransitionTo(newStatus))
                {
                    return false;
                }

                // Only proceed if status actually changes
                if (currentStatus != newStatus)
                {
                    // Update assessment state
                    _assessment.SetState(newState);
                    _assessment.Status = newStatus;

                    // Add comprehensive audit entry
                    await AddAuditEntry(action, actionDetails, localizationKey, currentStatus, newStatus);

                    // Send notifications if requested
                    if (sendNotifications)
                    {
                        _ = Task.Run(() => SendStateTransitionNotifications(action, currentStatus, newStatus));
                    }

                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                // Log error and return false
                return false;
            }
        }

        /// <summary>
        /// Validates current state with localized messages
        /// </summary>
        /// <returns>Validation result with localized messages</returns>
        public (bool IsValid, List<string> ValidationMessages) ValidateCurrentState()
        {
            return _assessment.CurrentState.ValidateState(_assessment, _localizer);
        }

        /// <summary>
        /// Gets localized available actions for current state
        /// </summary>
        /// <returns>List of localized action names</returns>
        public List<string> GetLocalizedAvailableActions()
        {
            var actions = _assessment.CurrentState.GetAvailableActions();
            return actions.Select(action => GetLocalizedActionName(action)).ToList();
        }

        /// <summary>
        /// Gets localized status display name
        /// </summary>
        /// <returns>Localized status name</returns>
        public string GetLocalizedStatusName()
        {
            return _assessment.Status switch
            {
                AssessmentStatus.Draft => _localizer[SharedResourcesKey.AssessmentStatusDraft],
                AssessmentStatus.WaitingForApproval => _localizer[SharedResourcesKey.AssessmentStatusWaitingForApproval],
                AssessmentStatus.Approved => _localizer[SharedResourcesKey.AssessmentStatusApproved],
                AssessmentStatus.Rejected => _localizer[SharedResourcesKey.AssessmentStatusRejected],
                AssessmentStatus.Active => _localizer[SharedResourcesKey.AssessmentStatusActive],
                AssessmentStatus.Completed => _localizer[SharedResourcesKey.AssessmentStatusCompleted],
                _ => _assessment.Status.ToString()
            };
        }

        /// <summary>
        /// Approves assessment if business rules are met
        /// </summary>
        /// <param name="reviewerComments">Optional reviewer comments</param>
        /// <returns>True if assessment was approved, false otherwise</returns>
        public async Task<bool> TryApproveAssessment(string reviewerComments = null)
        {
            try
            {
                if (_assessment.Status == AssessmentStatus.WaitingForApproval)
                {
                    var approvedState = new ApprovedState();
                    _assessment.ReviewerComments = reviewerComments;
                    _assessment.ReviewedBy = _currentUserService.UserId;
                    _assessment.ReviewedDate = DateTime.UtcNow;

                    return await ChangeStatusWithAudit(
                        approvedState,
                        AssessmentActionEnum.Approval,
                        _localizer[SharedResourcesKey.AssessmentApproved],
                        SharedResourcesKey.AssessmentApproved,
                        sendNotifications: true
                    );
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Rejects assessment with reason
        /// </summary>
        /// <param name="rejectionReason">Reason for rejection</param>
        /// <returns>True if assessment was rejected, false otherwise</returns>
        public async Task<bool> TryRejectAssessment(string rejectionReason)
        {
            try
            {
                if (_assessment.Status == AssessmentStatus.WaitingForApproval && !string.IsNullOrWhiteSpace(rejectionReason))
                {
                    var rejectedState = new RejectedState();
                    _assessment.ReviewerComments = rejectionReason;
                    _assessment.ReviewedBy = _currentUserService.UserId;
                    _assessment.ReviewedDate = DateTime.UtcNow;

                    return await ChangeStatusWithAudit(
                        rejectedState,
                        AssessmentActionEnum.Rejection,
                        _localizer[SharedResourcesKey.AssessmentRejected],
                        SharedResourcesKey.AssessmentRejected,
                        sendNotifications: true
                    );
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Distributes assessment to board members
        /// </summary>
        /// <returns>True if assessment was distributed, false otherwise</returns>
        public async Task<bool> TryDistributeAssessment()
        {
            try
            {
                if (_assessment.Status == AssessmentStatus.Approved)
                {
                    var activeState = new ActiveState();
                    return await ChangeStatusWithAudit(
                        activeState,
                        AssessmentActionEnum.Distribution,
                        _localizer[SharedResourcesKey.AssessmentDistributed],
                        SharedResourcesKey.AssessmentDistributed,
                        sendNotifications: true
                    );
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region Private Methods

        /// <summary>
        /// Adds comprehensive audit entry following ResolutionStateContext pattern
        /// </summary>
        private async Task AddAuditEntry(AssessmentActionEnum action, string actionDetails, string localizationKey,
            AssessmentStatus oldStatus, AssessmentStatus newStatus)
        {
            try
            {
                // Initialize collection if null
                _assessment.StatusHistories ??= new List<AssessmentStatusHistory>();

                // Get user role following Resolution pattern
                var userRole = await GetCurrentUserRole();

                // Create comprehensive status history entry with all required audit fields
                var statusHistory = new AssessmentStatusHistory
                {
                    // Core assessment reference
                    AssessmentId = _assessment.Id,
                    AssessmentStatusId = (int)newStatus,

                    // Action information
                    Action = action,
                    Reason = actionDetails,

                    // Comprehensive action details - detailed description of the operation
                    ActionDetails = actionDetails,

                    // Localization key reference (NOT translated text) following notification pattern
                    // This allows proper localization on retrieval for multilingual support
                    Notes = localizationKey,

                    // User context information
                    UserRole = userRole.ToString(),
                    CreatedBy = _currentUserService.UserId,

                    // Status transition information (for status changes)
                    PreviousStatus = oldStatus,
                    NewStatus = newStatus,

                    // Timestamp (automatically set by CreationAuditedEntity)
                    CreatedAt = DateTime.UtcNow
                };

                _assessment.StatusHistories.Add(statusHistory);
            }
            catch (Exception)
            {
                // Log error but don't throw to avoid breaking the main operation
            }
        }

        /// <summary>
        /// Gets the current user role for audit logging following Resolution module pattern
        /// Determines the current user's role within the specific fund context
        /// Checks FundBoardSecretary table, Fund.LegalCouncilId field, and FundManager table
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

        /// <summary>
        /// Sends notifications for state transitions following Resolution module patterns
        /// </summary>
        private async Task SendStateTransitionNotifications(AssessmentActionEnum action,
            AssessmentStatus oldStatus, AssessmentStatus newStatus)
        {
            try
            {
                var fund = await _repository.Funds.GetByIdAsync(_assessment.FundId);
                if (fund == null) return;

                var notifications = new List<Domain.Entities.Notifications.Notification>();

                switch (action)
                {
                    case AssessmentActionEnum.Submission:
                        await AddSubmissionNotifications(notifications, fund);
                        break;
                    case AssessmentActionEnum.Approval:
                        await AddApprovalNotifications(notifications, fund);
                        break;
                    case AssessmentActionEnum.Rejection:
                        await AddRejectionNotifications(notifications, fund);
                        break;
                    case AssessmentActionEnum.Distribution:
                        await AddDistributionNotifications(notifications, fund);
                        break;
                    case AssessmentActionEnum.Completion:
                        await AddCompletionNotifications(notifications, fund);
                        break;
                }

                if (notifications.Any())
                {
                    await _repository.Notifications.AddRangeAsync(notifications);
                }
            }
            catch (Exception)
            {
                // Log error but don't fail the main operation
            }
        }

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
            var actions = _assessment.CurrentState.GetAvailableActions();
            return actions.Select(action => GetLocalizedActionName(action)).ToList();
        }

        /// <summary>
        /// Adds notifications for assessment submission (MSG002)
        /// Notifies Legal Council and Board Secretaries
        /// </summary>
        private async Task AddSubmissionNotifications(List<Domain.Entities.Notifications.Notification> notifications, Domain.Entities.FundManagement.Fund fund)
        {
            // Notify Legal Council
            if (fund.LegalCouncilId > 0)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    UserId = fund.LegalCouncilId,
                    FundId = fund.Id,
                    Title = string.Empty,
                    Body = $"{_assessment.Title}|{fund.NameEn}|{_currentUserService.UserName}",
                    NotificationType = (int)Domain.Entities.Notifications.NotificationType.AssessmentSubmittedForApproval,
                    NotificationModule = (int)Domain.Entities.Notifications.NotificationModule.Assessments,
                    IsRead = false
                });
            }

            // Notify Board Secretaries
            var boardSecretaries = fund.FundBoardSecretaries ?? new List<Domain.Entities.FundManagement.FundBoardSecretary>();
            foreach (var boardSecretary in boardSecretaries)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    UserId = boardSecretary.UserId,
                    FundId = fund.Id,
                    Title = string.Empty,
                    Body = $"{_assessment.Title}|{fund.NameEn}|{_currentUserService.UserName}",
                    NotificationType = (int)Domain.Entities.Notifications.NotificationType.AssessmentSubmittedForApproval,
                    NotificationModule = (int)Domain.Entities.Notifications.NotificationModule.Assessments,
                    IsRead = false
                });
            }
        }

        /// <summary>
        /// Adds notifications for assessment approval (MSG002)
        /// Notifies assessment creator
        /// </summary>
        private async Task AddApprovalNotifications(List<Domain.Entities.Notifications.Notification> notifications, Domain.Entities.FundManagement.Fund fund)
        {
            if (_assessment.CreatedBy.HasValue)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    UserId = _assessment.CreatedBy.Value,
                    FundId = fund.Id,
                    Title = string.Empty,
                    Body = $"{_assessment.Title}|{fund.NameEn}|{_currentUserService.UserName}",
                    NotificationType = (int)Domain.Entities.Notifications.NotificationType.AssessmentApproved,
                    NotificationModule = (int)Domain.Entities.Notifications.NotificationModule.Assessments,
                    IsRead = false
                });
            }
        }

        /// <summary>
        /// Adds notifications for assessment rejection (MSG004)
        /// Notifies assessment creator
        /// </summary>
        private async Task AddRejectionNotifications(List<Domain.Entities.Notifications.Notification> notifications, Domain.Entities.FundManagement.Fund fund)
        {
            if (_assessment.CreatedBy.HasValue)
            {
                var rejectionReason = _assessment.ReviewerComments ?? "No reason provided";
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    UserId = _assessment.CreatedBy.Value,
                    FundId = fund.Id,
                    Title = string.Empty,
                    Body = $"{_assessment.Title}|{fund.NameEn}|{_currentUserService.UserName}|{rejectionReason}",
                    NotificationType = (int)Domain.Entities.Notifications.NotificationType.AssessmentRejected,
                    NotificationModule = (int)Domain.Entities.Notifications.NotificationModule.Assessments,
                    IsRead = false
                });
            }
        }

        /// <summary>
        /// Adds notifications for assessment distribution (MSG002)
        /// Notifies all board members
        /// </summary>
        private async Task AddDistributionNotifications(List<Domain.Entities.Notifications.Notification> notifications, Domain.Entities.FundManagement.Fund fund)
        {
            // Notify all board members
            var boardMembers = fund.FundBoardMembers ?? new List<Domain.Entities.FundManagement.FundBoardMember>();
            foreach (var boardMember in boardMembers)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    UserId = boardMember.UserId,
                    FundId = fund.Id,
                    Title = string.Empty,
                    Body = $"{_assessment.Title}|{fund.NameEn}|{_currentUserService.UserName}",
                    NotificationType = (int)Domain.Entities.Notifications.NotificationType.AssessmentDistributed,
                    NotificationModule = (int)Domain.Entities.Notifications.NotificationModule.Assessments,
                    IsRead = false
                });
            }
        }

        /// <summary>
        /// Adds notifications for assessment completion (MSG002)
        /// Notifies assessment creator and fund stakeholders
        /// </summary>
        private async Task AddCompletionNotifications(List<Domain.Entities.Notifications.Notification> notifications, Domain.Entities.FundManagement.Fund fund)
        {
            var recipients = new List<int>();

            // Add assessment creator
            if (_assessment.CreatedBy.HasValue)
            {
                recipients.Add(_assessment.CreatedBy.Value);
            }

            // Add fund managers
            var fundManagers = fund.FundManagers ?? new List<Domain.Entities.FundManagement.FundManager>();
            recipients.AddRange(fundManagers.Select(fm => fm.UserId));

            // Add legal council
            if (fund.LegalCouncilId > 0)
            {
                recipients.Add(fund.LegalCouncilId);
            }

            // Add board secretaries
            var boardSecretaries = fund.FundBoardSecretaries ?? new List<Domain.Entities.FundManagement.FundBoardSecretary>();
            recipients.AddRange(boardSecretaries.Select(bs => bs.UserId));

            // Remove duplicates
            recipients = recipients.Distinct().ToList();

            foreach (var userId in recipients)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    UserId = userId,
                    FundId = fund.Id,
                    Title = string.Empty,
                    Body = $"{_assessment.Title}|{fund.NameEn}|{_currentUserService.UserName}",
                    NotificationType = (int)Domain.Entities.Notifications.NotificationType.AssessmentCompleted,
                    NotificationModule = (int)Domain.Entities.Notifications.NotificationModule.Assessments,
                    IsRead = false
                });
            }
        }

        #endregion
    }
}
