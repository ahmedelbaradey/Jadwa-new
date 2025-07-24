using Application.Features.Identity.Users.Commands.AddUser;
using Domain.Entities.Users;

namespace Application.Mapping.Users
{
    public partial class UserProfile
    {
        public void AddUserMapping()
        {
            CreateMap<AddUserCommand, User>()
                  .ForMember(dest => dest.Roles, opt => opt.Ignore());


        }
    }
}
