using System.Security.Claims;
using Abstraction.Constants;
using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logger;
using Domain.Entities.Users;
using Domain.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Identity.Implementations
{
    public class AuthorizationIdentityService : IAuthorizationService
    {
        #region Fileds
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        private readonly ILoggerManager _logger;
        #endregion

        #region Constructors
        public AuthorizationIdentityService(RoleManager<Role> roleManager, UserManager<User> userManager, ILoggerManager logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }
        #endregion

        #region Functions

        public async Task<bool> AddRoleAsync(string roleName, List<RoleClaims> roleClaims)
        {
            try
            {
              
                var role = new Role();
                role.Name = roleName.Trim().ToLower();

                var result = await _roleManager.CreateAsync(role);
                var addedClaims = roleClaims.Where(c => c.HasClaim).Select(a => a.Value).ToList();
                int affectedClaims = 0;
                // add claim if not exist in DBClaims and exist in inComingClaimNames
                if(addedClaims.Any())
                {
                    foreach (var claim in addedClaims)
                    {
                        affectedClaims++;
                        await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, claim));
                    }
                }
                if (!result.Succeeded)
                    return false;

                return result.Succeeded || affectedClaims > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AddRoleAsync");
                throw;
            }
        }
        public async Task<bool> EditRoleById(int Id, string roleName, List<RoleClaims> roleClaims)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(Id.ToString());
                if (role == null)
                    return false;

                role.Name = roleName;
                var result = await _roleManager.UpdateAsync(role);

                if (!result.Succeeded)
                    return false;
                var dbClaims = await _roleManager.GetClaimsAsync(role);
                var dbClaimNames = dbClaims.Select(a => a.Value).ToList();
                var addedClaims  = roleClaims.Where(c=> c.HasClaim).Select(a => a.Value).ToList();
                var deletedClaims = roleClaims.Where(c => !c.HasClaim).Select(a => a.Value).ToList();
                int affectedClaims = 0;

                // add claim if not exist in DBClaims and exist in inComingClaimNames
                if (addedClaims.Any())
                {
                    foreach (var claim in addedClaims)
                    {
                        if (!dbClaimNames.Contains(claim))
                        {
                            affectedClaims++;
                            await _roleManager.AddClaimAsync(role, new Claim(CustomClaimTypes.Permission, claim));
                        }
                    }
                }

                // remove claim if exist in DBClaims and not exist in inComingClaimNames
                if(dbClaims.Any())
                {
                    foreach (var claim in dbClaims)
                    {
                        if (deletedClaims.Contains(claim.Value))
                        {
                            affectedClaims++;
                            await _roleManager.RemoveClaimAsync(role, claim);
                        }
                    }
                }      
                
                return result.Succeeded || affectedClaims > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EditRoleById");
                throw;
            }
        }
        public async Task<bool> DeleteRoleById(Role role)
        {
            try
            {
                var users = await _userManager.GetUsersInRoleAsync(role.Name!);
                if (users != null && users.Count() > 0)
                    return false;


                var result = await _roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in DeleteRoleById");
                throw;
            }
        }
        public async Task<bool> IsRoleNameExist(string rolename)
        {
            try
            {
                var result = await _roleManager.RoleExistsAsync(rolename.ToLower());
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in IsRoleNameExist");
                throw;
            }
        }
        public async Task<Role> GetRoleByID(int Id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(Id.ToString());
                return role;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRoleByID");
                throw;
            }
        }
        public async Task<List<Role>> GetRoleListAsync()
        {
            try
            {
                var roles = _roleManager.Roles.ToList();
                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRoleListAsync");
                throw;
            }
        }

        public async Task<User> GetUserByRoles(int userId)
        {
            try
            {
                var user = await _userManager.Users.Include(u => u.Roles).Where(c => c.Id == userId).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRoleListAsync");
                throw;
            }
        }
        public async Task<ManageUserRolesResponse> GetUsersRoles(User user)
        {
            try
            {
                var userRoles = new List<UserRoles>();
                var response = new ManageUserRolesResponse();
                var rolesForUser = await _userManager.GetRolesAsync(user);
                var rolesInSystem = _roleManager.Roles.ToList();
                foreach (var role in rolesInSystem)
                {
                    var userRole = new UserRoles();
                    userRole.Id = role.Id;
                    userRole.Name = role.Name!;
                    userRole.HasRole = rolesForUser.Contains(role.Name!);
                    userRoles.Add(userRole);
                }

                response.UserId = user.Id;
                response.UserRoles = userRoles;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetManagerUsersRolesData");
                throw;
            }
        }
        public async Task<string> UpdateUserRoles(EditUserRolesRequest request)
        {

           // var trans = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId.ToString());
                if (user == null)
                {
                    return "UserNotFound";
                }
                var rolesForUser = await _userManager.GetRolesAsync(user);
                var addedRoles = request.UserRoles.Where(x => x.HasRole == true).Select(x => x.Name);
                foreach (var item in addedRoles)
                {
                    if (!rolesForUser.Contains(item))
                    {
                        await _userManager.AddToRoleAsync(user, item);
                    }
                }
                var deleted = request.UserRoles.Where(x => x.HasRole == false).Select(x => x.Name);
                foreach (var item in deleted)
                {
                    if (rolesForUser.Contains(item))
                    {
                        await _userManager.RemoveFromRoleAsync(user, item);
                    }
                }
                //  await trans.CommitAsync();
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateUserRoles");
              //  await trans.RollbackAsync();
                return "FaildAdded";
                throw;
            }
        }

        public async Task<string> EditUserRoles(List<string> roles, User user)
        {
            try
            {
                var rolesForUser = await _userManager.GetRolesAsync(user);
                
                foreach (var item in roles)
                {
                    if (!rolesForUser.Contains(item))
                    {
                        await _userManager.AddToRoleAsync(user, item);
                    }
                }
                var deleted = rolesForUser.Except(roles).ToList();
                foreach (var item in deleted)
                {
                    if (rolesForUser.Contains(item))
                    {
                        await _userManager.RemoveFromRoleAsync(user, item);
                    }
                }
                return "Success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UpdateUserRoles");
              //  await trans.RollbackAsync();
                return "FaildAdded";
                throw;
            }
        }

        public async Task<List<RoleClaims>> GetRoleClaims(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var claimsInSystem = new List<string>();
            foreach (var module in Claims.GenerateModules())
            {
                foreach (var claim in Claims.GeneratePermissions(module))
                {
                    claimsInSystem.Add($"{claim}");
                }
            }
            var _claims = await _roleManager.GetClaimsAsync(role);
            var currentRoleClaims = _claims.Select(a => a.Value).ToList();

            var roleClaims = new List<RoleClaims>();

            foreach (var claim in claimsInSystem)
            {
                var roleClaim = new RoleClaims
                {
                    Value = claim!,
                    Type = "Permission",
                    HasClaim = currentRoleClaims.Contains(claim!)
                };
                roleClaims.Add(roleClaim);
            }

            return roleClaims;
        }
     
         

     
        #endregion
    }
}
