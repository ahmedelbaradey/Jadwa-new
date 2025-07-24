using Application.Features.Identity.Authorizations.Queries.Responses;
using Application.Base.Abstracts;
using Abstraction.Base.Response;


namespace Application.Features.Identity.Authorizations.Queries.GetRoleList
{
    public record GetRoleListQuery : IQuery<BaseResponse<List<GetRoleListResponse>>>
    {

    }
}
