using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Users.Commands.AdminResetPassword
{
    /// <summary>
    /// Command for administrative password reset
    /// Sprint 3 implementation for admin-initiated password resets
    /// </summary>
    public record AdminResetPasswordCommand : ICommand<BaseResponse<string>>
    {
        /// <summary>
        /// User ID whose password should be reset
        /// </summary>
        public int UserId { get; set; }

    }

   
}
