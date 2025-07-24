using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Domain.Helpers;


namespace Application.Features.Identity.Authorizations.Queries.GetClaimList
{
    public record GetClaimListQuery : IQuery<BaseResponse<List<RoleClaims>>>
    {

    }
}
