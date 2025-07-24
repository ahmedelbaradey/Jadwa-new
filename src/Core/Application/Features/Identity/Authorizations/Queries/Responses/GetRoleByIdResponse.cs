using Domain.Helpers;

namespace Application.Features.Identity.Authorizations.Queries.Responses
{
    public record GetRoleByIdResponse : GetRoleListResponse
    {
        public List<RoleClaims> RoleClaims { get; set; } = null!;
    }
}
