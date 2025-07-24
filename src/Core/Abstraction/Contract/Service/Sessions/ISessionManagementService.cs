using Domain.Entities.Sessions;
using Domain.Helpers;
using Domain.Enums;

namespace Abstraction.Contract.Service.Sessions
{
    /// <summary>
    /// Interface for comprehensive session management operations
    /// Supports 30-minute timeout with sliding expiration, role-based timeouts, and Redis caching
    /// </summary>
    public interface ISessionManagementService
    {
        /// <summary>
        /// Create a new user session with role-based timeout
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="jwtTokenId">JWT token ID</param>
        /// <returns>Created session</returns>
        Task<UserSession> CreateSessionAsync(int userId, string jwtTokenId);

        /// <summary>
        /// Get session by session ID
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <returns>Session if found, null otherwise</returns>
        Task<UserSession?> GetSessionAsync(string sessionId);

        /// <summary>
        /// Get all active sessions for a user
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <returns>List of active sessions</returns>
        Task<List<UserSession>> GetUserSessionsAsync(int userId);

        /// <summary>
        /// Extend session expiration (sliding expiration)
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <returns>True if extended successfully</returns>
        Task<bool> ExtendSessionAsync(string sessionId);

        /// <summary>
        /// Validate session and update activity
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <param name="updateActivity">Whether to update activity timestamp</param>
        /// <returns>Session validation response</returns>
        Task<SessionValidationResponse> ValidateSessionAsync(string sessionId, bool updateActivity = true);

        /// <summary>
        /// Terminate a specific session
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <param name="reason">Termination reason</param>
        /// <returns>True if terminated successfully</returns>
        Task<bool> TerminateSessionAsync(string sessionId, SessionTerminationReason reason);

        /// <summary>
        /// Get session information for API responses
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <returns>Session information</returns>
        Task<SessionInfo?> GetSessionInfoAsync(string sessionId);

        /// <summary>
        /// Update session activity timestamp
        /// </summary>
        /// <param name="sessionId">Session ID</param>
        /// <returns>True if updated successfully</returns>
        Task<bool> UpdateSessionActivityAsync(string sessionId);
    }
}
