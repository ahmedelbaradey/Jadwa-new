using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Users;
using AutoMapper;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Identity;

namespace Application.Features.Identity.Users.Commands.UpdateUserProfile
{
    /// <summary>
    /// Handler for updating user profile information
    /// Implements Clean Architecture and CQRS patterns with file upload support
    /// </summary>
    public class UpdateUserProfileCommandHandler : BaseResponseHandler, ICommandHandler<UpdateUserProfileCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityServiceManager _identityService;
        // TODO: Add IFileUploadService when available
        #endregion

        #region Constructor
        public UpdateUserProfileCommandHandler(
            UserManager<User> userManager,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            IIdentityServiceManager identityService,
            ICurrentUserService currentUserService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }
        #endregion

        #region Handler
        public async Task<BaseResponse<string>> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Set user ID from current user context
                request.Id = _currentUserService.UserId.GetValueOrDefault();

                // Get existing user
                var user = await _identityService.UserManagmentService.FindByIdAsync(request.Id.ToString());
                if (user == null)
                {
                    return NotFound<string>(_localizer[SharedResourcesKey.UserNotFound]);
                }

                // Check email uniqueness if changed
                if (!string.Equals(user.Email, request.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var existingUserWithEmail = await  _identityService.UserManagmentService.FindByEmailAsync(request.Email);
                    if (existingUserWithEmail != null && existingUserWithEmail.Id != user.Id)
                    {
                        return BadRequest<string>(_localizer[SharedResourcesKey.ProfileDuplicateEmail]);
                    }
                }

                // Handle file uploads
                string? newCVPath = null;
                string? newPhotoPath = null;

                if (request.CVFile != null)
                {
                    // TODO: Implement file upload service integration
                    // For now, we'll store a placeholder path
                    newCVPath = $"/uploads/cv/{request.Id}_{DateTime.Now:yyyyMMddHHmmss}_{request.CVFile.FileName}";
                }

                if (request.PersonalPhoto != null)
                {
                    // TODO: Implement file upload service integration
                    // For now, we'll store a placeholder path
                    newPhotoPath = $"/uploads/photos/{request.Id}_{DateTime.Now:yyyyMMddHHmmss}_{request.PersonalPhoto.FileName}";
                }

                // Update user properties using AutoMapper
                _mapper.Map(request, user);

                // Set file paths if uploaded
                if (newCVPath != null)
                {
                    user.CVFilePath = newCVPath;
                }

                if (newPhotoPath != null)
                {
                    user.PersonalPhotoPath = newPhotoPath;
                }

                // Update last update date
                user.UpdatedAt = DateTime.Now;
                user.UpdatedBy = _currentUserService.UserId;
                // Save changes
                var result = await _userManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    return BadRequest<string>($"{_localizer[SharedResourcesKey.ProfileSystemErrorSavingData]}: {errors}");
                }

                return Success<string>(_localizer[SharedResourcesKey.ProfileUpdatedSuccessfully]);
            }
            catch (Exception ex)
            {
                return ServerError<string>(_localizer[SharedResourcesKey.ProfileSystemErrorSavingData]);
            }
        }
        #endregion
    }
}
