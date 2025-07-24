using Domain.Helpers;
using Microsoft.AspNetCore.Identity;
using Application.Base.Abstracts;
using Domain.Entities.Users;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;

namespace Application.Features.Identity.Authentications.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : BaseResponseHandler, ICommandHandler<RefreshTokenCommand, BaseResponse<JwtAuthResponse>>
    {
        #region Fileds
        private readonly IIdentityServiceManager _service;
        #endregion

        #region Constructors
        public RefreshTokenCommandHandler(UserManager<User> userManager, IIdentityServiceManager service)
        {
            _service = service;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<JwtAuthResponse>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var jwtToken = _service.AuthenticationService.ReadJwtToken(request.AccessToken);
                var userIdAndExpiryDate = await _service.AuthenticationService.ValidateDetails(jwtToken, request.AccessToken, request.RefreshToken);

                switch (userIdAndExpiryDate)
                {
                    case ("AlgorithmIsWrong", null):
                        return ServerError<JwtAuthResponse>("Algorithm is wrong.");

                    case ("TokenIsRunning", null):
                        return ServerError<JwtAuthResponse>("Toekn is not expired.");

                    case ("RefreshTokenNotFound", null):
                        return ServerError<JwtAuthResponse>("Refresh token not found.");

                    case ("RefreshTokenIsExpired", null):
                        return ServerError<JwtAuthResponse>("Refresh token is expired.");
                }

                var (userId, ExpiryDate) = userIdAndExpiryDate;

                var user = await _service.UserManagmentService.FindByIdAsync(userId);
                if (user == null)
                    return NotFound<JwtAuthResponse>("User not found!");

                var result = await _service.AuthenticationService.GetRefreshToken(user, jwtToken, ExpiryDate, request.RefreshToken);
                return Success(result);
            }
            catch (Exception ex)
            {
                return ServerError<JwtAuthResponse>(ex.Message);
            }
        }
        #endregion
    }
}
