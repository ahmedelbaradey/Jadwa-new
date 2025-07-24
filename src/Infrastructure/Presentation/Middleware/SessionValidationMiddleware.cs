using Abstraction.Contract.Service.Sessions;
using Abstraction.Contracts.Logger;
using Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;

namespace Presentation.Middleware
{
    /// <summary>
    /// Middleware for validating user sessions and implementing 30-minute timeout with sliding expiration
    /// Runs after authentication to validate session activity and update timestamps
    /// </summary>
    public class SessionValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerManager _logger;

        // Endpoints that should be excluded from session validation
        private readonly HashSet<string> _excludedPaths = new(StringComparer.OrdinalIgnoreCase)
        {
            "/api/Users/Authentication/Sign-In",
            "/api/Users/Authentication/Sign-Out",
            "/api/Users/Authentication/Refresh-Token",
            "/api/Users/Authentication/Is-Valid_Token",
            "/api/Users/Session/Extend-Session",
            "/api/Users/Session/Session-Status",
            "/api/Users/Session/Validate-Session",
            "/api/Users/Session/Timeout-Config",
            "/swagger",
            "/health",
            "/favicon.ico"
        };

        public SessionValidationMiddleware(
            RequestDelegate next,
            ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Skip session validation for excluded paths
                if (ShouldSkipValidation(context))
                {
                    await _next(context);
                    return;
                }

                // Skip if user is not authenticated
                if (!context.User.Identity?.IsAuthenticated == true)
                {
                    await _next(context);
                    return;
                }

                // Get session management service
                var sessionService = context.RequestServices.GetService<ISessionManagementService>();
                if (sessionService == null)
                {
                    
                    _logger.LogWarn("SessionManagementService not available, skipping session validation");
                    await _next(context);
                    return;
                }

                // Extract session information from JWT token
                var sessionInfo = ExtractSessionInfo(context);
                if (sessionInfo == null)
                {
                    _logger.LogWarn("No session information found in JWT token");
                    await _next(context);
                    return;
                }

                // Validate session
                var validationResult = await ValidateSessionAsync(sessionService, sessionInfo, context);
                if (!validationResult.IsValid)
                {
                    await HandleInvalidSession(context, validationResult.FailureReason);
                    return;
                }

                // Add session info to context for controllers
                context.Items["SessionInfo"] = validationResult.SessionInfo;
                context.Items["SessionId"] = sessionInfo.SessionId;
                context.Items["WasSessionExtended"] = validationResult.WasExtended;

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in SessionValidationMiddleware");
                // Continue processing to avoid breaking the application
                await _next(context);
            }
        }

        private bool ShouldSkipValidation(HttpContext context)
        {
            var path = context.Request.Path.Value ?? string.Empty;
            
            // Skip for excluded paths
            if (_excludedPaths.Any(excluded => path.StartsWith(excluded, StringComparison.OrdinalIgnoreCase)))
                return true;

            // Skip for OPTIONS requests (CORS preflight)
            if (context.Request.Method.Equals("OPTIONS", StringComparison.OrdinalIgnoreCase))
                return true;

            // Skip for static files
            if (path.Contains(".") && !path.Contains("/api/"))
                return true;

            return false;
        }

        private SessionExtractionInfo? ExtractSessionInfo(HttpContext context)
        {
            try
            {
                var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
                    return null;

                var token = authHeader.Substring("Bearer ".Length).Trim();
                var jwtHandler = new JwtSecurityTokenHandler();
                
                if (!jwtHandler.CanReadToken(token))
                    return null;

                var jwtToken = jwtHandler.ReadJwtToken(token);
                
                // Extract user ID and session ID from claims
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type  == nameof(UserClaimsModel.Id));
                var sessionIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "SessionId");
                var jwtIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out var userId))
                    return null;

                return new SessionExtractionInfo
                {
                    UserId = userId,
                    SessionId = sessionIdClaim?.Value,
                    JwtId = jwtIdClaim?.Value,
                    Token = token
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting session info from JWT token");
                return null;
            }
        }

        private async Task<SessionValidationResponse> ValidateSessionAsync(
            ISessionManagementService sessionService, 
            SessionExtractionInfo sessionInfo, 
            HttpContext context)
        {
            try
            {
                // If no session ID in token, create a new session for backward compatibility
                if (string.IsNullOrEmpty(sessionInfo.SessionId))
                {
                    var newSession = await sessionService.CreateSessionAsync(
                        sessionInfo.UserId,
                        sessionInfo.JwtId ?? Guid.NewGuid().ToString() 
                    );

                    sessionInfo.SessionId = newSession.SessionId;
                    context.Items["NewSessionCreated"] = true;
                }

                // Validate session with activity update
                return await sessionService.ValidateSessionAsync(sessionInfo.SessionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error validating session {sessionInfo.SessionId}");
                return new SessionValidationResponse
                {
                    IsValid = false,
                    FailureReason = "Session validation error"
                };
            }
        }

        private async Task HandleInvalidSession(HttpContext context, string? reason)
        {
            _logger.LogWarn($"Invalid session detected: {reason}");

            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = "Session expired or invalid",
                errorCode = "SESSION_EXPIRED",
                reason = reason,
                timestamp = DateTime.Now,
                redirectToLogin = true
            };

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }

        private class SessionExtractionInfo
        {
            public int UserId { get; set; }
            public string? SessionId { get; set; }
            public string? JwtId { get; set; }
            public string Token { get; set; } = null!;
        }
    }
}
