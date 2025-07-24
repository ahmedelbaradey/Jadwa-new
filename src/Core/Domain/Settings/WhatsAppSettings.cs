namespace Domain.Settings
{
    /// <summary>
    /// Configuration settings for WhatsApp Business API integration
    /// Used to configure the WhatsApp notification service
    /// </summary>
    public class WhatsAppSettings
    {
        /// <summary>
        /// WhatsApp Business API base URL
        /// Example: https://graph.facebook.com
        /// </summary>
        public string ApiUrl { get; set; } = string.Empty;

        /// <summary>
        /// WhatsApp Business API access token
        /// Required for authentication with WhatsApp API
        /// </summary>
        public string ApiToken { get; set; } = string.Empty;

        /// <summary>
        /// WhatsApp Business phone number ID
        /// The ID of the phone number registered with WhatsApp Business
        /// </summary>
        public string PhoneNumberId { get; set; } = string.Empty;

        /// <summary>
        /// WhatsApp API version to use
        /// Default: v17.0
        /// </summary>
        public string Version { get; set; } = "v17.0";

        /// <summary>
        /// Request timeout in seconds
        /// Default: 30 seconds
        /// </summary>
        public int TimeoutSeconds { get; set; } = 30;

        /// <summary>
        /// Maximum retry attempts for failed requests
        /// Default: 3 attempts
        /// </summary>
        public int MaxRetryAttempts { get; set; } = 3;

        /// <summary>
        /// Delay between retry attempts in milliseconds
        /// Default: 1000ms (1 second)
        /// </summary>
        public int RetryDelayMs { get; set; } = 1000;

        /// <summary>
        /// Whether to enable WhatsApp notifications
        /// Can be used to disable WhatsApp service in certain environments
        /// </summary>
        public bool Enabled { get; set; } = true;

        /// <summary>
        /// Validates that all required settings are provided
        /// </summary>
        /// <returns>True if configuration is valid, false otherwise</returns>
        public bool IsValid()
        {
            return !string.IsNullOrEmpty(ApiUrl) &&
                   !string.IsNullOrEmpty(ApiToken) &&
                   !string.IsNullOrEmpty(PhoneNumberId) &&
                   !string.IsNullOrEmpty(Version);
        }

        /// <summary>
        /// Gets validation error messages for invalid configuration
        /// </summary>
        /// <returns>List of validation error messages</returns>
        public List<string> GetValidationErrors()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(ApiUrl))
                errors.Add("WhatsApp ApiUrl is required");

            if (string.IsNullOrEmpty(ApiToken))
                errors.Add("WhatsApp ApiToken is required");

            if (string.IsNullOrEmpty(PhoneNumberId))
                errors.Add("WhatsApp PhoneNumberId is required");

            if (string.IsNullOrEmpty(Version))
                errors.Add("WhatsApp Version is required");

            if (TimeoutSeconds <= 0)
                errors.Add("WhatsApp TimeoutSeconds must be greater than 0");

            if (MaxRetryAttempts < 0)
                errors.Add("WhatsApp MaxRetryAttempts must be 0 or greater");

            if (RetryDelayMs < 0)
                errors.Add("WhatsApp RetryDelayMs must be 0 or greater");

            return errors;
        }
    }
}
