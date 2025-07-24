using Abstraction.Base.Response;
using Abstraction.Constants;
using Abstraction.Contract.Service;
using Abstraction.Contract.Service.Sessions;
using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Domain.Entities.Users;
using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Resources;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Application.Features.Identity.Authentications.Commands.SignOut
{
    /// <summary>
    /// Handler for user logout
    /// Enhanced for Sprint 3 with proper session termination and audit logging
    /// </summary>
    public class SignOutCommandHandler : BaseResponseHandler, ICommandHandler<SignOutCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly IIdentityServiceManager _identityServiceManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ISessionManagementService _sessionManagementService;
        private readonly ILoggerManager _logger;
        // TODO: Add IAuditLogService when available
        // TODO: Add ITokenBlacklistService when available
        #endregion

        #region Constructors
        public SignOutCommandHandler(
            IIdentityServiceManager identityServiceManager,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer,
            ISessionManagementService sessionManagementService,
            ILoggerManager logger)
        {
            _identityServiceManager = identityServiceManager;
            _currentUserService = currentUserService;
            _localizer = localizer;
            _sessionManagementService = sessionManagementService;
            _logger = logger;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<string>> Handle(SignOutCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = _currentUserService.UserId.GetValueOrDefault();
                if (userId == 0)
                {
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                var user = await _identityServiceManager.UserManagmentService.FindByIdAsync(userId.ToString());
                if (user == null)
                {
                    return NotFound<string>(_localizer[SharedResourcesKey.UserNotFound]);
                }

                // Sprint 3 Enhancement: Comprehensive session termination

                // 1. Terminate user sessions based on request
                await TerminateUserSessionsAsync(userId, request);

                // 2. Remove FCM tokens (existing functionality)
                var userClaims = await _identityServiceManager.UserManagmentService.GetClaimsAsync(user);
                var fcmClaims = userClaims.Where(c => c.Type == CustomClaimTypes.FCMWebToken).ToList();
                if (fcmClaims.Any())
                {
                    await _identityServiceManager.UserManagmentService.RemoveClaimsAsync(user, fcmClaims);
                }

                // 3. TODO: Add access token to blacklist (when token blacklist service is available)
                // if (!string.IsNullOrEmpty(request.AccessToken))
                // {
                //     await _tokenBlacklistService.BlacklistTokenAsync(request.AccessToken);
                // }

                // 4. TODO: Invalidate refresh token (when refresh token service is available)
                // if (!string.IsNullOrEmpty(request.RefreshToken))
                // {
                //     await _refreshTokenService.InvalidateTokenAsync(request.RefreshToken);
                // }

                // 5. Update user's security stamp to invalidate existing tokens
                await _identityServiceManager.UserManagmentService.UpdateSecurityStampAsync(user);

                // 6. Sprint 3 Enhancement: Audit logging
                // TODO: Add audit log entry when audit service is available
                // await _auditLogService.LogUserActionAsync(userId, "User Logout",
                //     $"User {user.UserName} logged out at {DateTime.Now}", "Logout");

                return Success<string>(_localizer[SharedResourcesKey.LogoutSuccessful]);
            }
            catch (Exception ex)
            {
                return ServerError<string>(_localizer[SharedResourcesKey.LogoutSystemError]);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Terminate user sessions based on logout request parameters
        /// </summary>
        /// <param name="userId">User ID</param>
        /// <param name="request">Sign out command with session termination options</param>
        /// <returns>Task</returns>
        private async Task TerminateUserSessionsAsync(int userId, SignOutCommand request)
        {
            try
            {

                // Terminate specific session
                var sessionId = GetCurrentSessionId();
                if (!string.IsNullOrEmpty(sessionId))
                {
                    var terminated = await _sessionManagementService.TerminateSessionAsync(sessionId, SessionTerminationReason.UserLogout);
                    if (terminated)
                    {
                        _logger.LogInfo($"Terminated session {sessionId} for user {userId} during logout");
                    }
                    else
                    {
                        _logger.LogWarn($"Failed to terminate session {sessionId} for user {userId} - session may not exist");
                    }
                }
                else
                {
                    _logger.LogWarn($"No session ID provided or found in JWT token for user {userId} logout");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error terminating sessions for user {userId} during logout");
                // Don't throw - logout should succeed even if session termination fails
            }
        }

        /// <summary>
        /// Extract current session ID from JWT token claims
        /// </summary>
        /// <returns>Session ID if found, null otherwise</returns>
        private string? GetCurrentSessionId()
        {
            try
            {
                // Try to get session ID from JWT token claims
                var jwtIdClaim = _currentUserService.GetClaimValue(JwtRegisteredClaimNames.Jti);
                return jwtIdClaim;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not extract session ID from current user context");
                return null;
            }
        }

        #endregion

    }
}
