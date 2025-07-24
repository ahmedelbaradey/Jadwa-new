using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Domain.Helpers;

namespace Application.Features.Identity.Users.Queries.GetUserRoles
{
    public record GetUserRolesQuery : IQuery<BaseResponse<ManageUserRolesResponse>>
    {
        public int Id { get; set; }
    }
}
