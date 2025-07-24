namespace Abstraction.Contracts.Identity
{
    public  interface IIdentityServiceManager
    {
        public IAuthenticationService AuthenticationService { get; }
        public IAuthorizationService AuthorizationService { get; }
        public IUserManagmentService UserManagmentService { get; }
    }
}
