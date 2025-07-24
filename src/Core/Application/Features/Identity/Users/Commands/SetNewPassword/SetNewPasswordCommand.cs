using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Identity.Users.Commands.SetNewPassword
{
    /// <summary>
    /// Command for changing user password
    /// Enhanced for Sprint 3 with registration completion logic
    /// </summary>
    public record SetNewPasswordCommand : ICommand<BaseResponse<string>>
    {
        public int UserId { get; set; }
        /// <summary>
        /// New password
        /// </summary>
        public string NewPassword { get; set; } = null!;

        /// <summary>
        /// Confirm new password
        /// </summary>
        public string ConfirmPassword { get; set; } = null!;

    }

    
}
