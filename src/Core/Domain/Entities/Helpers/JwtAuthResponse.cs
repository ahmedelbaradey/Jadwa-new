namespace Domain.Helpers
{
    public record JwtAuthResponse
    {
        public string? AccessToken { get; set; }
        public RefreshToken refreshToken { get; set; } = null!;
        public int UserId { get; set; }
        /// <summary>
        /// Redirect URL for frontend navigation after login
        /// Sprint 3 enhancement for conditional redirection
        /// </summary>
        public bool? IsFirstLogin { get; set; }

        /// <summary>
        /// Session timeout information for frontend
        /// </summary>
        public SessionTimeoutInfo? SessionTimeout { get; set; }

        /// <summary>
        /// Session ID for tracking
        /// </summary>
        public string? SessionId { get; set; }
    }

    public record RefreshToken
    {
        public string UserName { get; set; } = null!;
        public DateTime ExpireAt { get; set; }
        public string TokenString { get; set; } = null!;
    }

    /// <summary>
    /// Session timeout information for frontend
    /// </summary>
    public record SessionTimeoutInfo
    {
        /// <summary>
        /// Session timeout in minutes
        /// </summary>
        public int TimeoutMinutes { get; set; }

        /// <summary>
        /// Whether sliding expiration is enabled
        /// </summary>
        public bool SlidingExpiration { get; set; }

        /// <summary>
        /// Session expiration timestamp
        /// </summary>
        public DateTime ExpiresAt { get; set; }
    }
}
