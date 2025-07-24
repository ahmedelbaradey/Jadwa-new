namespace Domain.Helpers
{
    /// <summary>
    /// Configuration settings for comprehensive session management
    /// Supports 30-minute timeout with sliding expiration, role-based timeouts, and Redis caching
    /// </summary>
    public record SessionSettings
    {
        /// <summary>
        /// Default session timeout duration in minutes (default: 30 minutes)
        /// </summary>
        public int TimeoutMinutes { get; set; } = 30;

        /// <summary>
        /// Enable sliding expiration - resets timeout on user activity
        /// </summary>
        public bool EnableSlidingExpiration { get; set; } = true;

        /// <summary>
        /// Session timeout in TimeSpan format
        /// </summary>
        public TimeSpan SessionTimeout => TimeSpan.FromMinutes(TimeoutMinutes);
         

    }
}
