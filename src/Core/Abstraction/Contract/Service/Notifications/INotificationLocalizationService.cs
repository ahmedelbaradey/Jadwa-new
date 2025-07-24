using Domain.Entities.Notifications;

namespace Abstraction.Contract.Service.Notifications
{
    public interface INotificationLocalizationService
    {
        /// <summary>
        /// Gets localized notification message based on user's preferred language
        /// </summary>
        /// <param name="userId">User ID to determine language preference</param>
        /// <param name="notificationType">Type of notification</param>
        /// <param name="parameters">Parameters for message formatting</param>
        /// <returns>Localized notification message</returns>
        Task<LocalizedNotificationMessage> GetLocalizedNotificationAsync(int userId, NotificationType notificationType, params object[] parameters);

        /// <summary>
        /// Gets localized notification message for a specific culture
        /// </summary>
        /// <param name="culture">Culture code (e.g., "ar-EG", "en-US")</param>
        /// <param name="notificationType">Type of notification</param>
        /// <param name="parameters">Parameters for message formatting</param>
        /// <returns>Localized notification message</returns>
        LocalizedNotificationMessage GetLocalizedNotification(string culture, NotificationType notificationType, params object[] parameters);

        /// <summary>
        /// Gets user's preferred language
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>User's preferred language culture code</returns>
        Task<string> GetUserPreferredLanguageAsync(int userId);
    }

    public class LocalizedNotificationMessage
    {
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public string Culture { get; set; } = string.Empty;
    }
}
