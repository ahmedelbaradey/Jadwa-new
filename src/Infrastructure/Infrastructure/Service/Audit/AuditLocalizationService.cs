using Abstraction.Contracts.Logger;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.Users;
using Domain.Services.Audit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Resources;
using System.Globalization;

namespace Infrastructure.Service.Audit
{
    /// <summary>
    /// Service for localizing audit history entries
    /// Follows the established notification pattern for multilingual support
    /// Provides localized descriptions based on stored localization keys in Notes field
    /// </summary>
    public class AuditLocalizationService : IAuditLocalizationService
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;

        public AuditLocalizationService(
            IStringLocalizer<SharedResources> localizer,
            UserManager<User> userManager,
            ILoggerManager logger)
        {
            _localizer = localizer;
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets localized action description for a resolution status history entry
        /// Uses the localization key stored in the Notes field to retrieve translated text
        /// </summary>
        public string GetLocalizedActionDescription(ResolutionStatusHistory historyEntry, string? culture = null)
        {
            try
            {
                if (string.IsNullOrEmpty(historyEntry.Notes))
                {
                    return GetFallbackActionDescription(historyEntry);
                }

                var originalCulture = CultureInfo.CurrentCulture;
                var originalUICulture = CultureInfo.CurrentUICulture;

                try
                {
                    // Set culture if provided
                    if (!string.IsNullOrEmpty(culture))
                    {
                        var cultureInfo = new CultureInfo(culture);
                        CultureInfo.CurrentCulture = cultureInfo;
                        CultureInfo.CurrentUICulture = cultureInfo;
                    }

                    // Get localized action name from stored key
                    var localizedActionName = _localizer[historyEntry.Notes].Value;
                    
                    // If localization key not found, use the key itself as fallback
                    if (localizedActionName == historyEntry.Notes)
                    {
                        return GetFallbackActionDescription(historyEntry);
                    }

                    return localizedActionName;
                }
                finally
                {
                    // Restore original culture
                    CultureInfo.CurrentCulture = originalCulture;
                    CultureInfo.CurrentUICulture = originalUICulture;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Error getting localized action description for history entry {HistoryId}", historyEntry.Id));
                return GetFallbackActionDescription(historyEntry);
            }
        }

        /// <summary>
        /// Gets localized action description with user-specific language preferences
        /// </summary>
        public async Task<string> GetLocalizedActionDescriptionAsync(ResolutionStatusHistory historyEntry, int? userId = null)
        {
            try
            {
                string culture = "ar-EG"; // Default to Arabic

                if (userId.HasValue)
                {
                    culture = await GetUserPreferredLanguageAsync(userId.Value);
                }

                return GetLocalizedActionDescription(historyEntry, culture);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Error getting localized action description for user {UserId}", userId));
                return GetLocalizedActionDescription(historyEntry, "ar-EG");
            }
        }

        /// <summary>
        /// Gets comprehensive localized description including action details and status transitions
        /// </summary>
        public string GetLocalizedComprehensiveDescription(ResolutionStatusHistory historyEntry, string? culture = null)
        {
            try
            {
                var localizedAction = GetLocalizedActionDescription(historyEntry, culture);
                var statusTransition = GetLocalizedStatusTransition(historyEntry.PreviousStatus, historyEntry.NewStatus, culture);
                
                var description = localizedAction;
                
                if (!string.IsNullOrEmpty(statusTransition))
                {
                    description += $" ({statusTransition})";
                }

                // Add user role and timestamp information
                if (!string.IsNullOrEmpty(historyEntry.UserRole))
                {
                    var byText = culture?.StartsWith("ar") == true ? "بواسطة" : "by";
                    description += $" {byText} {historyEntry.UserRole}";
                }

                return description;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Error getting comprehensive localized description for history entry {HistoryId}", historyEntry.Id));
                return GetFallbackActionDescription(historyEntry);
            }
        }

        /// <summary>
        /// Gets localized status transition description
        /// </summary>
        public string GetLocalizedStatusTransition(ResolutionStatusEnum? previousStatus, ResolutionStatusEnum? newStatus, string? culture = null)
        {
            try
            {
                if (!previousStatus.HasValue || !newStatus.HasValue || previousStatus == newStatus)
                {
                    return string.Empty;
                }

                var originalCulture = CultureInfo.CurrentCulture;
                var originalUICulture = CultureInfo.CurrentUICulture;

                try
                {
                    // Set culture if provided
                    if (!string.IsNullOrEmpty(culture))
                    {
                        var cultureInfo = new CultureInfo(culture);
                        CultureInfo.CurrentCulture = cultureInfo;
                        CultureInfo.CurrentUICulture = cultureInfo;
                    }

                    var statusText = culture?.StartsWith("ar") == true ? "الحالة" : "Status";
                    var fromText = culture?.StartsWith("ar") == true ? "من" : "from";
                    var toText = culture?.StartsWith("ar") == true ? "إلى" : "to";

                    // Get localized status names
                    var previousStatusText = GetLocalizedStatusName(previousStatus.Value, culture);
                    var newStatusText = GetLocalizedStatusName(newStatus.Value, culture);

                    return $"{statusText}: {previousStatusText} {toText} {newStatusText}";
                }
                finally
                {
                    // Restore original culture
                    CultureInfo.CurrentCulture = originalCulture;
                    CultureInfo.CurrentUICulture = originalUICulture;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting localized status transition");
                return $"Status: {previousStatus} → {newStatus}";
            }
        }

        /// <summary>
        /// Gets user's preferred language from user profile
        /// </summary>
        private async Task<string> GetUserPreferredLanguageAsync(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                
                // Check if user has language preference set
                if (user != null && !string.IsNullOrEmpty(user.PreferredLanguage))
                {
                    return user.PreferredLanguage;
                }

                // Default to Arabic if no preference set
                return "ar-EG";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Error getting user preferred language for user {UserId}", userId));
                return "ar-EG";
            }
        }

        /// <summary>
        /// Gets localized status name using resource keys
        /// </summary>
        public string GetLocalizedStatusName(ResolutionStatusEnum status, string? culture = null)
        {
            try
            {
                // Map status enum to resource key
                var statusKey = status switch
                {
                    ResolutionStatusEnum.Draft => _localizer[SharedResourcesKey.ResolutionStatusDraft] ,
                    ResolutionStatusEnum.Pending => _localizer[SharedResourcesKey.ResolutionStatusPending]  ,
                    ResolutionStatusEnum.CompletingData => _localizer[SharedResourcesKey.ResolutionStatusCompletingData] ,
                    ResolutionStatusEnum.WaitingForConfirmation => _localizer[SharedResourcesKey.ResolutionStatusWaitingForConfirmation],
                    ResolutionStatusEnum.Confirmed => _localizer[SharedResourcesKey.ResolutionStatusConfirmed] ,
                    ResolutionStatusEnum.VotingInProgress => _localizer[SharedResourcesKey.ResolutionStatusVotingInProgress],
                    ResolutionStatusEnum.Approved => _localizer[SharedResourcesKey.ResolutionStatusApproved],
                    ResolutionStatusEnum.NotApproved => _localizer[SharedResourcesKey.ResolutionStatusNotApproved] ,
                    ResolutionStatusEnum.Rejected => _localizer[SharedResourcesKey.ResolutionStatusRejected]  ,
                    ResolutionStatusEnum.Cancelled => _localizer[SharedResourcesKey.ResolutionStatusCancelled] ,
                    _ => status.ToString()
                };

                var localizedStatus = _localizer[statusKey].Value;
                
                // If localization not found, return enum name
                return localizedStatus == statusKey ? localizedStatus : status.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Error getting localized status name for {Status}", status));
                return status.ToString();
            }
        }

        

        /// <summary>
        /// Gets fallback action description when localization fails
        /// </summary>
        private static string GetFallbackActionDescription(ResolutionStatusHistory historyEntry)
        {
            return historyEntry.Action switch
            {
                ResolutionActionEnum.ResolutionCreation => "Resolution Created",
                ResolutionActionEnum.ResolutionEdit => "Resolution Edited",
                ResolutionActionEnum.ResolutionCompletion => "Resolution Completed",
                ResolutionActionEnum.ResolutionConfirmation => "Resolution Confirmed",
                ResolutionActionEnum.ResolutionRejection => "Resolution Rejected",
                ResolutionActionEnum.ResolutionCancellation => "Resolution Cancelled",
                ResolutionActionEnum.SentToVote => "Resolution Sent to Vote",
                ResolutionActionEnum.ResolutionVoteSuspend => "Voting Suspended",
                ResolutionActionEnum.ResolutionDeletion => "Resolution Deleted",
                _ => historyEntry.Action.ToString()
            };
        }

        public string GetLocalizedActionName(ResolutionActionEnum status, string? culture = null)
        {
            try
            {
                // Map status enum to resource key
                var statusKey = status switch
                {
                    ResolutionActionEnum.ResolutionCreation => _localizer[SharedResourcesKey.AuditActionResolutionCreation],
                    ResolutionActionEnum.ResolutionEdit => _localizer[SharedResourcesKey.AuditActionResolutionEdit],
                    ResolutionActionEnum.ResolutionCompletion => _localizer[SharedResourcesKey.AuditActionResolutionCompletion],
                    ResolutionActionEnum.ResolutionConfirmation => _localizer[SharedResourcesKey.AuditActionResolutionConfirmation],
                    ResolutionActionEnum.ResolutionRejection => _localizer[SharedResourcesKey.AuditActionResolutionRejection],
                    ResolutionActionEnum.ResolutionCancellation => _localizer[SharedResourcesKey.AuditActionResolutionCancellation],
                    ResolutionActionEnum.ResolutionApproved => _localizer[SharedResourcesKey.AuditActionResolutionApproved],
                    ResolutionActionEnum.ResolutionUnApproved => _localizer[SharedResourcesKey.AuditActionResolutionUnApproved],
                    ResolutionActionEnum.Confirmed => _localizer[SharedResourcesKey.AuditActionResolutionConfirmation],
                    ResolutionActionEnum.SentToVote => _localizer[SharedResourcesKey.AuditActionResolutionSentToVote],
                    ResolutionActionEnum.ResolutionVoteSuspend => _localizer[SharedResourcesKey.AuditActionResolutionVoteSuspend],
                    ResolutionActionEnum.ResolutionDeletion => _localizer[SharedResourcesKey.AuditActionResolutionDeletion],
                    _ => status.ToString()
                };

                var localizedStatus = _localizer[statusKey].Value;

                // If localization not found, return enum name
                return localizedStatus == statusKey ? localizedStatus : status.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, String.Format("Error getting localized status name for {Status}", status));
                return status.ToString();
            }
        }

        public string GetLocalizedRole(string role, string? culture = null)
        {
             return _localizer[role].Value;
        }
         
    }
}
