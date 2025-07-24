using Application.Features.Identity.Users.Dtos;
using Domain.Entities.Users;

namespace Application.Mapping.Users
{
    public partial class UserProfile
    {
        public void GetUserProfileMapping()
        {
            CreateMap<User, UserProfileResponseDto>()
                .ForMember(dest => dest.CVFilePath, opt => opt.Ignore()) // Will be populated with preview URL in handler
                .ForMember(dest => dest.PersonalPhotoPath, opt => opt.Ignore()) // Will be populated with preview URL in handler
                .ForMember(dest => dest.PreviewUrl, opt => opt.Ignore()); // Will be populated with preview URL in handler
        }
    }
}
