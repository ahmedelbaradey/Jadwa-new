using Abstraction.Constants;
using Abstraction.Contract.Service.Notifications;
using Abstraction.Contracts.Logger;
using Domain.Entities.FundManagement;
using Domain.Entities.Notifications;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Resources;
using System.Globalization;

namespace Infrastructure.Service.Notifications
{
    public class NotificationLocalizationService : INotificationLocalizationService
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;

        public NotificationLocalizationService(
            IStringLocalizer<SharedResources> localizer,
            UserManager<User> userManager,
            ILoggerManager logger)
        {
            _localizer = localizer;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<LocalizedNotificationMessage> GetLocalizedNotificationAsync(int userId, NotificationType notificationType, params object[] parameters)
        {
            try
            {
                var userLanguage = await GetUserPreferredLanguageAsync(userId);
                return GetLocalizedNotification(userLanguage, notificationType, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting localized notification for user {UserId}");
                // Fallback to default language (Arabic)
                return GetLocalizedNotification("ar-EG", notificationType, parameters);
            }
        }

        public LocalizedNotificationMessage GetLocalizedNotification(string culture, NotificationType notificationType, params object[] parameters)
        {
            try
            {
                // Set the current culture for localization
                var originalCulture = CultureInfo.CurrentCulture;
                var originalUICulture = CultureInfo.CurrentUICulture;

                try
                {
                    var cultureInfo = new CultureInfo(culture);
                    CultureInfo.CurrentCulture = cultureInfo;
                    CultureInfo.CurrentUICulture = cultureInfo;

                    var (titleKey, bodyKey) = GetNotificationKeys(notificationType);

                    var title = _localizer[titleKey].Value;
                    var bodyTemplate = _localizer[bodyKey].Value;

                    // Format the body with parameters if provided
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        var str = parameters[i]?.ToString();

                        if (!string.IsNullOrWhiteSpace(str) && (IsValidEnumName<Roles>(str) || IsValidEnumName<BoardMemberType>(str)))
                        {
                            parameters[i] = _localizer[str].Value;
                        }

                    }

                    var body = parameters?.Length > 0 ? string.Format(bodyTemplate, parameters) : bodyTemplate;

                    return new LocalizedNotificationMessage
                    {
                        Title = title,
                        Body = body,
                        Culture = culture
                    };
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
                _logger.LogError(ex, "Error getting localized notification for culture {Culture}");

                // Fallback to English with basic message
                return new LocalizedNotificationMessage
                {
                    Title = GetFallbackTitle(notificationType),
                    Body = GetFallbackBody(notificationType, parameters),
                    Culture = "en-US"
                };
            }
        }

        public async Task<string> GetUserPreferredLanguageAsync(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                return user?.PreferredLanguage ?? "ar-EG"; // Default to Arabic
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user preferred language for user {UserId}");
                return "ar-EG"; // Default to Arabic
            }
        }

        private static (string titleKey, string bodyKey) GetNotificationKeys(NotificationType notificationType)
        {
            return notificationType switch
            {
                NotificationType.AddedToFund => (SharedResourcesKey.AddFundNotificationTitle, SharedResourcesKey.AddFundNotificationBody),
                NotificationType.AddedToFundForManager => (SharedResourcesKey.AddFundNotificationTitle, SharedResourcesKey.AddFundForManagerNotificationBody),
                NotificationType.RemoveFromFund => (SharedResourcesKey.RemoveFromFundNotificationTitle, SharedResourcesKey.RemoveFromFundNotificationBody),
                NotificationType.ChangeExitDate => (SharedResourcesKey.ChangeExitDateNotificationTitle, SharedResourcesKey.ChangeExitDateNotificationBody),
                NotificationType.CompeleteFund => (SharedResourcesKey.CompeleteFundNotificationTitle, SharedResourcesKey.CompeleteFundNotificationBody),
                NotificationType.ResolutionVotingSuspended => (SharedResourcesKey.ResolutionVotingSuspendedNotificationTitle, SharedResourcesKey.ResolutionVotingSuspendedNotificationBody),
                NotificationType.ResolutionCreated => (SharedResourcesKey.ResolutionCreatedNotificationTitle, SharedResourcesKey.ResolutionCreatedNotificationBody),
                NotificationType.ResolutionUpdated => (SharedResourcesKey.ResolutionUpdatedNotificationTitle, SharedResourcesKey.ResolutionUpdatedNotificationBody),
                NotificationType.ResolutionCancelled => (SharedResourcesKey.ResolutionCancelledNotificationTitle, SharedResourcesKey.ResolutionCancelledNotificationBody),
                NotificationType.ResolutionConfirmed => (SharedResourcesKey.ResolutionConfirmedNotificationTitle, SharedResourcesKey.ResolutionConfirmedNotificationBody),
                NotificationType.ResolutionRejected => (SharedResourcesKey.ResolutionRejectedNotificationTitle, SharedResourcesKey.ResolutionRejectedNotificationBody),
                NotificationType.ResolutionSentToVote => (SharedResourcesKey.ResolutionSentToVoteNotificationTitle, SharedResourcesKey.ResolutionSentToVoteNotificationBody),
                NotificationType.FundActivated => (SharedResourcesKey.FundActivatedNotificationTitle, SharedResourcesKey.FundActivatedNotificationBody),
                NotificationType.BoardMemberAdded => (SharedResourcesKey.BoardMemberAddedNotificationTitle, SharedResourcesKey.BoardMemberAddedNotificationBody),
                NotificationType.BoardMemberAddedToFund => (SharedResourcesKey.BoardMemberAddedToFundNotificationTitle, SharedResourcesKey.BoardMemberAddedToFundNotificationBody),
                NotificationType.NewResolutionCreatedFromApproved => (SharedResourcesKey.NewResolutionCreatedFromApprovedNotificationTitle, SharedResourcesKey.NewResolutionCreatedFromApprovedNotificationBody),
                NotificationType.ResolutionDataCompleted => (SharedResourcesKey.ResolutionDataCompletedNotificationTitle, SharedResourcesKey.ResolutionDataCompletedNotificationBody),
                NotificationType.UserRelieveOfDuties => (SharedResourcesKey.EditUserRelieveOfDutiesNotificationTitle, SharedResourcesKey.EditUserRelieveOfDutiesNotification),
                NotificationType.UserRoleUpdate => (SharedResourcesKey.EditUserRoleUpdateNotificationTitle, SharedResourcesKey.EditUserRoleUpdateNotification),
                _ => (SharedResourcesKey.AddFundNotificationTitle, SharedResourcesKey.AddFundNotificationBody)
            };
        }

        private static string GetFallbackTitle(NotificationType notificationType)
        {
            return notificationType switch
            {
                NotificationType.AddedToFund => "Added to Fund",
                NotificationType.RemoveFromFund => "Removed from Fund",
                NotificationType.ChangeExitDate => "Exit Date Changed",
                NotificationType.CompeleteFund => "Fund Completed",
                NotificationType.ResolutionCreated => "Resolution Created",
                NotificationType.ResolutionUpdated => "Resolution Updated",
                NotificationType.ResolutionCancelled => "Resolution Cancelled",
                NotificationType.ResolutionConfirmed => "Resolution Confirmed",
                NotificationType.ResolutionRejected => "Resolution Rejected",
                NotificationType.ResolutionSentToVote => "Resolution Sent to Vote",
                NotificationType.FundActivated => "Fund Activated",
                NotificationType.NewResolutionCreatedFromApproved => "New Resolution Created",
                NotificationType.ResolutionVotingSuspended => "Voting Suspended",
                NotificationType.ResolutionDataCompleted => "Resolution Data Completed",
                NotificationType.UserRelieveOfDuties => "Relieve of Duties",
                NotificationType.UserRoleUpdate => "Update Duties",
                _ => "Notification"
            };
        }

        private static string GetFallbackBody(NotificationType notificationType, params object[] parameters)
        {
            var message = notificationType switch
            {
                NotificationType.AddedToFund => "A new fund is added with name {0} by {1}, you are assigned as {2}",
                NotificationType.RemoveFromFund => "You have been relieved of your role as {0} in fund {1} by {2}",
                NotificationType.ChangeExitDate => "The exit date for the fund {0} has been modified to {1} by {2}",
                NotificationType.CompeleteFund => "Data is completed for the fund {0}, by {1}",
                NotificationType.ResolutionCreated => "A new resolution is added attached to fund {0} by fund manager {1}, kindly complete resolution info.",
                NotificationType.ResolutionUpdated => "Resolution {0} in fund {1} has been updated by {2} {3}",
                NotificationType.ResolutionCancelled => "Resolution {0} in fund {1} has been cancelled by fund manager {2}",
                NotificationType.ResolutionConfirmed => "Resolution {0} in fund {1} has been confirmed by {2}",
                NotificationType.ResolutionRejected => "Resolution {0} in fund {1} has been rejected by {2}. Reason: {3}",
                NotificationType.ResolutionSentToVote => "Resolution {0} in fund {1} has been sent to vote by {2}",
                NotificationType.FundActivated => "Fund {0} is successfully activated, as 2 independent members are attached to it",
                NotificationType.NewResolutionCreatedFromApproved => "A new resolution is added to fund {0} by {1} {2}, as an update on resolution no. {3}",
                NotificationType.ResolutionVotingSuspended => "Voting has been suspended for resolution {0} in fund {1} by {2} {3} for editing",
                NotificationType.ResolutionDataCompleted => "Data is completed for the resolution code {0} attached to fund {1}, by {2}",
                NotificationType.UserRelieveOfDuties => "You have been relieved of your duties as {0}",
                NotificationType.UserRoleUpdate => "Your duties have been updated: you have been relieved of your duties as {0} and assigned as {1}",
                _ => "You have a new notification."
            };

            return null;
        }
        private bool IsValidEnumName<TEnum>(string value) where TEnum : struct, Enum
        {
            return Enum.GetNames(typeof(TEnum)).Any(name => name.Equals(value, StringComparison.OrdinalIgnoreCase));
        }
    }
}
