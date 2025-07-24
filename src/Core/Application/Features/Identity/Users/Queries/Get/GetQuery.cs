using Application.Features.Identity.Users.Queries.Responses;
using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Users.Queries.Get
{
    public record GetUserQuery : IQuery<BaseResponse<GetUserResponse>>
    {
        public int Id { get; set; }
    }
}
