using Abstraction.Enums;

namespace Abstraction.Base.Dto
{
    /// <summary>
    /// Data Transfer Object representing the response from WhatsApp message sending operation
    /// Contains delivery status and tracking information
    /// </summary>
    public class WhatsAppMessageResponseDto
    {
        /// <summary>
        /// Indicates whether the message was successfully sent
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// Unique message ID from WhatsApp API for tracking
        /// Used to check delivery status later
        /// </summary>
        public string? MessageId { get; set; }

        /// <summary>
        /// Current delivery status of the message
        /// </summary>
        public WhatsAppDeliveryStatus DeliveryStatus { get; set; }

        /// <summary>
        /// Error message if sending failed
        /// Contains details about the failure reason
        /// </summary>
        public string? ErrorMessage { get; set; }

        /// <summary>
        /// Error code from WhatsApp API if applicable
        /// Used for specific error handling
        /// </summary>
        public string? ErrorCode { get; set; }

        /// <summary>
        /// Timestamp when the message was sent
        /// </summary>
        public DateTime SentAt { get; set; }

        /// <summary>
        /// Phone number the message was sent to
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// User ID associated with the message
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Type of message that was sent
        /// </summary>
        public WhatsAppMessageType MessageType { get; set; }

        /// <summary>
        /// Additional metadata from WhatsApp API response
        /// </summary>
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
