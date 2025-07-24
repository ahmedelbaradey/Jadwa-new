using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Users;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;

namespace Application.Features.Identity.Users.Commands.ChangePassword
{
    /// <summary>
    /// Handler for changing user password
    /// Enhanced for Sprint 3 with registration completion logic
    /// </summary>
    public class SetNewPasswordCommandHandler : BaseResponseHandler, ICommandHandler<ChangePasswordCommand, BaseResponse<string>>
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
        public async Task<BaseResponse<string>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Set user ID from current user context if not provided

                // Get user
                var user = await _identityServiceManager.UserManagmentService.FindByIdAsync(_currentUserService.UserId.ToString());
                if (user == null)
                    return NotFound<string>(_localizer[SharedResourcesKey.UserNotFound]);
                // Regular password change - requires current password
                if (string.IsNullOrEmpty(request.CurrentPassword))
                {
                    return BadRequest<string>(_localizer[SharedResourcesKey.PasswordIncorrectCurrent]);
                }
                var result = await _identityServiceManager.AuthenticationService.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);


                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest<string>($"{_localizer[SharedResourcesKey.PasswordChangeSystemError]}: {errors}");
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
