using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Constants;
using Abstraction.Contracts.Logger;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contracts.Identity;
using Application.Features.Identity.Users.Queries.Responses;

namespace Application.Features.Identity.Users.Queries.CheckSingleHolderRoleAvailability
{
    /// <summary>
    /// Handler for checking the availability status of single-holder roles in the system
    /// Provides real-time information about which single-holder roles are currently assigned
    /// Supports frontend workflows for role assignment and administrative decision-making
    /// </summary>
    public class CheckRoleAvailabilityQueryHandler : BaseResponseHandler, IQueryHandler<CheckRoleAvailabilityQuery, BaseResponse<SingleHolderRoleAvailabilityResponse>>
    {
        #region Fields
        private readonly IIdentityServiceManager _identityServiceManager;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructor
        public CheckRoleAvailabilityQueryHandler(
            IIdentityServiceManager identityServiceManager,
            IStringLocalizer<SharedResources> localizer,
            ILoggerManager logger)
        {
            _identityServiceManager = identityServiceManager;
            _localizer = localizer;
            _logger = logger;
        }
        #endregion

        #region Handler
        public async Task<BaseResponse<SingleHolderRoleAvailabilityResponse>> Handle(CheckRoleAvailabilityQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var response = new SingleHolderRoleAvailabilityResponse();
                // Check each single-holder role for active user assignment
                response.LegalCouncilHasActiveUser = await HasActiveUserInRoleAsync(RoleHelper.LegalCouncil);
                response.FinanceControllerHasActiveUser = await HasActiveUserInRoleAsync(RoleHelper.FinanceController);
                response.ComplianceLegalManagingDirectorHasActiveUser = await HasActiveUserInRoleAsync(RoleHelper.ComplianceLegalManagingDirector);
                response.HeadOfRealEstateHasActiveUser = await HasActiveUserInRoleAsync(RoleHelper.HeadOfRealEstate);
                return Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking single-holder role availability");
                return ServerError<SingleHolderRoleAvailabilityResponse>(_localizer[SharedResourcesKey.SystemErrorRetrievingData]);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks if a specific role has an active user assigned
        /// </summary>
        /// <param name="roleName">Name of the role to check</param>
        /// <returns>True if there is an active user assigned to the role</returns>
        private async Task<bool> HasActiveUserInRoleAsync(string roleName)
        {
            try
            {
                // Get all users in the specified role
                var usersInRole = await _identityServiceManager.UserManagmentService.GetUsersByRole(roleName);
                
                // Check if any of the users are active
                var hasActiveUser = usersInRole.Any(user => user.IsActive);
               
                _logger.LogInfo($"Role '{roleName}' has active user: {hasActiveUser}");
                
                return hasActiveUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking active users for role '{roleName}'");
                return false; // Default to false on error
            }
        }

        #endregion
    }
}
