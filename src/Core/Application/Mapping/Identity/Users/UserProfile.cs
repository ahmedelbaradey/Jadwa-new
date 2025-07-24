using AutoMapper;

namespace Application.Mapping.Users
{
    public partial class UserProfile : Profile
    {
        public UserProfile()
        {
            AddUserMapping();
            EditUserMapping();
            GetUserPaginatedListMapping();
            GetUserByIdMapping();
            UpdateUserProfileMapping();
            GetUserProfileMapping();
        }
    }
}
