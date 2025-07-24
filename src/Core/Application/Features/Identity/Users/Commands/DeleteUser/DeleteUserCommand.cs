using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Users.Commands.DeleteUser
{
    public record DeleteUserCommand : ICommand<BaseResponse<string>>
    {
        public int Id { get; set; }
    }
}
