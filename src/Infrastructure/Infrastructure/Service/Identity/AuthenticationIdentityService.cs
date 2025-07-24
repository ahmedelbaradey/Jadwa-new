using Domain.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Domain.Entities.Users;
using Microsoft.Extensions.Caching.Distributed;
using Abstraction.Constants;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Identity;
using Abstraction.Common.Extensions;


namespace Infrastructure.Identity.Implementations
{
    public class AuthenticationIdentityService : IAuthenticationService
    {
        #region Fileds
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ILoggerManager _logger;
        private readonly IDistributedCache _distributedCache;
        #endregion

        #region Constructors
        public AuthenticationIdentityService(UserManager<User> userManager, RoleManager<Role> roleManager, JwtSettings jwtSettings, ILoggerManager logger, IDistributedCache distributedCache)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _logger = logger;
            _roleManager = roleManager;
            _distributedCache = distributedCache;
        }

        #endregion

        #region Main_Functions

        public async Task<JwtAuthResponse> GetJwtToken(User user)
        {
            try
            {
                //Generate Jwt Token..
                var (jwtToken, accessToken) = await GenerateJwtToken(user);

                //Generate Resfresh Token
                //var refreshToken = RefreshToken(user.UserName!);
                ////Save To Redis
                //var userRefreshToken = new UserRefreshToken
                //{
                //    AddedTime = DateTime.Now,
                //    ExpiryDate = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
                //    IsUsed = true,
                //    IsRevoked = false,
                //    JwtId = jwtToken.Id,
                //    RefreshToken = refreshToken.TokenString,
                //    Token = accessToken,
                //    UserId = user.Id
                //};
                //await Create(userRefreshToken);

                //return response
                var response = new JwtAuthResponse
                {
                    AccessToken = accessToken,
                    //refreshToken = refreshToken
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetJwtToken");
                throw;
            }
        }
        public async Task<bool>  HasPasswordAsync(User user)
        {
            try
            {
                 return await _userManager.HasPasswordAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetJwtToken");
                throw;
            }
        }     
        public async Task<IdentityResult> RemovePasswordAsync(User user)
        {
            try
            {
                return await _userManager.RemovePasswordAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetJwtToken");
                throw;
            }
        }
        public async Task<IdentityResult> AddPasswordAsync(User user,string password)
        {
            try
            {
                return  await _userManager.AddPasswordAsync(user,password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetJwtToken");
                throw;
            }
        }
        public async Task<JwtAuthResponse> GetRefreshToken(User user, JwtSecurityToken jwtToken, DateTime? expiryDate, string refreshToken)
        {
            try
            {
                var (jwtSecurityToken, newToken) = await GenerateJwtToken(user);
                var refreshTokenResult = new RefreshToken();
                refreshTokenResult.UserName = jwtToken.Claims.FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")!.Value;
                refreshTokenResult.TokenString = refreshToken;
                refreshTokenResult.ExpireAt = (DateTime)expiryDate;

                var response = new JwtAuthResponse();
                response.AccessToken = newToken;
                response.refreshToken = refreshTokenResult;

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetRefreshToken");
                throw;
            }

        }
        public JwtSecurityToken ReadJwtToken(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new ArgumentNullException(nameof(accessToken));
            }
            var handler = new JwtSecurityTokenHandler();
            var response = handler.ReadJwtToken(accessToken);
            return response;
        }
        public async Task<(string, DateTime?)> ValidateDetails(JwtSecurityToken jwtToken, string accessToken, string refreshTken)
        {
            try
            {
                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
                {
                    return ("AlgorithmIsWrong", null);
                }
                if (jwtToken.ValidTo > DateTime.Now)
                {
                    return ("TokenIsRunning", null);
                }
                //Get UserId From Glaims in jwtToken
                var userId = jwtToken.Claims.FirstOrDefault(x => x.Type == "Id")!.Value;
                var userRefreshtoken = await GetById(int.Parse(userId));
                if (userRefreshtoken == null)
                {
                    return ("RefreshTokenNotFound", null);
                }
                var expiryDate = userRefreshtoken.ExpiryDate;
                return (userId, expiryDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ValidateDetails");
                throw;
            }
        }
        public async Task<string> ValidateJwtToken(string accessToken)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuer = _jwtSettings.ValidateIssure,
                    ValidIssuers = new[] { _jwtSettings.Issure },
                    ValidateIssuerSigningKey = _jwtSettings.ValidateIssureSigningKey,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret)),
                    ValidateAudience = _jwtSettings.validateAudience,
                    ValidAudience = _jwtSettings.Audience,
                    ValidateLifetime = _jwtSettings.ValidateLifeTime
                };
                var validator = handler.ValidateToken(accessToken, parameters, out SecurityToken validatedToken);
                if (validator == null)
                {
                    return "InvalidJwtToken";
                }
                return "NotExpired";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ValidateJwtToken");
                throw;
            }
        }      
        public Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
        {
            try
            {
               return _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetJwtToken");
                throw;
            }
             
        }
        #endregion

        #region Sub-Functions
        private async Task<(JwtSecurityToken, string)> GenerateJwtToken(User user)
        {
            var roleNames = await _userManager.GetRolesAsync(user);
            var Claims = await GetClaims(user, roleNames.ToList());

            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSettings.Secret));
            var _CredentialS = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var jwtToken = new JwtSecurityToken(
                _jwtSettings.Issure,
                _jwtSettings.Audience,
                claims: Claims,
                expires: DateTime.Now.AddMinutes(_jwtSettings.AccessTokenExpireMinutes),
                signingCredentials: _CredentialS
                );

            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return (jwtToken, accessToken);
        }
        private async Task<List<Claim>> GetClaims(User user, List<string> roles)
        {
            // Generate unique identifiers for session tracking
            var jwtId = Guid.NewGuid().ToString();
            var sessionId = Guid.NewGuid().ToString();

            //Add some properties to claims...
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,user.FullName!),
                new Claim(ClaimTypes.NameIdentifier,user.UserName!),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(nameof(UserClaimsModel.Id),user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, jwtId), // JWT ID for token identification
                new Claim("SessionId", sessionId), // Session ID for session management
            };

            //Add roles to claims...
            foreach (var roleName in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, roleName));
                var _role = await _roleManager.FindByNameAsync(roleName);
                if (_role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(_role);
                    foreach (Claim roleClaim in roleClaims)
                    {
                        claims.Add(new Claim(CustomClaimTypes.Permission, roleClaim.Value));
                    }
                }
            }
            return claims;
        }
        private async Task<UserRefreshToken?> GetById(int userId)
        {
            string key = $"userrefreshtoken-{userId}";
            if (!_distributedCache.TryGetValue(key, out UserRefreshToken? _userRefreshToken))
            {
                return null;
            }
            return _userRefreshToken;
        }
        private async Task<UserRefreshToken?> Create(UserRefreshToken userRefreshToken)
        {
            string key = $"userrefreshtoken-{userRefreshToken.UserId}";
            var cacheEntryOptions = new DistributedCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(3600))
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600));
            await _distributedCache.SetAsync(key, userRefreshToken, cacheEntryOptions);

            if (!_distributedCache.TryGetValue(key, out UserRefreshToken? _userRefreshToken))
            {
                return null;
            }
            return _userRefreshToken;
        }
        private RefreshToken RefreshToken(string username)
        {
            var refreshToken = new RefreshToken
            {
                ExpireAt = DateTime.Now.AddDays(_jwtSettings.RefreshTokenExpireDate),
                UserName = username,
                TokenString = GenerateRefreshToken()
            };
            return refreshToken;
        }
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            var randomNumberGenerate = RandomNumberGenerator.Create();
            randomNumberGenerate.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

     
        #endregion
    }
}
