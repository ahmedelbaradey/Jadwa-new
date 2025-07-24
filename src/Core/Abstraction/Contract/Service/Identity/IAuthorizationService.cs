using Domain.Entities.Users;
using Domain.Helpers;

namespace Abstraction.Contracts.Identity
{
    public interface IAuthorizationService
    {
        public Task<bool> AddRoleAsync(string roleName, List<RoleClaims> roleClaims);
        public Task<bool> EditRoleById(int Id, string roleName, List<RoleClaims> roleClaims);
        public Task<bool> DeleteRoleById(Role role);
        public Task<bool> IsRoleNameExist(string rolename);
        public Task<Role> GetRoleByID(int Id);
        public Task<List<Role>> GetRoleListAsync();
        public Task<ManageUserRolesResponse> GetUsersRoles(User user);
        public Task<string> UpdateUserRoles(EditUserRolesRequest request);
        public Task<List<RoleClaims>> GetRoleClaims(string roleId);
        Task<string> EditUserRoles(List<string> roles, User user);
        Task<User> GetUserByRoles(int userId);
    }
}
