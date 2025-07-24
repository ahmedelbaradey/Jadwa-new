using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Users.Commands.ActivateUser
{
    /// <summary>
    /// Command to activate a user account
    /// Sprint 3 implementation for user status management
    /// </summary>
    public record ActivateUserCommand : ICommand<BaseResponse<string>>
    {
        /// <summary>
        /// User ID to activate
        /// </summary>
        public int UserId { get; set; }

    }
}
