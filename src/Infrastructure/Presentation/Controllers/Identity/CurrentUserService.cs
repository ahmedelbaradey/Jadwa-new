using Abstraction.Contract.Service;
using Domain.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers.Identity
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? UserName =>
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        public int? UserId =>
           Convert.ToInt32(_httpContextAccessor.HttpContext?.User?.FindFirstValue(nameof(UserClaimsModel.Id)));

        public IList<string> Roles =>
            _httpContextAccessor.HttpContext?.User?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList() ?? new List<string>();

        /// <summary>
        /// Get a specific claim value from the current user's JWT token
        /// </summary>
        /// <param name="claimType">The type of claim to retrieve</param>
        /// <returns>The claim value if found, null otherwise</returns>
        public string? GetClaimValue(string claimType)
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirstValue(claimType);
        }
    }
}