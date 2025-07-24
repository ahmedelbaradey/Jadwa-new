using Domain.Helpers;
using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Authentications.Commands.RefreshToken
{
    public record RefreshTokenCommand : ICommand<BaseResponse<JwtAuthResponse>>
    {
        public string AccessToken { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
    }
}
