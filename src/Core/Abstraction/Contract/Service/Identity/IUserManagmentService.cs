using System.Security.Claims;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
namespace Abstraction.Contracts.Identity
{
    public interface  IUserManagmentService
    {
        public Task<User?> FindByEmailAsync(string email);
        public Task<User?> FindByNameAsync(string userName);
        public Task<IdentityResult> CreateAsync(User user, string password);
        public Task<User?> FindByIdAsync(string id);
        public Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
        public Task<IdentityResult> DeleteAsync(User user);
        public Task<IdentityResult> UpdateAsync(User user);
        public Task<IdentityResult> AddToRoleAsync(User user, string role);
        public IQueryable<User> Users();
        public Task<IEnumerable<User>> GetUsersByRole(string role);
        Task<IEnumerable<Claim>> GetClaimsAsync(User user);
        Task UpdateSecurityStampAsync(User user);
        Task RemoveClaimsAsync(User user, List<Claim> fcmClaims);
        Task<IList<string>> GetUserRolesAsync(User user);

        // Enhanced methods for optimized role retrieval
        /// <summary>
        /// Retrieves a user by ID with roles eagerly loaded
        /// Optimizes performance by eliminating N+1 queries for role information
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User with roles loaded, or null if not found</returns>
        public Task<User?> FindByIdWithRolesAsync(string id);

        /// <summary>
        /// Retrieves users with roles eagerly loaded using a queryable interface
        /// Enables efficient filtering and pagination while including role information
        /// </summary>
        /// <returns>Queryable of users with roles included</returns>
        public IQueryable<User> UsersWithRoles();

        /// <summary>
        /// Retrieves a list of users with roles eagerly loaded
        /// Optimized for scenarios where all users need to be loaded with their roles
        /// </summary>
        /// <returns>List of users with roles included</returns>
        public Task<List<User>> GetUsersWithRolesAsync();

        // Role validation methods for unique role checking
        /// <summary>
        /// Checks if any active user (excluding the specified user) holds only the specified role
        /// Used for unique role validation during user creation and updates
        /// </summary>
        /// <param name="roleName">The role name to check for uniqueness</param>
        /// <param name="excludeUserId">User ID to exclude from the check (for edit scenarios)</param>
        /// <returns>User who holds only this role, or null if no conflict</returns>
        public Task<User?> FindActiveUserWithOnlyRoleAsync(string roleName, int? excludeUserId = null);

        /// <summary>
        /// Checks if a user has multiple roles assigned
        /// Used to determine if a role conflict should be considered
        /// </summary>
        /// <param name="userId">User ID to check</param>
        /// <returns>True if user has multiple roles, false otherwise</returns>
        public Task<bool> UserHasMultipleRolesAsync(int userId);

        /// <summary>
        /// Gets the list of users who hold only the specified role
        /// Used for role conflict resolution and replacement scenarios
        /// </summary>
        /// <param name="roleName">The role name to search for</param>
        /// <returns>List of users who hold only this role</returns>
        public Task<List<User>> GetUsersWithOnlyRoleAsync(string roleName);
    }
}
