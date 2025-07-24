using Application.Features.Identity.Authorizations.Queries.Responses;
using Domain.Entities.Users;
using Domain.Entities.Users;

namespace Application.Mapping.Authorizations
{
    public partial class AuthorizationProfile
    {
        public void GetRoleByIdMapping()
        {
            CreateMap<Role, GetRoleByIdResponse>()
                .ForMember(dest => dest.roleName, opt => opt.MapFrom(src => src.Name));
        }
    }
}
