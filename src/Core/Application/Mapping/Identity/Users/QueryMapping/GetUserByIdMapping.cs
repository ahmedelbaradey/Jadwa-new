using Application.Features.Identity.Users.Queries.Responses;
using Domain.Entities.Users;

namespace Application.Mapping.Users
{
    public partial class UserProfile
    {
        public void GetUserByIdMapping()
        {
            CreateMap<User, GetUserResponse>();
        }
    }
}
