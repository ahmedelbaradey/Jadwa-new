using Domain.Entities.Users;
using Domain.Helpers;

namespace Application.Features.Identity.Users.Queries.Responses
{
    public record GetUserResponse : GetUserListResponse
    {
        public List<UserRoles> UserRoles { get; set; }
    }
}
