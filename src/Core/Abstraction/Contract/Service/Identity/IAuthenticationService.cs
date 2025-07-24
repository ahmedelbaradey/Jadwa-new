using System.IdentityModel.Tokens.Jwt;
using Domain.Helpers;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;

namespace Abstraction.Contracts.Identity
{
    public interface IAuthenticationService
    {
        public Task<JwtAuthResponse> GetJwtToken(User user);
        public JwtSecurityToken ReadJwtToken(string accessToken);
        public Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshTken);
        public Task<JwtAuthResponse> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken);
        public Task<string> ValidateJwtToken(string accessToken);
        Task<bool>  HasPasswordAsync(User user);
        Task<IdentityResult>  RemovePasswordAsync(User user);
        Task<IdentityResult> AddPasswordAsync(User user,string password);
        Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword);
    }
}
