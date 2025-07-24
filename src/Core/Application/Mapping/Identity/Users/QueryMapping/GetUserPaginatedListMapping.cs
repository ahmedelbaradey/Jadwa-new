using Application.Features.Identity.Users.Queries.Responses;
using Domain.Entities.Users;

namespace Application.Mapping.Users
{
    public partial class UserProfile
    {
        public void GetUserPaginatedListMapping()
        {
            CreateMap<User, GetUserListResponse>()
                // Map roles directly from navigation properties for optimized performance
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name).ToList()))
                .ForMember(dest => dest.PrimaryRole, opt => opt.MapFrom(src => src.Roles.Select(r => r.Name).FirstOrDefault()))
                .ForMember(dest => dest.LastUpdateDate, opt => opt.MapFrom(src => src.UpdatedAt));
        }
    }
}
