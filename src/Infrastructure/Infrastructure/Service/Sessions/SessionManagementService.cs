using Abstraction.Contract.Service.Sessions;
using Abstraction.Contracts.Logger;
using Domain.Entities.Sessions;
using Domain.Helpers;
using Domain.Enums;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Infrastructure.Service.Sessions
{
    /// <summary>
    /// Redis-based session management service implementation
    /// Provides comprehensive session tracking with 30-minute timeout, sliding expiration, and role-based timeouts
    /// </summary>
    public class SessionManagementService : ISessionManagementService
    {
        private readonly IDistributedCache _distributedCache;
        private readonly SessionSettings _sessionSettings;
        private readonly ILoggerManager _logger;

        private const string SESSION_KEY_PREFIX = "session:";
        private const string USER_SESSIONS_KEY_PREFIX = "user_sessions:";
        private const string ACTIVITY_KEY_PREFIX = "activity:";

        public SessionManagementService(
            IDistributedCache distributedCache,
            SessionSettings sessionSettings,
            ILoggerManager logger)
        {
            _distributedCache = distributedCache;
            _sessionSettings = sessionSettings;
            _logger = logger;
        }

        public async Task<UserSession> CreateSessionAsync(int userId, string jwtTokenId)
        {
            try
            {
                var sessionId = GenerateSessionId();
                var now = DateTime.Now;

                // Calculate timeout based on role and Remember Me
                var timeoutMinutes = _sessionSettings.TimeoutMinutes;

                var session = new UserSession
                {
                    SessionId = sessionId,
                    UserId = userId,
                    JwtTokenId = jwtTokenId,
                    CreatedAt = now,
                    LastActivityAt = now,
                    ExpiresAt = now.AddMinutes(timeoutMinutes),
                    IsActive = true
                   
                };

                // Store session in Redis
                await StoreSessionAsync(session);

                // Update user sessions list
                await AddToUserSessionsAsync(userId, sessionId);

                // Update activity tracking
                await UpdateActivityTrackingAsync(sessionId, userId);

                _logger.LogInfo($"Session created for user {userId}: {sessionId}");
                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating session for user {userId}");
                throw;
            }
        }

        public async Task<UserSession?> GetSessionAsync(string sessionId)
        {
            try
            {
                var key = GetSessionKey(sessionId);
                var sessionJson = await _distributedCache.GetStringAsync(key);

                if (string.IsNullOrEmpty(sessionJson))
                    return null;

                var session = JsonSerializer.Deserialize<UserSession>(sessionJson);
                return session;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving session {sessionId}");
                return null;
            }
        }

        public async Task<List<UserSession>> GetUserSessionsAsync(int userId)
        {
            try
            {
                var userSessionsKey = GetUserSessionsKey(userId);
                var sessionIdsJson = await _distributedCache.GetStringAsync(userSessionsKey);

                if (string.IsNullOrEmpty(sessionIdsJson))
                    return new List<UserSession>();

                var sessionIds = JsonSerializer.Deserialize<List<string>>(sessionIdsJson) ?? new List<string>();
                var sessions = new List<UserSession>();

                foreach (var sessionId in sessionIds)
                {
                    var session = await GetSessionAsync(sessionId);
                    if (session != null && session.IsActive && !session.IsExpired)
                    {
                        sessions.Add(session);
                    }
                }

                return sessions;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving sessions for user {userId}");
                return new List<UserSession>();
            }
        }

        public async Task<bool> ExtendSessionAsync(string sessionId)
        {
            try
            {
                var session = await GetSessionAsync(sessionId);
                if (session == null || !session.IsActive || session.IsExpired)
                    return false;

                // Calculate timeout based on role
                var timeoutMinutes = _sessionSettings.TimeoutMinutes;
                var previousExpiresAt = session.ExpiresAt;
                session.ExtendSession(timeoutMinutes);

                await StoreSessionAsync(session);
                await UpdateActivityTrackingAsync(sessionId, session.UserId);
                
                _logger.LogInfo($"Session extended: {sessionId} (Timeout: {timeoutMinutes}min)");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error extending session {sessionId}");
                return false;
            }
        }

        public async Task<SessionValidationResponse> ValidateSessionAsync(string sessionId, bool updateActivity = true)
        {
            try
            {
                var session = await GetSessionAsync(sessionId);

                if (session == null)
                {
                    return new SessionValidationResponse
                    {
                        IsValid = false,
                        FailureReason = "Session not found"
                    };
                }

                if (!session.IsActive)
                {
                    return new SessionValidationResponse
                    {
                        IsValid = false,
                        FailureReason = "Session is inactive"
                    };
                }

                if (session.IsExpired)
                {
                    await TerminateSessionAsync(sessionId, SessionTerminationReason.InactivityTimeout);
                    return new SessionValidationResponse
                    {
                        IsValid = false,
                        FailureReason = "Session expired"
                    };
                }

                var wasExtended = false;
                // Update activity if sliding expiration is enabled
                if (_sessionSettings.EnableSlidingExpiration && updateActivity)
                {
                    var timeoutMinutes = _sessionSettings.TimeoutMinutes;
                    session.LastActivityAt = DateTime.Now;
                    session.ExpiresAt = DateTime.Now.AddMinutes(timeoutMinutes);
                    await StoreSessionAsync(session);
                    await UpdateActivityTrackingAsync(sessionId, session.UserId);
                    wasExtended = true;
                }

                var sessionInfo = MapToSessionInfo(session);
                return new SessionValidationResponse
                {
                    IsValid = true,
                    SessionInfo = sessionInfo,
                    WasExtended = wasExtended
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error validating session {sessionId}");
                return new SessionValidationResponse
                {
                    IsValid = false,
                    FailureReason = "Validation error"
                };
            }
        }

        public async Task<bool> TerminateSessionAsync(string sessionId, SessionTerminationReason reason)
        {
            try
            {
                var session = await GetSessionAsync(sessionId);
                if (session == null)
                    return false;

                session.Terminate(reason.ToString());
                await StoreSessionAsync(session);

                // Remove from user sessions list
                await RemoveFromUserSessionsAsync(session.UserId, sessionId);

                // Remove activity tracking
                await RemoveActivityTrackingAsync(sessionId);

                _logger.LogInfo($"Session terminated: {sessionId}, Reason: {reason}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error terminating session {sessionId}");
                return false;
            }
        }

        public async Task<SessionInfo?> GetSessionInfoAsync(string sessionId)
        {
            try
            {
                var session = await GetSessionAsync(sessionId);
                return session != null ? MapToSessionInfo(session) : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting session info for {sessionId}");
                return null;
            }
        }

        public async Task<bool> UpdateSessionActivityAsync(string sessionId)
        {
            try
            {
                var session = await GetSessionAsync(sessionId);
                if (session == null || !session.IsActive)
                    return false;

                session.UpdateActivity();
                await StoreSessionAsync(session);
                await UpdateActivityTrackingAsync(sessionId, session.UserId);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating session activity for {sessionId}");
                return false;
            }
        }

        #region Private Methods
 

        private async Task StoreSessionAsync(UserSession session)
        {
            var key = GetSessionKey(session.SessionId);
            var sessionJson = JsonSerializer.Serialize(session);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = session.ExpiresAt
            };

            await _distributedCache.SetStringAsync(key, sessionJson, options);
        }

        private async Task AddToUserSessionsAsync(int userId, string sessionId)
        {
            var key = GetUserSessionsKey(userId);
            var existingSessionsJson = await _distributedCache.GetStringAsync(key);
            var sessionIds = string.IsNullOrEmpty(existingSessionsJson) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(existingSessionsJson) ?? new List<string>();

            if (!sessionIds.Contains(sessionId))
            {
                sessionIds.Add(sessionId);
                var updatedJson = JsonSerializer.Serialize(sessionIds);
                await _distributedCache.SetStringAsync(key, updatedJson);
            }
        }

        private async Task RemoveFromUserSessionsAsync(int userId, string sessionId)
        {
            var key = GetUserSessionsKey(userId);
            var existingSessionsJson = await _distributedCache.GetStringAsync(key);
            
            if (!string.IsNullOrEmpty(existingSessionsJson))
            {
                var sessionIds = JsonSerializer.Deserialize<List<string>>(existingSessionsJson) ?? new List<string>();
                sessionIds.Remove(sessionId);
                var updatedJson = JsonSerializer.Serialize(sessionIds);
                await _distributedCache.SetStringAsync(key, updatedJson);
            }
        }

        private async Task UpdateActivityTrackingAsync(string sessionId, int userId)
        {
            var key = GetActivityKey(sessionId);
            var activityData = new
            {
                SessionId = sessionId,
                UserId = userId,
                LastActivity = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var options = new DistributedCacheEntryOptions
            {
                SlidingExpiration = _sessionSettings.SessionTimeout
            };

            await _distributedCache.SetStringAsync(key, JsonSerializer.Serialize(activityData), options);
        }

        private async Task RemoveActivityTrackingAsync(string sessionId)
        {
            var key = GetActivityKey(sessionId);
            await _distributedCache.RemoveAsync(key);
        }

        private SessionInfo MapToSessionInfo(UserSession session)
        {
            var remainingSeconds = Math.Max(0, (int)session.RemainingTime.TotalSeconds);

            return new SessionInfo
            {
                SessionId = session.SessionId,
                IsActive = session.IsActive,
                IsExpired = session.IsExpired,
                ExpiresAt = session.ExpiresAt,
                RemainingSeconds = remainingSeconds,
                LastActivityAt = session.LastActivityAt,
                CreatedAt = session.CreatedAt,
            };
        }

        private string GenerateSessionId()
        {
            return Guid.NewGuid().ToString("N");
        }

        private string GetSessionKey(string sessionId)
        {
            return $"{SESSION_KEY_PREFIX}{sessionId}";
        }

        private string GetUserSessionsKey(int userId)
        {
            return $"{USER_SESSIONS_KEY_PREFIX}{userId}";
        }

        private string GetActivityKey(string sessionId)
        {
            return $"{ACTIVITY_KEY_PREFIX}{sessionId}";
        }

       

        #endregion
    }
}
