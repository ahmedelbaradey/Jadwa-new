
using Abstraction.Enums;

namespace Abstraction.Base.Dto
{
    /// <summary>
    /// Data Transfer Object for WhatsApp message requests
    /// Used by IWhatsAppNotificationService for message delivery
    /// </summary>
    public class WhatsAppMessageRequestDto
    {
        /// <summary>
        /// Target user ID for the message
        /// Used for localization and audit purposes
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Saudi mobile phone number in format +966XXXXXXXXX
        /// Must be validated before sending
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Type of WhatsApp message being sent
        /// Determines the message template and content
        /// </summary>
        public WhatsAppMessageType MessageType { get; set; }

        /// <summary>
        /// Message content/body to be sent
        /// Can be pre-formatted or will be generated from template
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Optional culture code for localization (e.g., "ar-EG", "en-US")
        /// If not provided, will use user's preferred language
        /// </summary>
        public string? Culture { get; set; }

        /// <summary>
        /// Parameters for message template formatting
        /// Used to populate placeholders in message templates
        /// </summary>
        public object[]? Parameters { get; set; }

        /// <summary>
        /// Optional metadata for tracking and audit purposes
        /// Can include additional context information
        /// </summary>
        public Dictionary<string, object>? Metadata { get; set; }

        /// <summary>
        /// Priority level for message delivery
        /// Higher priority messages may be processed first
        /// </summary>
        public MessagePriority Priority { get; set; } = MessagePriority.Normal;
    }
}
