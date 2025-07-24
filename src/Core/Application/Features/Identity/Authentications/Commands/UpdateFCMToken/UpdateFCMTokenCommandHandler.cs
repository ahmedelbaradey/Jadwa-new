using Domain.Helpers;
using Application.Base.Abstracts;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Users;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;
using Abstraction.Constants;
using System.Security.Claims;
namespace Application.Features.Identity.Authentications.Commands.UpdateFCMToken
{
    public class UpdateFCMTokenCommandHandler : BaseResponseHandler, ICommandHandler<UpdateFCMTokenCommand, BaseResponse<string>>
    {
        #region Fileds
        private readonly UserManager<User> _userManager;
        private readonly IIdentityServiceManager _service;
        #endregion

        #region Constructors
        public UpdateFCMTokenCommandHandler(UserManager<User> userManager, IIdentityServiceManager service)
        {
            _userManager = userManager;
            _service = service;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<string>> Handle(UpdateFCMTokenCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);
                if (user == null)
                    return NotFound<string>("User with this username not found!");
                var accessToken = await _service.AuthenticationService.GetJwtToken(user);
                var userClaims = await _userManager.GetClaimsAsync(user);
                var newFCMTokenClaim = new Claim(CustomClaimTypes.FCMWebToken, request.FCMWebToken);
                if (userClaims.Where(c => c.Type == CustomClaimTypes.FCMWebToken).Any())
                {
                    if (userClaims.Contains(newFCMTokenClaim))
                    {
                        //do nothing
                    }
                    else
                    {
                        await _userManager.ReplaceClaimAsync(user, userClaims.Where(c => c.Type == CustomClaimTypes.FCMWebToken).SingleOrDefault(), newFCMTokenClaim);
                    }
                }
                else
                {
                    await _userManager.AddClaimAsync(user, newFCMTokenClaim);
                }

                return Success("Token Updated");
            }
            catch (Exception ex)
            {
                return ServerError<string>(ex.Message);
            }
        }

        #endregion

    }
}
