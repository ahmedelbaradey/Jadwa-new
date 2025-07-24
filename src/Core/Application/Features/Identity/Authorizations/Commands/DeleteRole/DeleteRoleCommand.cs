using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Authorizations.Commands.DeleteRole
{
    public record DeleteRoleCommand : ICommand<BaseResponse<string>>
    {
        public int Id { get; set; }
    }
}
