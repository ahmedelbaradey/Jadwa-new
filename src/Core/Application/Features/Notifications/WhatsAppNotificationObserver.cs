using Core.Abstraction.Contract.Service.Notifications;
using Domain.Entities.Notifications;
using Domain.Entities.Users;
using Domain.Exceptions;
using Abstraction.Contracts.Logger;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Abstraction.Contract.Repository.Notifications;
using Abstraction.Enums;

namespace Application.Features.Notification
{
    /// <summary>
    /// WhatsApp notification observer that integrates with the existing notification system
    /// Sends WhatsApp messages when notifications are triggered through the observer pattern
    /// </summary>
    public class WhatsAppNotificationObserver : IFundNotificationObserver
    {
        private readonly IWhatsAppNotificationService _whatsAppService;
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;

        public WhatsAppNotificationObserver(
            IWhatsAppNotificationService whatsAppService,
            UserManager<User> userManager,
            ILoggerManager logger)
        {
            _whatsAppService = whatsAppService ?? throw new ArgumentNullException(nameof(whatsAppService));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task OnSendNotification(MessageRequest message)
        {
            try
            {
                _logger.LogInfo($"Processing WhatsApp notification for user {message.UserId}");

                // Get user information
                var user = await _userManager.FindByIdAsync(message.UserId.ToString());
                if (user == null)
                {
                    _logger.LogWarn($"User {message.UserId} not found for WhatsApp notification");
                    return;
                }

                // Get user's phone number
                var phoneNumber = await GetUserPhoneNumberAsync(user);
                if (string.IsNullOrEmpty(phoneNumber))
                {
                    _logger.LogWarn($"No phone number found for user {message.UserId}");
                    return;
                }

                // Map notification type to WhatsApp message type
                var whatsAppMessageType = MapNotificationTypeToWhatsApp(message.Type);
                if (!whatsAppMessageType.HasValue)
                {
                    _logger.LogInfo($"Notification type {message.Type} not supported for WhatsApp");
                    return;
                }

                // Send WhatsApp message
                var response = await _whatsAppService.SendLocalizedMessageAsync(
                    message.UserId,
                    phoneNumber,
                    whatsAppMessageType.Value,
                    message.Parameters,
                    CancellationToken.None);

                if (response.IsSuccess)
                {
                    _logger.LogInfo($"WhatsApp notification sent successfully to user {message.UserId}. MessageId: {response.MessageId}");
                }
                else
                {
                    _logger.LogError(null,$"Failed to send WhatsApp notification to user {message.UserId}. Error: {response.ErrorMessage}");
                }
            }
            catch (WhatsAppNotificationException ex)
            {
                _logger.LogError(ex, $"WhatsApp-specific error sending notification to user {message.UserId}: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error sending WhatsApp notification to user {message.UserId}");
            }
        }

        /// <summary>
        /// Gets user's phone number from their profile or claims
        /// </summary>
        private async Task<string?> GetUserPhoneNumberAsync(User user)
        {
            // First try to get phone number from user profile
            if (!string.IsNullOrEmpty(user.PhoneNumber))
            {
                return FormatSaudiPhoneNumber(user.PhoneNumber);
            }

            // Try to get from user claims if not in profile
            var claims = await _userManager.GetClaimsAsync(user);
            var phoneClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.MobilePhone);
            
            if (phoneClaim != null && !string.IsNullOrEmpty(phoneClaim.Value))
            {
                return FormatSaudiPhoneNumber(phoneClaim.Value);
            }

            return null;
        }

        /// <summary>
        /// Formats phone number to Saudi format (+966XXXXXXXXX)
        /// </summary>
        private string? FormatSaudiPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return null;

            // Remove all non-digit characters
            var digits = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // Handle different input formats
            if (digits.StartsWith("966") && digits.Length == 12)
            {
                return $"+{digits}";
            }
            else if (digits.StartsWith("5") && digits.Length == 9)
            {
                return $"+966{digits}";
            }
            else if (digits.StartsWith("05") && digits.Length == 10)
            {
                return $"+966{digits.Substring(1)}";
            }

            // Return null if format is not recognized
            return null;
        }

        /// <summary>
        /// Maps existing notification types to WhatsApp message types
        /// Only maps notification types that are relevant for WhatsApp messaging
        /// </summary>
        private WhatsAppMessageType? MapNotificationTypeToWhatsApp(NotificationType notificationType)
        {
            return notificationType switch
            {
                // Map specific notification types that should trigger WhatsApp messages
                // Based on Sprint 3 requirements, these would be user account related notifications
                
                // Note: These mappings should be adjusted based on your specific NotificationType enum values
                // and business requirements for which notifications should be sent via WhatsApp
                
                NotificationType.AddedToFund => null, // Fund notifications typically go through Firebase
                NotificationType.RemoveFromFund => null, // Fund notifications typically go through Firebase
                
                // Add mappings for user account related notifications when they are added to NotificationType enum
                // NotificationType.PasswordReset => WhatsAppMessageType.PasswordReset,
                // NotificationType.UserRegistration => WhatsAppMessageType.UserRegistration,
                // NotificationType.AccountActivation => WhatsAppMessageType.AccountActivation,
                // NotificationType.AccountDeactivation => WhatsAppMessageType.AccountDeactivation,
                
                _ => null // Default: don't send WhatsApp for other notification types
            };
        }
    }

    /// <summary>
    /// Extension methods for integrating WhatsApp notifications with existing notification flows
    /// </summary>
    public static class WhatsAppNotificationExtensions
    {
        /// <summary>
        /// Adds WhatsApp observer to the notification observable
        /// Call this method to enable WhatsApp notifications alongside Firebase notifications
        /// </summary>
        public static void AddWhatsAppNotifications(
            this FundNotificationObservable observable,
            IWhatsAppNotificationService whatsAppService,
            UserManager<User> userManager,
            ILoggerManager logger)
        {
            var whatsAppObserver = new WhatsAppNotificationObserver(whatsAppService, userManager, logger);
            observable.Subscribe(whatsAppObserver);
        }

        /// <summary>
        /// Sends direct WhatsApp notification for user account operations
        /// Use this for operations like password reset, user registration, etc.
        /// </summary>
        public static async Task SendWhatsAppUserNotificationAsync(
            this IWhatsAppNotificationService whatsAppService,
            int userId,
            string phoneNumber,
            WhatsAppMessageType messageType,
            params object[] parameters)
        {
            await whatsAppService.SendLocalizedMessageAsync(
                userId,
                phoneNumber,
                messageType,
                parameters,
                CancellationToken.None);
        }
    }
}
