using Application.Features.Identity.Authorizations.Queries.Responses;
using Domain.Entities.Users;

namespace Application.Mapping.Authorizations
{
    public partial class AuthorizationProfile
    {
        public void GetRoleListMapping()
        {
            CreateMap<Role, GetRoleListResponse>()
                .ForMember(dest => dest.roleName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
