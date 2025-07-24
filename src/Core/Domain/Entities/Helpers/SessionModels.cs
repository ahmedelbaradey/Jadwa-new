namespace Domain.Helpers
{
    /// <summary>
    /// Session information response model for API endpoints
    /// Contains current session status and timing information
    /// </summary>
    public record SessionInfo
    {
        /// <summary>
        /// Session identifier
        /// </summary>
        public string SessionId { get; set; } = null!;

        /// <summary>
        /// Whether the session is currently active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Whether the session is expired
        /// </summary>
        public bool IsExpired { get; set; }

        /// <summary>
        /// Session expiration timestamp
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Remaining session time in seconds
        /// </summary>
        public int RemainingSeconds { get; set; }

        /// <summary>
        /// Last activity timestamp
        /// </summary>
        public DateTime LastActivityAt { get; set; }

        /// <summary>
        /// Session creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }
    }

    /// <summary>
    /// Session validation response model
    /// </summary>
    public record SessionValidationResponse
    {
        /// <summary>
        /// Whether the session is valid
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Validation failure reason
        /// </summary>
        public string? FailureReason { get; set; }

        /// <summary>
        /// Session information if valid
        /// </summary>
        public SessionInfo? SessionInfo { get; set; }

        /// <summary>
        /// Whether session was extended due to activity
        /// </summary>
        public bool WasExtended { get; set; }
    }
}
