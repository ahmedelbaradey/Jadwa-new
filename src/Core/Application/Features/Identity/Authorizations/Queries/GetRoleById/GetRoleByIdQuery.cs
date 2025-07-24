using Application.Features.Identity.Authorizations.Queries.Responses;
using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Authorizations.Queries.GetRoleById
{
    public record GetRoleByIdQuery : IQuery<BaseResponse<GetRoleByIdResponse>>
    {
        public int Id { get; set; }
    }
}
