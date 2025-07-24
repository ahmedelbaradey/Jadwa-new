using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Microsoft.AspNetCore.Http;
using Application.Features.Identity.Users.Dtos;

namespace Application.Features.Identity.Users.Commands.UpdateUserProfile
{
    /// <summary>
    /// Command to update user profile information
    /// Supports file uploads for CV and personal photo
    /// </summary>
    public record UpdateUserProfileCommand : BaseUserDto, ICommand<BaseResponse<string>>
    {
        /// <summary>
        /// CV file upload (PDF/DOCX, max 10MB)
        /// </summary>
        public IFormFile? CVFile { get; set; }

        /// <summary>
        /// Personal photo upload (JPG/PNG, max 2MB)
        /// </summary>
        public IFormFile? PersonalPhoto { get; set; }
    }
}
