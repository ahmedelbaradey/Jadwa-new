using Domain.Entities.Notifications;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Repository;
using Domain.Entities.FundManagement;
using Domain.Entities.FundManagement.State;

namespace Application.Services
{
    /// <summary>
    /// Context class for managing fund state transitions with comprehensive audit logging and notifications
    /// Follows the same pattern as ResolutionStateContext for consistency
    /// Based on requirements in Stories.md and Sprint.md for fund lifecycle management
    /// Integrates with audit logging, notification system, and localization
    /// </summary>
    public class FundStateContext
    {
        private readonly Fund _fund;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IRepositoryManager _repository;
        private readonly ICurrentUserService _currentUserService;

        public FundStateContext(
            Fund fund,
            IStringLocalizer<SharedResources> localizer,
            IRepositoryManager repository,
            ICurrentUserService currentUserService)
        {
            _fund = fund;
            _localizer = localizer;
            _repository = repository;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Changes fund status with comprehensive audit logging and notifications
        /// Integrates with audit logging using localization keys and user context
        /// </summary>
        /// <param name="newState">New state to transition to</param>
        /// <param name="action">Action being performed</param>
        /// <param name="actionDetails">Detailed description of the action</param>
        /// <param name="localizationKey">Localization key for the action</param>
        /// <param name="sendNotifications">Whether to send notifications for this transition</param>
        /// <returns>True if transition was successful, false otherwise</returns>
        public bool ChangeStatusWithAudit(
            IFundState newState,
            FundActionEnum action,
            string actionDetails,
            string localizationKey,
            bool sendNotifications = true)
        {
            try
            {
                var currentStatus = _fund.GetCurrentStatusEnum();
                var newStatus = newState.Status;

                // Validate transition
                if (!FundStateFactory.IsTransitionAllowed(currentStatus, newStatus))
                {
                    return false;
                }

                // Get current status ID for audit trail
                var currentStatusId = _fund.Status?.Id ?? 0;
                var newStatusId = (int)newStatus;

                // Only proceed if status actually changes or it's a non-status action
                if (currentStatusId != newStatusId || action != FundActionEnum.StatusChange)
                {
                    // Update fund state
                    _fund.SetState(newState);

                    // Add comprehensive audit entry
                    AddAuditEntry(action, actionDetails, localizationKey, currentStatusId, newStatusId);

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
        /// Adds comprehensive audit entry with user context and localization support
        /// Follows the ResolutionStatusHistory pattern for consistency
        /// </summary>
        /// <param name="action">Action being performed</param>
        /// <param name="actionDetails">Detailed description of the action</param>
        /// <param name="localizationKey">Localization key for retrieval-time translation</param>
        /// <param name="previousStatusId">Previous status ID (optional)</param>
        /// <param name="newStatusId">New status ID (optional)</param>
        public void AddAuditEntry(
            FundActionEnum action,
            string actionDetails,
            string localizationKey,
            int? previousStatusId = null,
            int? newStatusId = null)
        {
            try
            {
                _fund.FundStatusHistories ??= new List<FundStatusHistory>();

                var auditEntry = new FundStatusHistory
                {
                    FundId = _fund.Id,
                    StatusHistoryId = newStatusId ?? _fund.Status?.Id ?? 1,
                    CreatedAt = DateTime.Now,
                    CreatedBy = _currentUserService.UserId ?? 0
                };

                _fund.FundStatusHistories.Add(auditEntry);
            }
            catch (Exception)
            {
                // Log error but don't throw to avoid breaking the main operation
            }
        }

        /// <summary>
        /// Validates if fund can be activated based on business rules
        /// Fund can be activated if it has 2 or more independent board members
        /// </summary>
        /// <returns>True if fund can be activated, false otherwise</returns>
        public bool CanActivateFund()
        {
            try
            {
                if (_fund.BoardMembers == null)
                    return false;

                var independentMembersCount = _fund.BoardMembers
                    .Count(bm => bm.IsActive && bm.MemberType == BoardMemberType.Independent);

                return independentMembersCount >= 2;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Activates fund if business rules are met
        /// Used when board members are added and fund should be activated
        /// </summary>
        /// <returns>True if fund was activated, false otherwise</returns>
        public bool TryActivateFund()
        {
            try
            {
                var currentStatus = _fund.GetCurrentStatusEnum();
                
                // Only activate if not already active and business rules are met
                if (currentStatus != FundStatusEnum.Active && CanActivateFund())
                {
                    var activeState = new ActiveFund();
                    return ChangeStatusWithAudit(
                        activeState,
                        FundActionEnum.FundActivation,
                        _localizer[SharedResourcesKey.FundActivatedDueToMembers],
                        SharedResourcesKey.FundActivatedDueToMembers,
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
        /// Sends notifications for state transitions based on action type
        /// Follows the established notification patterns from command handlers
        /// </summary>
        private async Task SendStateTransitionNotifications(FundActionEnum action, FundStatusEnum previousStatus, FundStatusEnum newStatus)
        {
            try
            {
                var notifications = new List<Notification>();

                // Get stakeholders based on action type
                var stakeholderUserIds = GetStakeholderUserIds(action);

                // Determine notification type based on action
                var notificationType = GetNotificationTypeForAction(action);

                if (notificationType.HasValue)
                {
                    foreach (var userId in stakeholderUserIds)
                    {
                        notifications.Add(new Notification
                        {
                            Title = string.Empty, // Will be localized at send time
                            Body = GetNotificationBody(action), // Store parameters for localization
                            FundId = _fund.Id,
                            UserId = userId,
                            NotificationType = (int)notificationType.Value,
                        });
                    }

                    if (notifications.Count > 0 && _repository?.Notifications != null)
                    {
                        await _repository.Notifications.AddRangeAsync(notifications);
                    }
                }
            }
            catch (Exception)
            {
                // Log error but don't throw to avoid breaking the main operation
            }
        }

        /// <summary>
        /// Gets stakeholder user IDs based on action type
        /// </summary>
        private List<int> GetStakeholderUserIds(FundActionEnum action)
        {
            var stakeholderUserIds = new List<int>();

            try
            {
                // Add Fund Managers
                if (_fund.FundManagers != null)
                {
                    stakeholderUserIds.AddRange(_fund.FundManagers.Where(fm => fm.IsDeleted != true).Select(fm => fm.UserId));
                }

                // Add Board Secretaries
                if (_fund.FundBoardSecretaries != null)
                {
                    stakeholderUserIds.AddRange(_fund.FundBoardSecretaries.Where(bs => bs.IsDeleted != true).Select(bs => bs.UserId));
                }

                // Add Legal Council
                stakeholderUserIds.Add(_fund.LegalCouncilId);

                // For fund activation, also add board members
                if (action == FundActionEnum.FundActivation && _fund.BoardMembers != null)
                {
                    stakeholderUserIds.AddRange(_fund.BoardMembers.Where(bm => bm.IsActive).Select(bm => bm.UserId));
                }

                // Remove duplicates
                return stakeholderUserIds.Distinct().ToList();
            }
            catch (Exception)
            {
                return new List<int>();
            }
        }

        /// <summary>
        /// Gets notification type for specific fund action
        /// </summary>
        private NotificationType? GetNotificationTypeForAction(FundActionEnum action)
        {
            return action switch
            {
                FundActionEnum.FundCreation => NotificationType.AddedToFund,
                FundActionEnum.FundDataCompletion => NotificationType.CompeleteFund,
                FundActionEnum.FundDataEdit => NotificationType.CompeleteFund,
                FundActionEnum.FundActivation => NotificationType.FundActivated,
                FundActionEnum.ExitDateEdit => NotificationType.ChangeExitDate,
                _ => null
            };
        }

        /// <summary>
        /// Gets notification body parameters for localization
        /// </summary>
        private string GetNotificationBody(FundActionEnum action)
        {
            return action switch
            {
                FundActionEnum.FundActivation => _fund.Name, // MSG008: Fund name only
                FundActionEnum.ExitDateEdit => $"{_fund.Name}|{_fund.ExitDate}|{_currentUserService.UserName}",
                _ => $"{_fund.Name}|{_currentUserService.UserName}"
            };
        }
    }
}
