using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Users.Commands.ResendRegistrationMessage
{
    /// <summary>
    /// Command to resend registration message to a user
    /// Sprint 3 implementation for registration message management
    /// </summary>
    public record ResendRegistrationMessageCommand : ICommand<BaseResponse<string>>
    {
        /// <summary>
        /// User ID to resend registration message to
        /// </summary>
        public int UserId { get; set; }


    }
}
