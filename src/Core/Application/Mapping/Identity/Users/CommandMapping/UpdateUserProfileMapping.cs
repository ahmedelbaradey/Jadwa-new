using Application.Features.Identity.Users.Commands.UpdateUserProfile;
using Domain.Entities.Users;

namespace Application.Mapping.Users
{
    public partial class UserProfile
    {
        public void UpdateUserProfileMapping()
        {
            CreateMap<UpdateUserProfileCommand, User>();
        }
    }
}
