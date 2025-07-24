using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Identity.Authorizations.Queries.Responses;

namespace Application.Features.Identity.Authorizations.Commands.EditRole
{
    public record EditRoleCommand : GetRoleByIdResponse, ICommand<BaseResponse<string>>
    {

    }
}
