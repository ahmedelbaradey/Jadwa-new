using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Microsoft.AspNetCore.Http;
using Application.Features.Identity.Users.Dtos;

namespace Application.Features.Identity.Users.Commands.AddUser
{
    /// <summary>
    /// Enhanced command for adding users with Sprint 3 administrative features
    /// Includes role assignment and registration message integration
    /// </summary>
    public record AddUserCommand : BaseUserDto, ICommand<BaseResponse<string>>
    {
        // Basic Information
      //  public string Password { get; set; }  
      //  public string ConfirmPassword { get; set; }  

        /// <summary>
        /// CV file upload (optional)
        /// </summary>
        public string? CVFile { get; set; }

        /// <summary>
        /// Personal photo upload (optional)
        /// </summary>
        public string? PersonalPhoto { get; set; }

        // Administrative Features
        /// <summary>
        /// Roles to assign to the user
        /// </summary>
         public List<string> Roles { get; set; } 

         
    }

}
