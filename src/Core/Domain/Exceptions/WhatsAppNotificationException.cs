namespace Domain.Exceptions
{
    /// <summary>
    /// Base exception for WhatsApp notification service errors
    /// </summary>
    public class WhatsAppNotificationException : Exception
    {
        public string? ErrorCode { get; }
        public string? PhoneNumber { get; }
        public int? UserId { get; }

        public WhatsAppNotificationException(string message) : base(message)
        {
        }

        public WhatsAppNotificationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public WhatsAppNotificationException(string message, string? errorCode, string? phoneNumber = null, int? userId = null) 
            : base(message)
        {
            ErrorCode = errorCode;
            PhoneNumber = phoneNumber;
            UserId = userId;
        }
    }

    /// <summary>
    /// Exception thrown when phone number validation fails
    /// </summary>
    public class InvalidPhoneNumberException : WhatsAppNotificationException
    {
        public InvalidPhoneNumberException(string phoneNumber) 
            : base($"Invalid phone number format: {phoneNumber}. Saudi phone numbers must be in format +966XXXXXXXXX", "INVALID_PHONE", phoneNumber)
        {
        }
    }

    /// <summary>
    /// Exception thrown when WhatsApp API authentication fails
    /// </summary>
    public class WhatsAppAuthenticationException : WhatsAppNotificationException
    {
        public WhatsAppAuthenticationException(string message) 
            : base($"WhatsApp API authentication failed: {message}", "AUTH_FAILED")
        {
        }
    }

    /// <summary>
    /// Exception thrown when WhatsApp API rate limit is exceeded
    /// </summary>
    public class WhatsAppRateLimitException : WhatsAppNotificationException
    {
        public DateTime RetryAfter { get; }

        public WhatsAppRateLimitException(DateTime retryAfter) 
            : base($"WhatsApp API rate limit exceeded. Retry after: {retryAfter}", "RATE_LIMIT_EXCEEDED")
        {
            RetryAfter = retryAfter;
        }
    }

    /// <summary>
    /// Exception thrown when message template is not found or invalid
    /// </summary>
    public class WhatsAppTemplateException : WhatsAppNotificationException
    {
        public string TemplateType { get; }

        public WhatsAppTemplateException(string templateType, string message) 
            : base($"WhatsApp template error for {templateType}: {message}", "TEMPLATE_ERROR")
        {
            TemplateType = templateType;
        }
    }

    /// <summary>
    /// Exception thrown when message delivery fails
    /// </summary>
    public class WhatsAppDeliveryException : WhatsAppNotificationException
    {
        public string MessageId { get; }

        public WhatsAppDeliveryException(string messageId, string message, string? errorCode = null) 
            : base($"WhatsApp message delivery failed (ID: {messageId}): {message}", errorCode ?? "DELIVERY_FAILED")
        {
            MessageId = messageId;
        }
    }
}
