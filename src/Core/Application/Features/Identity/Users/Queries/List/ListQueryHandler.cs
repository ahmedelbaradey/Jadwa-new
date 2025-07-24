using AutoMapper;
using Application.Features.Identity.Users.Queries.Responses;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Logger;
using Abstraction.Common.Wappers;
using Abstraction.Contracts.Identity;
using Domain.Entities.Users;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Application.Common.Helpers;



namespace Application.Features.Identity.Users.Queries.List
{

    /// <summary>
    /// Enhanced handler for user listing with advanced filtering capabilities
    /// Sprint 3 enhancement with role, status, and registration filtering
    /// </summary>
    public class ListQueryHandler : BaseResponseHandler, IQueryHandler<ListQuery, PaginatedResult<GetUserListResponse>>
    {
        #region Fields
        private readonly IIdentityServiceManager _service;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        private readonly IIdentityServiceManager _identityServiceManager;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public ListQueryHandler(
            IIdentityServiceManager service,
            IMapper mapper,
            ILoggerManager logger,
            IIdentityServiceManager identityServiceManager,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService)
        {
            _mapper = mapper;
            _service = service;
            _logger = logger;
            _identityServiceManager = identityServiceManager;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Functions
        public async Task<PaginatedResult<GetUserListResponse>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Use the optimized UsersWithRoles method to eagerly load roles
                var users = _service.UserManagmentService.UsersWithRoles().AsQueryable();

                int currentUserId = _currentUserService.UserId ?? 0;
                users = users.Where(u => u.Id != currentUserId && u.IsDeleted != true);

                // Sprint 3 Enhancement: Apply advanced filtering
                users = await ApplyFiltersAsync(users, request);
                if (!users.Any())
                {
                    return PaginatedResult<GetUserListResponse>.EmptyCollection(_localizer[SharedResourcesKey.NoRecords]);
                }

                if (!string.IsNullOrEmpty(request.OrderBy) && request.OrderBy.Contains("roles"))
                {
                    return await HandleRoleBasedSorting(users, request);
                    //request.OrderBy.Replace("roles", "PrimaryRole");
                }

                // Project to DTO and paginate - roles are now automatically mapped via navigation properties
                var paginatedList = await _mapper.ProjectTo<GetUserListResponse>(users).ToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy);

                // Apply localization to role names
                ApplyRoleLocalization(paginatedList.Data);

                _logger.LogInfo($"Successfully retrieved {paginatedList.Data.Count} users with localized roles using optimized query");

                return paginatedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Enhanced User List Query");
                return PaginatedResult<GetUserListResponse>.ServerError(_localizer[SharedResourcesKey.SystemErrorRetrievingData]);
            }
        }

        /// <summary>
        /// Apply advanced filtering based on Sprint 3 requirements
        /// </summary>
        private async Task<IQueryable<User>> ApplyFiltersAsync(IQueryable<User> users, ListQuery request)
        {
            // Basic search filter
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                users = users.Where(u =>u.UserName != null && u.UserName.Contains(request.Search));
            }

            // Advanced search filter
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                users = users.Where(u => u.FullName.Contains(request.Name));
            }

            // Status filter (active/inactive)
            if (request.IsActive.HasValue)
            {
                if (request.IsActive.Value)
                {
                    // Active users: not locked out or lockout has expired
                    users = users.Where(u => u.IsActive);
                }
                else
                {
                    // Inactive users: locked out and lockout is still active
                    users = users.Where(u => !u.IsActive);
                }
            }

            // Role filter (requires separate handling due to many-to-many relationship)
            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                var usersInRole = await _identityServiceManager.UserManagmentService.GetUsersByRole(request.Role);
                var userIdsInRole = usersInRole.Select(u => u.Id).ToList();
                users = users.Where(u => userIdsInRole.Contains(u.Id));
            }

            return users;
        }


        private async Task<PaginatedResult<GetUserListResponse>> HandleRoleBasedSorting(IQueryable<User> users, ListQuery request)
        {
            var usersList = users.ToList();

            // Map users to DTOs
            var userDtos = _mapper.Map<List<GetUserListResponse>>(usersList);

            // Apply localization to role names
            ApplyRoleLocalization(userDtos);

            // Sort by localized primary role names
            userDtos = request.OrderBy.Contains("asc") ?
                userDtos.OrderBy(u => u.PrimaryRole ?? string.Empty).ToList() :
                userDtos.OrderByDescending(u => u.PrimaryRole ?? string.Empty).ToList();

            var totalCount = userDtos.Count;
            var pageNumber = request.PageNumber <= 0 ? 1 : request.PageNumber;
            var pageSize = request.PageSize <= 0 ? 200 : request.PageSize;

            var pagedUsers = userDtos
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return PaginatedResult<GetUserListResponse>.Success(pagedUsers, totalCount, pageNumber, pageSize);
        }

        /// <summary>
        /// Apply localization to role names in user response DTOs
        /// Converts database role names to localized display names based on user's preferred language
        /// </summary>
        /// <param name="userResponses">List of user response DTOs to localize</param>
        private void ApplyRoleLocalization(IEnumerable<GetUserListResponse> userResponses)
        {
            foreach (var userResponse in userResponses)
            {
                // Localize all role names
                if (userResponse.Roles != null && userResponse.Roles.Count > 0)
                {
                    userResponse.Roles = [.. userResponse.Roles
                        .Select(roleName => LocalizationHelper.GetUserRoleDisplay(roleName, _localizer))];
                }

                // Localize primary role name
                if (!string.IsNullOrEmpty(userResponse.PrimaryRole))
                {
                    userResponse.PrimaryRole = LocalizationHelper.GetUserRoleDisplay(userResponse.PrimaryRole, _localizer);
                }
            }
        }
        #endregion
    }
}
