using Domain.Helpers;
using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Authentications.Commands.SignIn
{
    public record SignInCommand : ICommand<BaseResponse<JwtAuthResponse>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public record SessionExtractionInfo
    {
        public string JwtId { get; set; } = null!;
        public string SessionId { get; set; } = null!;
    }
}
