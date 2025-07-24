using AutoMapper;

namespace Application.Mapping.Authorizations
{
    public partial class AuthorizationProfile : Profile
    {
        public AuthorizationProfile()
        {
            GetRoleByIdMapping();
            GetRoleListMapping();
        }
    }
}
