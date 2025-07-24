using Application.Features.Identity.Users.Commands.EditUser;
using Application.Features.Identity.Users.Commands.EditUserPreferredLanguage;
using Domain.Entities.Users;

namespace Application.Mapping.Users
{
    public partial class UserProfile
    {
        public void EditUserMapping()
        {
            CreateMap<EditUserCommand, User>().ForMember(dest => dest.Roles, opt => opt.Ignore());

            CreateMap<EditUserPreferredLanguageCommand, User>();
        }
    }
}
