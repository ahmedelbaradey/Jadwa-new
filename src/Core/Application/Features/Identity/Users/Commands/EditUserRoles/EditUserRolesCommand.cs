using Domain.Helpers;
using Application.Base;
using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Users.Commands.EditUserRoles
{
    public record EditUserRolesCommand : EditUserRolesRequest, ICommand<BaseResponse<string>>
    {

    }
}
