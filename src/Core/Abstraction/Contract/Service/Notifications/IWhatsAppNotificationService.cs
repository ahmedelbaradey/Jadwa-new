
using Abstraction.Base.Dto;
using Abstraction.Enums;

namespace Core.Abstraction.Contract.Service.Notifications
{
    /// <summary>
    /// Interface for WhatsApp notification service
    /// Provides methods for sending WhatsApp messages to users based on Sprint 3 requirements
    /// </summary>
    public interface IWhatsAppNotificationService
    {
        /// <summary>
        /// Sends a WhatsApp message using the provided request details
        /// </summary>
        /// <param name="request">WhatsApp message request containing all necessary information</param>
        /// <param name="cancellationToken">Cancellation token for async operation</param>
        /// <returns>Response containing delivery status and tracking information</returns>
        /// <exception cref="InvalidPhoneNumberException">Thrown when phone number format is invalid</exception>
        /// <exception cref="WhatsAppAuthenticationException">Thrown when API authentication fails</exception>
        /// <exception cref="WhatsAppRateLimitException">Thrown when API rate limit is exceeded</exception>
        /// <exception cref="WhatsAppDeliveryException">Thrown when message delivery fails</exception>
        Task<WhatsAppMessageResponseDto> SendMessageAsync(WhatsAppMessageRequestDto request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a localized WhatsApp message using predefined templates
        /// Automatically retrieves user's preferred language and formats message
        /// </summary>
        /// <param name="userId">Target user ID</param>
        /// <param name="phoneNumber">Saudi mobile phone number (+966XXXXXXXXX)</param>
        /// <param name="messageType">Type of message to send</param>
        /// <param name="parameters">Parameters for message template formatting</param>
        /// <param name="cancellationToken">Cancellation token for async operation</param>
        /// <returns>Response containing delivery status and tracking information</returns>
        Task<WhatsAppMessageResponseDto> SendLocalizedMessageAsync(
            int userId,
            string phoneNumber,
            WhatsAppMessageType messageType,
            object[]? parameters = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Validates Saudi phone number format
        /// Ensures phone number is in correct format (+966XXXXXXXXX) before sending
        /// </summary>
        /// <param name="phoneNumber">Phone number to validate</param>
        /// <returns>True if phone number is valid, false otherwise</returns>
        bool ValidatePhoneNumber(string phoneNumber);

        /// <summary>
        /// Retrieves the current delivery status of a previously sent message
        /// </summary>
        /// <param name="messageId">WhatsApp message ID returned from SendMessageAsync</param>
        /// <param name="cancellationToken">Cancellation token for async operation</param>
        /// <returns>Current delivery status of the message</returns>
        Task<WhatsAppDeliveryStatus> GetDeliveryStatusAsync(string messageId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends password reset message with temporary password
        /// Implements MSG-RESET-006 from Sprint 3 requirements
        /// </summary>
        /// <param name="userId">User ID receiving the password reset</param>
        /// <param name="phoneNumber">User's phone number</param>
        /// <param name="temporaryPassword">New temporary password</param>
        /// <param name="cancellationToken">Cancellation token for async operation</param>
        /// <returns>Response containing delivery status and tracking information</returns>
        Task<WhatsAppMessageResponseDto> SendPasswordResetMessageAsync(
            int userId,
            string phoneNumber,
            string temporaryPassword,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends user registration confirmation message
        /// Implements MSG-ADD-008 from Sprint 3 requirements
        /// </summary>
        /// <param name="userId">New user ID</param>
        /// <param name="phoneNumber">User's phone number</param>
        /// <param name="username">Username for login</param>
        /// <param name="loginUrl">URL for system access</param>
        /// <param name="cancellationToken">Cancellation token for async operation</param>
        /// <returns>Response containing delivery status and tracking information</returns>
        Task<WhatsAppMessageResponseDto> SendUserRegistrationMessageAsync(
            int userId,
            string phoneNumber,
            string username,
            string loginUrl,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends account activation notification message
        /// Implements MSG-ACTDEACT-009 from Sprint 3 requirements
        /// </summary>
        /// <param name="userId">User ID being activated</param>
        /// <param name="phoneNumber">User's phone number</param>
        /// <param name="cancellationToken">Cancellation token for async operation</param>
        /// <returns>Response containing delivery status and tracking information</returns>
        Task<WhatsAppMessageResponseDto> SendAccountActivationMessageAsync(
            int userId,
            string phoneNumber,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends account deactivation notification message
        /// Implements MSG-ACTDEACT-010 from Sprint 3 requirements
        /// </summary>
        /// <param name="userId">User ID being deactivated</param>
        /// <param name="phoneNumber">User's phone number</param>
        /// <param name="cancellationToken">Cancellation token for async operation</param>
        /// <returns>Response containing delivery status and tracking information</returns>
        Task<WhatsAppMessageResponseDto> SendAccountDeactivationMessageAsync(
            int userId,
            string phoneNumber,
            CancellationToken cancellationToken = default);
    }
}
