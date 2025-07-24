using Domain.Helpers;
using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Authentications.Commands.UpdateFCMToken
{
    public record UpdateFCMTokenCommand : ICommand<BaseResponse<string>>
    {
        public string UserId { get; set; } = null!;
        public string FCMWebToken { get; set; } = null!;
    }
}
