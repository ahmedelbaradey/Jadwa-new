using System.Linq;
using System.Security.Claims;
using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logger;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Identity.Implementations
{
    public class UserManagmentIdentityService : IUserManagmentService
    {
        #region Fileds
        private readonly UserManager<User> _userManager;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructors
        public UserManagmentIdentityService(UserManager<User> userManager, ILoggerManager logger)
        {
            _userManager = userManager;
            _logger = logger;   
        }
        #endregion

        #region Functions
        public async Task<IdentityResult> AddToRoleAsync(User user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }
        public async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            return await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        public async Task<IdentityResult> CreateAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> DeleteAsync(User user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<User?> FindByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<User?> FindByIdAsync(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<User?> FindByNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<IdentityResult> UpdateAsync(User user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public IQueryable<User> Users()
        {
            return _userManager.Users;
        }

        public async Task<IEnumerable<User>> GetUsersByRole(string role)
        {
            return await _userManager.GetUsersInRoleAsync(role.ToLower());
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(User user)
        {
            return await _userManager.GetClaimsAsync(user);
        }

        public async Task UpdateSecurityStampAsync(User user)
        {
            await _userManager.UpdateSecurityStampAsync(user);
        }

        public async Task RemoveClaimsAsync(User user, List<Claim> fcmClaims)
        {
             await _userManager.RemoveClaimsAsync(user, fcmClaims);
        }

        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        // Enhanced methods for optimized role retrieval
        /// <summary>
        /// Retrieves a user by ID with roles eagerly loaded
        /// Uses Entity Framework Include to load roles in a single query, eliminating N+1 queries
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User with roles loaded, or null if not found</returns>
        public async Task<User?> FindByIdWithRolesAsync(string id)
        {
            try
            {
                return await _userManager.Users
                    .Include(u => u.Roles)
                    .FirstOrDefaultAsync(u => u.Id.ToString() == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with roles for ID: {id}");
                return null;
            }
        }

        /// <summary>
        /// Retrieves users with roles eagerly loaded using a queryable interface
        /// Enables efficient filtering and pagination while including role information
        /// </summary>
        /// <returns>Queryable of users with roles included</returns>
        public IQueryable<User> UsersWithRoles()
        {
            return _userManager.Users.Include(u => u.Roles);
        }

        /// <summary>
        /// Retrieves a list of users with roles eagerly loaded
        /// Optimized for scenarios where all users need to be loaded with their roles
        /// </summary>
        /// <returns>List of users with roles included</returns>
        public async Task<List<User>> GetUsersWithRolesAsync()
        {
            try
            {
                return await _userManager.Users
                    .Include(u => u.Roles)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users with roles");
                return new List<User>();
            }
        }

        /// <summary>  
        /// Checks if any active user (excluding the specified user) holds only the specified role  
        /// Used for unique role validation during user creation and updates  
        /// </summary>  
        /// <param name="roleName">The role name to check for uniqueness</param>  
        /// <param name="excludeUserId">User ID to exclude from the check (for edit scenarios)</param>  
        /// <returns>User who holds only this role, or null if no conflict</returns>  
        public async Task<User?> FindActiveUserWithOnlyRoleAsync(string roleName, int? excludeUserId = null)
        {
            try
            {
                var allUsers = await GetUsersWithRolesAsync();
                allUsers = allUsers.FindAll(u => u.Id != excludeUserId.Value && u.IsActive && u.Roles.Select(C=>C.Name).Contains(roleName));
                return allUsers.FirstOrDefault();
                //var usersWithRole = new List<User>();
                //foreach (var user in allUsers)
                //{
                //    var userRoles = user.Roles.Select(c => c.Name);
                //    if (userRoles.Count() == 1 && userRoles.Contains(roleName))
                //    {
                //        usersWithRole.Add(user);
                //    }
                //}

              //  return usersWithRole.FirstOrDefault();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error finding active user with only role {roleName}");
                return null;
            }

 
        }



        /// <summary>
        /// Checks if a user has multiple roles assigned
        /// Used to determine if a role conflict should be considered
        /// </summary>
        /// <param name="userId">User ID to check</param>
        /// <returns>True if user has multiple roles, false otherwise</returns>
        public async Task<bool> UserHasMultipleRolesAsync(int userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId.ToString());
                if (user == null) return false;

                var userRoles = await _userManager.GetRolesAsync(user);
                return userRoles.Count > 1;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking multiple roles for user {userId}");
                return false;
            }
        }

        /// <summary>
        /// Gets the list of users who hold only the specified role
        /// Used for role conflict resolution and replacement scenarios
        /// </summary>
        /// <param name="roleName">The role name to search for</param>
        /// <returns>List of users who hold only this role</returns>
        public async Task<List<User>> GetUsersWithOnlyRoleAsync(string roleName)
        {
            try
            {
                var activeUsers = await _userManager.Users
                    .Where(u => u.IsActive && !u.IsDeleted.HasValue || !u.IsDeleted.Value)
                    .ToListAsync();

                var usersWithOnlyRole = new List<User>();

                foreach (var user in activeUsers)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    if (userRoles.Count == 1 && userRoles.Contains(roleName))
                    {
                        usersWithOnlyRole.Add(user);
                    }
                }

                return usersWithOnlyRole;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting users with only role {roleName}");
                return new List<User>();
            }
        }
        #endregion
    }
}
