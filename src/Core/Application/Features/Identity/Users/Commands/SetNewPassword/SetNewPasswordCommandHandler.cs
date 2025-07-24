using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Users;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;

namespace Application.Features.Identity.Users.Commands.SetNewPassword
{
    /// <summary>
    /// Handler for changing user password
    /// Enhanced for Sprint 3 with registration completion logic
    /// </summary>
    public class SetNewPasswordCommandHandler : BaseResponseHandler, ICommandHandler<SetNewPasswordCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly IIdentityServiceManager _service;
        private readonly IMapper _mapper;
        private readonly IIdentityServiceManager _identityServiceManager;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public SetNewPasswordCommandHandler(
            IIdentityServiceManager service,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            IIdentityServiceManager identityServiceManager,
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _service = service;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _identityServiceManager = identityServiceManager;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<string>> Handle(SetNewPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Set user ID from current user context if not provided
               
                // Get user
                var user = await _identityServiceManager.UserManagmentService.FindByIdAsync(_currentUserService.UserId.ToString());
                if (user == null)
                    return NotFound<string>(_localizer[SharedResourcesKey.UserNotFound]);


                // Sprint 3 Enhancement: Handle first-time password change
                bool isFirstTimePasswordChange = !user.RegistrationIsCompleted;
                IdentityResult result;
                if (isFirstTimePasswordChange)
                {
                    // For first-time users or mandatory resets, we don't require current password
                    // Remove existing password and add new one
                    if (await _identityServiceManager.AuthenticationService.HasPasswordAsync(user))
                    {
                        await _identityServiceManager.AuthenticationService.RemovePasswordAsync(user);
                    }
                    result = await _identityServiceManager.AuthenticationService.AddPasswordAsync(user, request.NewPassword);
                    if (!result.Succeeded)
                    {
                        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                        return BadRequest<string>($"{_localizer[SharedResourcesKey.PasswordChangeSystemError]}: {errors}");
                    }
                }
                else
                {
                    return ServerError<string>(_localizer[SharedResourcesKey.PasswordChangeSystemError]);
                }
                // Sprint 3 Enhancement: Mark registration as completed for first-time users
                if (isFirstTimePasswordChange)
                {
                    user.RegistrationIsCompleted = true;
                    user.UpdatedAt = DateTime.Now;
                    user.UpdatedBy = _currentUserService.UserId;
                    await _identityServiceManager.UserManagmentService.UpdateAsync(user);
                }
                return Success<string>(_localizer[SharedResourcesKey.PasswordChangedSuccessfully]);
            }
            catch (Exception ex)
            {
                return ServerError<string>(_localizer[SharedResourcesKey.PasswordChangeSystemError]);
            }
        }
        #endregion
    }
}
