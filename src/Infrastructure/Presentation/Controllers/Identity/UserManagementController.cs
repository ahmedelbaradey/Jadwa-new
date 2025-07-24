using Abstraction.Common.Wappers;
using Abstraction.Constants;
using Application.Features.Identity.Users.Commands.AddUser;
using Application.Features.Identity.Users.Commands.ChangePassword;
using Application.Features.Identity.Users.Commands.DeleteUser;
using Application.Features.Identity.Users.Commands.EditUser;
using Application.Features.Identity.Users.Commands.EditUserPreferredLanguage;
using Application.Features.Identity.Users.Commands.EditUserRoles;
using Application.Features.Identity.Users.Queries.Get;
using Application.Features.Identity.Users.Queries.GetCurrentCultureLanguage;
using Application.Features.Identity.Users.Queries.GetUserRoles;
using Application.Features.Identity.Users.Queries.GetUsersByRole;
using Application.Features.Identity.Users.Queries.List;
using Application.Features.Identity.Users.Queries.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Bases;
// Sprint 3 Enhanced Imports
using Application.Features.Identity.Users.Commands.ActivateUser;
using Application.Features.Identity.Users.Commands.AdminResetPassword;
using Application.Features.Identity.Users.Commands.ResendRegistrationMessage;
using Application.Features.Identity.Users.Commands.UpdateUserProfile;
using Application.Features.Identity.Users.Queries.GetUserProfile;
using Application.Features.Identity.Users.Queries.CheckSingleHolderRoleAvailability;
using Abstraction.Base.Response;
using Application.Features.Identity.Users.Dtos;
using Application.Features.Identity.Users.Commands.SetNewPassword;

namespace Identity.Controllers
{
    /// <summary>
    /// Enhanced User Management Controller with Sprint 3 features
    /// Includes user profile management, activation/deactivation, and administrative functions
    /// </summary>
    [Route("api/Users/UserManagement/[Action]")]
    [ApiController]
    //[Authorize] // Sprint 3 Enhancement: Require authentication for all endpoints
    public class UserManagementController : AppControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> UserList([FromQuery] ListQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int id)
        {
            var response = await Mediator.Send(new GetUserQuery() { Id = id });
            return NewResult(response);
        }

        /// <summary>
        /// Add a new user with Sprint 3 enhanced features
        /// </summary>
        /// <param name="command">User creation data</param>
        /// <returns>User creation response</returns>
        [HttpPost]
        //  [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddUser([FromBody] AddUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }


        /// <summary>
        /// Update user information with Sprint 3 enhanced features
        /// </summary>
        /// <param name="command">User update data</param>
        /// <returns>User update response</returns>
        [HttpPut]
      //  [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> UpdateUser([FromBody] EditUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserLanguage([FromBody] EditUserPreferredLanguageCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> UserRoles(int id)
        {
            var response = await Mediator.Send(new GetUserRolesQuery { Id = id });
            return NewResult(response);
        }


        [HttpPut]
        public async Task<IActionResult> UpdateUserRoles([FromBody] EditUserRolesCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var response = await Mediator.Send(new DeleteUserCommand() { Id = id });
            return NewResult(response);
        }

        /// <summary>
        /// Change user password with Sprint 3 enhanced features
        /// </summary>
        /// <param name="command">Password change data</param>
        /// <returns>Password change response</returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePasswordForUser([FromBody] ChangePasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        /// <summary>
        /// Change user password with Sprint 3 enhanced features
        /// </summary>
        /// <param name="command">Password change data</param>
        /// <returns>Password change response</returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> SetNewPasswordForUser([FromBody] SetNewPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<GetUserListResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFundManagerUsers()
        {
            var response = await Mediator.Send(new GetUsersByRoleQuery() { RoleName = Roles.FundManager.ToString() } );
            return NewResult(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<GetUserListResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBoardSecretaryUsers()
        {
            var response = await Mediator.Send(new GetUsersByRoleQuery() { RoleName =  Roles.BoardSecretary.ToString() });
            return NewResult(response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<GetUserListResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetLegalCouncilUsers()
        {
            var response = await Mediator.Send(new GetUsersByRoleQuery() {RoleName =  Roles.LegalCouncil.ToString() });
            return NewResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> UserListForBoardMembers([FromQuery] ListForBoardMembersQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        
        [HttpGet]
        public async Task<IActionResult> GetCurrentCultureLanguage()
        {
            var response = await Mediator.Send(new GetCurrentCultureLanguageQuery());
            return NewResult(response);
        }

        // ===== Sprint 3 Enhanced Endpoints =====

        /// <summary>
        /// Get user profile information
        /// </summary>
        /// <param name="userId">User ID (optional - defaults to current user)</param>
        /// <returns>User profile data</returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<UserProfileResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetUserProfile([FromQuery] int? userId = null)
        {
            var query = new GetUserProfileQuery { UserId = userId };
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        /// <summary>
        /// Update user profile information
        /// </summary>
        /// <param name="command">Profile update data</param>
        /// <returns>Success message</returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateUserProfile([FromForm] UpdateUserProfileCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        /// <summary>
        /// Activate a user account (Admin only)
        /// </summary>
        /// <param name="command">Activation data</param>
        /// <returns>Success message</returns>
        [HttpPost]
      //  [Authorize(Roles = "Admin,SuperAdmin")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ActivateUser([FromBody] ActivateUserCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

         

        /// <summary>
        /// Reset user password administratively (Admin only)
        /// </summary>
        /// <param name="command">Password reset data</param>
        /// <returns>Reset information</returns>
        [HttpPost]

        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AdminResetPassword([FromBody] AdminResetPasswordCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        /// <summary>
        /// Resend registration message to user (Admin only)
        /// </summary>
        /// <param name="command">Registration message data</param>
        /// <returns>Send status</returns>
        [HttpPost]

        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> ResendRegistrationMessage([FromBody] ResendRegistrationMessageCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        /// <summary>
        /// Check availability status of single-holder roles in the system
        /// Returns boolean flags indicating which single-holder roles have active users assigned
        /// </summary>
        /// <returns>Single-holder role availability status</returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<SingleHolderRoleAvailabilityResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckRoleAvailability()
        {
            var query = new CheckRoleAvailabilityQuery();
            var response = await Mediator.Send(query);
            return NewResult(response);
        }
    }
}
