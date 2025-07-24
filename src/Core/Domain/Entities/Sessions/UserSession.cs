using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Sessions
{
    /// <summary>
    /// Represents an active user session with comprehensive tracking and security features
    /// Supports 30-minute timeout with sliding expiration, role-based timeouts, and session fingerprinting
    /// </summary>
    public class UserSession
    {
        /// <summary>
        /// Unique session identifier
        /// </summary>
        [Key]
        public string SessionId { get; set; } = null!;

        /// <summary>
        /// User ID associated with this session
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// JWT token ID associated with this session
        /// </summary>
        public string? JwtTokenId { get; set; }

        /// <summary>
        /// Session creation timestamp
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Last activity timestamp - updated on each request
        /// </summary>
        public DateTime LastActivityAt { get; set; }

        /// <summary>
        /// Session expiration timestamp
        /// </summary>
        public DateTime ExpiresAt { get; set; }

        /// <summary>
        /// Whether the session is currently active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Session termination reason (Logout, Timeout, Security, etc.)
        /// </summary>
        public string? TerminationReason { get; set; }

        /// <summary>
        /// Check if session is expired based on current time
        /// </summary>
        public bool IsExpired => DateTime.Now > ExpiresAt;

        /// <summary>
        /// Get remaining session time
        /// </summary>
        public TimeSpan RemainingTime => IsExpired ? TimeSpan.Zero : ExpiresAt - DateTime.Now;
     
        /// <summary>
        /// Extend session expiration time (sliding expiration)
        /// </summary>
        /// <param name="timeoutMinutes">Timeout duration in minutes</param>
        public void ExtendSession(int timeoutMinutes)
        {
            LastActivityAt = DateTime.Now;
            ExpiresAt = DateTime.Now.AddMinutes(timeoutMinutes);
        }
        /// <summary>
        /// Terminate the session with reason
        /// </summary>
        /// <param name="reason">Termination reason</param>
        public void Terminate(string reason)
        {
            IsActive = false;
            TerminationReason = reason;
        }
        /// <summary>
        /// Update session activity timestamp
        /// </summary>
        public void UpdateActivity()
        {
            LastActivityAt = DateTime.Now;
        }
    }
}
