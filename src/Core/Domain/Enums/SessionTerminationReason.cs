namespace Domain.Enums
{
    /// <summary>
    /// Enumeration of possible session termination reasons
    /// Used for audit logging and session management
    /// </summary>
    public enum SessionTerminationReason
    {
        /// <summary>
        /// User initiated logout
        /// </summary>
        UserLogout = 1,

        /// <summary>
        /// Session expired due to inactivity timeout
        /// </summary>
        InactivityTimeout = 2,

        /// <summary>
        /// Session terminated for security reasons (suspicious activity, fingerprint mismatch)
        /// </summary>
        SecurityViolation = 3,

        /// <summary>
        /// Session terminated by administrator
        /// </summary>
        AdminTermination = 4,

        /// <summary>
        /// Session terminated due to password change
        /// </summary>
        PasswordChange = 5,

        /// <summary>
        /// Session terminated due to account deactivation
        /// </summary>
        AccountDeactivation = 6,

        /// <summary>
        /// Session terminated due to maximum concurrent sessions exceeded
        /// </summary>
        ConcurrentSessionLimit = 7,

        /// <summary>
        /// Session terminated due to system maintenance
        /// </summary>
        SystemMaintenance = 8,

        /// <summary>
        /// Session terminated due to token expiration
        /// </summary>
        TokenExpiration = 9,

        /// <summary>
        /// Session terminated due to system error
        /// </summary>
        SystemError = 10,

        /// <summary>
        /// Session terminated due to role change
        /// </summary>
        RoleChange = 11,

        /// <summary>
        /// Session terminated due to forced logout from all devices
        /// </summary>
        ForcedLogoutAllDevices = 12
    }
}
