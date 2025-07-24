using Domain.Entities.AssessmentManagement;
using Domain.States.AssessmentStates;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository;

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
        public bool ChangeStatusWithAudit(
            IAssessmentState newState,
            Domain.Entities.AssessmentManagement.AssessmentActionEnum action,
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
                    AddAuditEntry(action, actionDetails, localizationKey, currentStatus, newStatus);

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
        public bool TryApproveAssessment(string reviewerComments = null)
        {
            try
            {
                if (_assessment.Status == AssessmentStatus.WaitingForApproval)
                {
                    var approvedState = new ApprovedState();
                    _assessment.ReviewerComments = reviewerComments;
                    _assessment.ReviewedBy = _currentUserService.UserId;
                    _assessment.ReviewedDate = DateTime.UtcNow;

                    return ChangeStatusWithAudit(
                        approvedState,
                        Domain.Entities.AssessmentManagement.AssessmentActionEnum.Approval,
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
        public bool TryRejectAssessment(string rejectionReason)
        {
            try
            {
                if (_assessment.Status == AssessmentStatus.WaitingForApproval && !string.IsNullOrWhiteSpace(rejectionReason))
                {
                    var rejectedState = new RejectedState();
                    _assessment.ReviewerComments = rejectionReason;
                    _assessment.ReviewedBy = _currentUserService.UserId;
                    _assessment.ReviewedDate = DateTime.UtcNow;

                    return ChangeStatusWithAudit(
                        rejectedState,
                        Domain.Entities.AssessmentManagement.AssessmentActionEnum.Rejection,
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
        public bool TryDistributeAssessment()
        {
            try
            {
                if (_assessment.Status == AssessmentStatus.Approved)
                {
                    var activeState = new ActiveState();
                    return ChangeStatusWithAudit(
                        activeState,
                        Domain.Entities.AssessmentManagement.AssessmentActionEnum.Distribution,
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
        private void AddAuditEntry(Domain.Entities.AssessmentManagement.AssessmentActionEnum action, string actionDetails, string localizationKey,
            AssessmentStatus oldStatus, AssessmentStatus newStatus)
        {
            try
            {
                // Initialize collection if null
                _assessment.StatusHistories ??= new List<AssessmentStatusHistory>();

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
                    UserRole = GetCurrentUserRole(),
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
        /// Gets the current user role for audit logging
        /// </summary>
        private string GetCurrentUserRole()
        {
            // This would typically get the user's role from the current user service
            // For now, return a default value
            return _currentUserService.UserRole ?? "User";
        }

        /// <summary>
        /// Sends notifications for state transitions following Resolution module patterns
        /// </summary>
        private async Task SendStateTransitionNotifications(Domain.Entities.AssessmentManagement.AssessmentActionEnum action,
            AssessmentStatus oldStatus, AssessmentStatus newStatus)
        {
            try
            {
                var fund = await _repository.Funds.GetByIdAsync(_assessment.FundId);
                if (fund == null) return;

                var notifications = new List<Domain.Entities.Notifications.Notification>();

                switch (action)
                {
                    case Domain.Entities.AssessmentManagement.AssessmentActionEnum.Submission:
                        await AddSubmissionNotifications(notifications, fund);
                        break;
                    case Domain.Entities.AssessmentManagement.AssessmentActionEnum.Approval:
                        await AddApprovalNotifications(notifications, fund);
                        break;
                    case Domain.Entities.AssessmentManagement.AssessmentActionEnum.Rejection:
                        await AddRejectionNotifications(notifications, fund);
                        break;
                    case Domain.Entities.AssessmentManagement.AssessmentActionEnum.Distribution:
                        await AddDistributionNotifications(notifications, fund);
                        break;
                    case Domain.Entities.AssessmentManagement.AssessmentActionEnum.Completion:
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
        /// Gets localized action name
        /// </summary>
        private string GetLocalizedActionName(string action)
        {
            return action switch
            {
                "Submit for Approval" => _localizer[SharedResourcesKey.AssessmentSubmitForApproval],
                "Approve" => _localizer[SharedResourcesKey.AssessmentApprove],
                "Reject" => _localizer[SharedResourcesKey.AssessmentReject],
                "Distribute" => _localizer[SharedResourcesKey.AssessmentDistribute],
                "Complete" => _localizer[SharedResourcesKey.AssessmentComplete],
                "Edit" => _localizer[SharedResourcesKey.AssessmentEdit],
                "View Details" => _localizer[SharedResourcesKey.AssessmentViewDetails],
                "Delete" => _localizer[SharedResourcesKey.AssessmentDelete],
                "Save" => _localizer[SharedResourcesKey.AssessmentSave],
                "Respond" => _localizer[SharedResourcesKey.AssessmentRespond],
                "View Rejection Reason" => _localizer[SharedResourcesKey.AssessmentViewRejectionReason],
                "Resubmit" => _localizer[SharedResourcesKey.AssessmentResubmit],
                "View Responses" => _localizer[SharedResourcesKey.AssessmentViewResponses],
                "View Results" => _localizer[SharedResourcesKey.AssessmentViewResults],
                "Complete Assessment" => _localizer[SharedResourcesKey.AssessmentCompleteAssessment],
                "Export Results" => _localizer[SharedResourcesKey.AssessmentExportResults],
                "Export Data" => _localizer[SharedResourcesKey.AssessmentExportData],
                "Archive" => _localizer[SharedResourcesKey.AssessmentArchive],
                _ => action
            };
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
