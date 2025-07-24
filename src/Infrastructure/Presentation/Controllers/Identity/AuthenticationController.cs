using Abstraction.Base.Response;
using Application.Features.Identity.Authentications.Commands.RefreshToken;
using Application.Features.Identity.Authentications.Commands.SignIn;
using Application.Features.Identity.Authentications.Commands.SignOut;
using Application.Features.Identity.Authentications.Commands.UpdateFCMToken;
using Application.Features.Identity.Authentications.Queries.ValidateAccessToken;
using Domain.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Bases;
using static Application.Features.Identity.Authentications.Commands.SignOut.SignOutCommand;

namespace Identity.Controllers
{
    [Route("api/Users/[controller]")]
    [ApiController]
   
    public class AuthenticationController : AppControllerBase
    {

        [AllowAnonymous]
        [HttpPost("Sign-In")]
        [ProducesResponseType(typeof(BaseResponse<JwtAuthResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpPost("Update-FCMToken")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateFCMToken([FromBody] UpdateFCMTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
        [AllowAnonymous]
        [HttpPost("Validate-Token")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ValidateToken([FromBody] AccessTokenQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpPost("Refresh-Token")]
        [ProducesResponseType(typeof(BaseResponse<JwtAuthResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [AllowAnonymous]
        [HttpGet("Is-Valid_Token")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> IsValidToken([FromBody] AccessTokenQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [HttpPost("Sign-Out")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [Authorize] // Sprint 3 Enhancement: Require authentication for logout
        public async Task<IActionResult> SignOut([FromBody] SignOutCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
