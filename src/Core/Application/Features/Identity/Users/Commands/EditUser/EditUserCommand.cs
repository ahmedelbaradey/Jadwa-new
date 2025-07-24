using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Microsoft.AspNetCore.Http;
using Application.Features.Identity.Users.Dtos;

namespace Application.Features.Identity.Users.Commands.EditUser
{
    /// <summary>
    /// Enhanced command for editing users with Sprint 3 administrative features
    /// Includes role management and advanced user properties
    /// </summary>
    public record EditUserCommand : BaseUserDto, ICommand<BaseResponse<string>>
    {
        /// <summary>
        /// CV file upload (PDF/DOCX, max 10MB)
        /// </summary>
        public string? CVFile { get; set; }

        /// <summary>
        /// Personal photo upload (JPG/PNG, max 2MB)
        /// </summary>
        public string? PersonalPhoto { get; set; }

        // Administrative Features
        /// <summary>
        /// Roles to assign to the user (replaces existing roles)
        /// </summary>
        public List<string> Roles { get; set; } = new();

    }
}
