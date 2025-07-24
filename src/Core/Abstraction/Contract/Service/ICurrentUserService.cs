using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abstraction.Contract.Service
{
    public interface ICurrentUserService
    {
        string? UserName { get; }
        int? UserId { get; }
        IList<string> Roles { get; }

        /// <summary>
        /// Get a specific claim value from the current user's JWT token
        /// </summary>
        /// <param name="claimType">The type of claim to retrieve</param>
        /// <returns>The claim value if found, null otherwise</returns>
        string? GetClaimValue(string claimType);
    }
}
