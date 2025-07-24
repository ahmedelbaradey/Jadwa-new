using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logger;
using Domain.Entities.Users;
using Domain.Helpers;
using Infrastructure.Identity.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;


namespace Infrastructure.Service
{
    public class IdentityServiceManager : IIdentityServiceManager
    {
        private readonly Lazy<IAuthorizationService> _authorizationService;
        private readonly Lazy<IAuthenticationService> _authenticationService;
        private readonly Lazy<IUserManagmentService> _userManagmentService;
        private readonly ILoggerManager _logger;
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly RoleManager<Role> _roleManager;
        private readonly IDistributedCache _distributedCache;
        public IdentityServiceManager(ILoggerManager logger, RoleManager<Role> roleManager, UserManager<User> userManager, JwtSettings jwtSettings, IDistributedCache distributedCache)
        {

            _roleManager = roleManager;
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _logger = logger;
            _distributedCache = distributedCache;
            _authorizationService = new Lazy<IAuthorizationService>(() => new AuthorizationIdentityService(_roleManager, _userManager, _logger));
            _authenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationIdentityService(_userManager, roleManager, _jwtSettings, _logger, _distributedCache));
            _userManagmentService = new Lazy<IUserManagmentService>(() => new UserManagmentIdentityService(_userManager, _logger));
        }
        public IAuthorizationService AuthorizationService => _authorizationService.Value;
        public IAuthenticationService AuthenticationService => _authenticationService.Value;
        public IUserManagmentService UserManagmentService => _userManagmentService.Value;
    }
}
