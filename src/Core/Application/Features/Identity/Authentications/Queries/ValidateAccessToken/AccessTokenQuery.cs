using Abstraction.Base.Response;
using Application.Base.Abstracts;
 

namespace Application.Features.Identity.Authentications.Queries.ValidateAccessToken
{
    public record AccessTokenQuery : IQuery<BaseResponse<string>>
    {
        public string Accesstoken { get; set; } = null!;
    }
}
