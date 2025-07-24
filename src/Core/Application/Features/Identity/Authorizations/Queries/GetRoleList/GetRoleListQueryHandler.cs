using AutoMapper;
using Application.Features.Identity.Authorizations.Queries.Responses;
using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Identity;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Constants;



namespace Application.Features.Identity.Authorizations.Queries.GetRoleList
{
    public class GetRoleListQueryHandler : BaseResponseHandler, IQueryHandler<GetRoleListQuery, BaseResponse<List<GetRoleListResponse>>>
    {
        #region Fileds
        private readonly IIdentityServiceManager _service;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructors
        public GetRoleListQueryHandler(IIdentityServiceManager service, IMapper mapper, ILoggerManager logger, IStringLocalizer<SharedResources> localizer)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
            _localizer = localizer;
        }
        #endregion

        #region Functions

        public async Task<BaseResponse<List<GetRoleListResponse>>> Handle(GetRoleListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var roles = await _service.AuthorizationService.GetRoleListAsync();
                var ExcludedRoles = new List<string> { RoleHelper.Admin , RoleHelper.SuperAdmin , RoleHelper.Basic };
                if (roles == null)
                {
                    return NotFound<List<GetRoleListResponse>>(_localizer[SharedResourcesKey.NotFoundRoles]);
                }

                var rolesMapper = _mapper.Map<List<GetRoleListResponse>>(roles);

                // excluded (admin,superadmin.basic) roles as per JDWA-1689 Issue
                rolesMapper = rolesMapper.Where(r => !ExcludedRoles.Contains(r.roleName.ToLower())).ToList();

                // Apply localization to role names
                foreach (var roleResponse in rolesMapper)
                {
                    // Keep original role name for internal use
                    var originalRoleName = roleResponse.roleName;

                    // Set localized display name
                    roleResponse.DisplayName = GetLocalizedRoleName(originalRoleName);
                }

                return Success(rolesMapper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return ServerError<List<GetRoleListResponse>>(ex.Message);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Get localized role name based on the role name from database
        /// Maps database role names to localized display names
        /// </summary>
        /// <param name="roleName">Role name from database (lowercase)</param>
        /// <returns>Localized role name</returns>
        private string GetLocalizedRoleName(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                return roleName;

            // Convert to lowercase for consistent comparison
            var roleNameLower = roleName.ToLower();

            return roleNameLower switch
            {
                "fundmanager" => _localizer[SharedResourcesKey.FundManager],
                "legalcouncil" => _localizer[SharedResourcesKey.LegalCouncil],
                "boardsecretary" => _localizer[SharedResourcesKey.BoardSecretary],
                "boardmember" => _localizer[SharedResourcesKey.BoardMember],
                "superadmin" => _localizer[SharedResourcesKey.SuperAdmin],
                "admin" => _localizer[SharedResourcesKey.Admin],
                "basic" => _localizer[SharedResourcesKey.Basic],
                "user" => _localizer[SharedResourcesKey.User],
                "financecontroller" => _localizer[SharedResourcesKey.FinanceController],
                "compliancelegalmanagingdirector" => _localizer[SharedResourcesKey.ComplianceLegalManagingDirector],
                "headofrealestate" => _localizer[SharedResourcesKey.HeadOfRealEstate],
                "associatedfundmanager" => _localizer[SharedResourcesKey.AssociateFundManager],
                _ => roleName // Return original name if no localization found
            };
        }

        #endregion
    }
}
