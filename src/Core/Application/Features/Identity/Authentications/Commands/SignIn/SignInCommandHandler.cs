using Domain.Helpers;
using Application.Base.Abstracts;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Users;
using Abstraction.Base.Response;
using Abstraction.Contract.Service.Sessions;
using Abstraction.Contracts.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using Resources;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace Application.Features.Identity.Authentications.Commands.SignIn
{
    public class SignInCommandHandler : BaseResponseHandler, ICommandHandler<SignInCommand, BaseResponse<JwtAuthResponse>>
    {
        #region Fields
        private readonly SignInManager<User> _signInManager;
        private readonly IIdentityServiceManager _service;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ISessionManagementService _sessionManagementService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion

        #region Constructors
        public SignInCommandHandler(
            SignInManager<User> signInManager,
            IIdentityServiceManager service,
            IStringLocalizer<SharedResources> localizer,
            ISessionManagementService sessionManagementService,
            IHttpContextAccessor httpContextAccessor)
        {
            _signInManager = signInManager;
            _service = service;
            _localizer = localizer;
            _sessionManagementService = sessionManagementService;
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<JwtAuthResponse>> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Find user by username
                var user = await _service.UserManagmentService.FindByNameAsync(request.UserName);
                if (user == null)
                    return NotFound<JwtAuthResponse>(_localizer[SharedResourcesKey.LoginUserNotFound]);

                // Check if account is deactivated (locked out)
                if (!user.IsActive)
                {
                    return BadRequest<JwtAuthResponse>(_localizer[SharedResourcesKey.LoginAccountDeactivated]);
                }
                // Attempt sign in
                var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
                if (!signInResult.Succeeded)
                {
                    return BadRequest<JwtAuthResponse>(_localizer[SharedResourcesKey.LoginIncorrectPassword]);
                }
                // Generate JWT token
                var accessToken = await _service.AuthenticationService.GetJwtToken(user);
                accessToken.IsFirstLogin = false;
                accessToken.UserId = user.Id;

                // Extract session information from JWT token
                var sessionInfo = ExtractSessionInfoFromToken(accessToken.AccessToken);

                // Create session if session management is available
                if (sessionInfo != null)
                {
                    try
                    {
                        var session = await _sessionManagementService.CreateSessionAsync(
                            user.Id,
                            sessionInfo.JwtId
                        );

                        // Add session ID to response
                        accessToken.SessionId = session.SessionId;
                    }
                    catch (Exception ex)
                    {
                        // Log error but don't fail login if session creation fails
                        // Session will be created by middleware on first API call
                    }
                }

                // Check if user needs to complete registration (first-time login)
                if (!user.RegistrationIsCompleted)
                {
                    accessToken.IsFirstLogin = true; // Frontend should handle this
                }

                return Success(accessToken);
            }
            catch (Exception ex)
            {
                return ServerError<JwtAuthResponse>(ex.Message);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Extract session information from JWT token
        /// </summary>
        /// <param name="accessToken">JWT access token</param>
        /// <returns>Session information if found</returns>
        private SessionExtractionInfo? ExtractSessionInfoFromToken(string? accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
                return null;

            try
            {
                var jwtHandler = new JwtSecurityTokenHandler();
                if (!jwtHandler.CanReadToken(accessToken))
                    return null;

                var jwtToken = jwtHandler.ReadJwtToken(accessToken);

                var jwtIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);
                var sessionIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "SessionId");

                if (jwtIdClaim == null || sessionIdClaim == null)
                    return null;

                return new SessionExtractionInfo
                {
                    JwtId = jwtIdClaim.Value,
                    SessionId = sessionIdClaim.Value
                };
            }
            catch
            {
                return null;
            }
        }
 
        #endregion
 

    }
}
