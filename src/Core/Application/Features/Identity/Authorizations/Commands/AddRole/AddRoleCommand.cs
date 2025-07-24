using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Domain.Helpers;

namespace Application.Features.Identity.Authorizations.Commands.AddRole
{
    public record AddRoleCommand : ICommand<BaseResponse<string>>
    {
        public string roleName { get; set; } = null!;
        public List<RoleClaims> RoleClaims { get; set; } = null!;
    }
}
