using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Domain.Entities.Users;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;

using Application.Features.Identity.Users.Dtos;
using Abstraction.Constants;
using Abstraction.Contracts.Service;
using Abstraction.Enums;
using Application.Common.Configurations;
using Microsoft.Extensions.Options;
using Abstraction.Contract.Service.Storage;

namespace Application.Features.Identity.Users.Queries.GetUserProfile
{
    /// <summary>
    /// Handler for getting user profile information
    /// Implements Clean Architecture and CQRS patterns
    /// </summary>
    public class GetUserProfileQueryHandler : BaseResponseHandler, IQueryHandler<GetUserProfileQuery, BaseResponse<UserProfileResponseDto>>
    {
        #region Fields
        private readonly IIdentityServiceManager _identityServiceManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IServiceManager _serviceManager;
        private readonly MinIOConfiguration _minioConfig;
        private readonly IPreviewUrlHelper _previewUrlHelper;
        #endregion

        #region Constructor
        public GetUserProfileQueryHandler(
            IMapper mapper,
            IIdentityServiceManager identityServiceManager ,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService,
            IServiceManager serviceManager,
            IOptions<MinIOConfiguration> minioConfig,
            IPreviewUrlHelper previewUrlHelper)
        {
            _identityServiceManager = identityServiceManager;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _serviceManager = serviceManager;
            _minioConfig = minioConfig.Value;
            _previewUrlHelper = previewUrlHelper;
        }
        #endregion

        #region Handler
        public async Task<BaseResponse<UserProfileResponseDto>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Determine which user profile to get
                var targetUserId = request.UserId ?? _currentUserService.UserId.GetValueOrDefault();
                var currentUserId = _currentUserService.UserId.GetValueOrDefault();

                // Authorization check - users can only access their own profile unless they're admin
                if (targetUserId != currentUserId)
                {
                    var currentUser = await _identityServiceManager.UserManagmentService.FindByIdAsync(currentUserId.ToString());
                    if (currentUser == null)
                    {
                        return Unauthorized<UserProfileResponseDto>(_localizer[SharedResourcesKey.UserNotFound]);
                    }

                    var currentUserRoles = await _identityServiceManager.UserManagmentService.GetUserRolesAsync(currentUser);
                    if (!currentUserRoles.Contains("admin") && !currentUserRoles.Contains("superadmin"))
                    {
                        return Unauthorized<UserProfileResponseDto>(_localizer[SharedResourcesKey.UnauthorizedUserAccess]);
                    }
                }

                // Get user by ID
                var user = await _identityServiceManager.UserManagmentService.FindByIdAsync(targetUserId.ToString());
                if (user == null)
                {
                    return NotFound<UserProfileResponseDto>(_localizer[SharedResourcesKey.UserNotFound]);
                }

                // Get user roles
                var userRoles = await _identityServiceManager.UserManagmentService.GetUserRolesAsync(user);

                // Map to response DTO
                var response = _mapper.Map<UserProfileResponseDto>(user);
                response.LegalCouncilHasActiveUser = await HasActiveUserInRoleAsync(RoleHelper.LegalCouncil, user.Id);
                response.FinanceControllerHasActiveUser = await HasActiveUserInRoleAsync(RoleHelper.FinanceController, user.Id);
                response.ComplianceLegalManagingDirectorHasActiveUser = await HasActiveUserInRoleAsync(RoleHelper.ComplianceLegalManagingDirector, user.Id);
                response.HeadOfRealEstateHasActiveUser = await HasActiveUserInRoleAsync(RoleHelper.HeadOfRealEstate, user.Id);
                response.Roles = userRoles.ToList();

                // Generate MinIO preview URLs for user files
                await PopulateUserFilePreviewUrls(response, user, cancellationToken);

                return Success(response);
            }
            catch (Exception ex)
            {
                return ServerError<UserProfileResponseDto>(_localizer[SharedResourcesKey.SystemErrorRetrievingData]);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks if a specific role has an active user assigned
        /// </summary>
        /// <param name="roleName">Name of the role to check</param>
        /// <returns>True if there is an active user assigned to the role</returns>
        private async Task<bool> HasActiveUserInRoleAsync(string roleName , int userId)
        {
            try
            {
                // Get all users in the specified role
                var usersInRole = await _identityServiceManager.UserManagmentService.GetUsersByRole(roleName);

                // Check if any of the users are active
                var hasActiveUser = usersInRole.Any(user => user.Id != userId &&  user.IsActive);


                return hasActiveUser;
            }
            catch (Exception ex)
            {
                return false; // Default to false on error
            }
        }

        /// <summary>
        /// Populates preview URLs for user files (CV and personal photo)
        /// </summary>
        private async Task PopulateUserFilePreviewUrls(UserProfileResponseDto response, User user, CancellationToken cancellationToken)
        {
            try
            {
                // Generate preview URL for CV file
                if (!string.IsNullOrEmpty(user.CVFilePath))
                {
                    response.CVFilePath = await _previewUrlHelper.GeneratePreviewUrlAsync(
                        user.CVFilePath,
                        (int)ModuleEnum.User,
                        cancellationToken);
                }

                // Generate preview URL for personal photo
                if (!string.IsNullOrEmpty(user.PersonalPhotoPath))
                {
                    response.PersonalPhotoPath = await _previewUrlHelper.GeneratePreviewUrlAsync(
                        user.PersonalPhotoPath,
                        (int)ModuleEnum.User,
                        cancellationToken);

                    // Also set the PreviewUrl property for backward compatibility
                    response.PreviewUrl = response.PersonalPhotoPath;
                }
            }
            catch (Exception ex)
            {
                // Log error but don't fail the entire request
                // Preview URLs will remain null if generation fails
            }
        }

        #endregion
    }
}
