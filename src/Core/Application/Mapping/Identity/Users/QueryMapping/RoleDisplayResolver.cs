using AutoMapper;
using Abstraction.Contracts.Identity;
using Domain.Entities.Users;
using Abstraction.Contracts.Logger;
using Application.Features.Identity.Users.Queries.Responses;
using Application.Common.Helpers;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Mapping.Users
{
    /// <summary>
    /// Custom AutoMapper value resolver for user primary role display
    /// Provides the primary role name for a user with proper filtering and error handling
    /// Note: This resolver is currently not used in favor of the EnhanceWithRoleInformation approach
    /// which provides better performance and error handling
    /// </summary>
    public class RoleDisplayResolver(IIdentityServiceManager identityServiceManager, IStringLocalizer<SharedResources> localizer) : IValueResolver<User, GetUserListResponse, string>
    {
        private readonly IIdentityServiceManager _identityServiceManager = identityServiceManager;
        private readonly IStringLocalizer<SharedResources> _localizer= localizer;
        public string Resolve(User source, GetUserListResponse destination, string destMember, ResolutionContext context)
        {
            try
            {
                // Note: Using async methods in AutoMapper resolvers is not recommended
                // This is why the EnhanceWithRoleInformation approach is preferred
                var user = _identityServiceManager.UserManagmentService.FindByIdAsync(source.Id.ToString()).GetAwaiter().GetResult();
                if (user != null)
                {
                    var roles = _identityServiceManager.AuthorizationService.GetUsersRoles(user).GetAwaiter().GetResult();
                    if (roles?.UserRoles != null)
                    {
                        // Filter to only roles where HasRole = true
                        return roles.UserRoles
                            .Where(r => r.HasRole)
                            .Select(r => r.Name)
                            .FirstOrDefault() ?? string.Empty;

                    }
                }
                return string.Empty;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// Custom AutoMapper value resolver for user roles list display
    /// Provides all role names for a user with proper filtering and error handling
    /// Note: This resolver is currently not used in favor of the EnhanceWithRoleInformation approach
    /// which provides better performance and error handling
    /// </summary>
    public class RolesDisplayResolver(IIdentityServiceManager identityServiceManager) : IValueResolver<User, GetUserListResponse, List<string>>
    {
        private readonly IIdentityServiceManager _identityServiceManager = identityServiceManager;

        public List<string> Resolve(User source, GetUserListResponse destination, List<string> destMember, ResolutionContext context)
        {
            try
            {
                // Note: Using async methods in AutoMapper resolvers is not recommended
                // This is why the EnhanceWithRoleInformation approach is preferred
                var user = _identityServiceManager.UserManagmentService.FindByIdAsync(source.Id.ToString()).GetAwaiter().GetResult();
                if (user != null)
                {
                    var roles = _identityServiceManager.AuthorizationService.GetUsersRoles(user).GetAwaiter().GetResult();
                    if (roles?.UserRoles != null)
                    {
                        // Filter to only roles where HasRole = true
                        return roles.UserRoles
                            .Where(r => r.HasRole)
                            .Select(r => r.Name)
                            .ToList();
                    }
                }
                return new List<string>();
            }
            catch (Exception ex)
            {
                return new List<string>();
            }
        }
    }

    /// <summary>
    /// Custom AutoMapper value resolver for localized user primary role display
    /// Provides the localized primary role name for a user based on user's preferred language
    /// </summary>
    public class LocalizedRoleDisplayResolver : IValueResolver<User, GetUserListResponse, string>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public LocalizedRoleDisplayResolver(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        public string Resolve(User source, GetUserListResponse destination, string destMember, ResolutionContext context)
        {
            try
            {
                // Get the first role name from the navigation property
                var primaryRoleName = source.Roles.ToList()?.FirstOrDefault()?.Name;

                if (string.IsNullOrEmpty(primaryRoleName))
                    return string.Empty;

                // Return localized role name
                return LocalizationHelper.GetUserRoleDisplay(primaryRoleName, _localizer);
            }
            catch (Exception)
            {
                // Return original role name if localization fails
                return source.Roles.ToList()?.FirstOrDefault()?.Name ?? string.Empty;
            }
        }
    }

    /// <summary>
    /// Custom AutoMapper value resolver for localized user roles list display
    /// Provides all localized role names for a user based on user's preferred language
    /// </summary>
    public class LocalizedRolesDisplayResolver : IValueResolver<User, GetUserListResponse, List<string>>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public LocalizedRolesDisplayResolver(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }

        public List<string> Resolve(User source, GetUserListResponse destination, List<string> destMember, ResolutionContext context)
        {
            try
            {
                // Get all role names from the navigation property
                var roleNames = source.Roles?.Select(r => r.Name).ToList();

                if (roleNames == null || !roleNames.Any())
                    return new List<string>();

                // Return localized role names
                return roleNames.Select(roleName => LocalizationHelper.GetUserRoleDisplay(roleName, _localizer)).ToList();
            }
            catch (Exception)
            {
                // Return original role names if localization fails
                return source.Roles?.Select(r => r.Name).ToList() ?? new List<string>();
            }
        }
    }
}
