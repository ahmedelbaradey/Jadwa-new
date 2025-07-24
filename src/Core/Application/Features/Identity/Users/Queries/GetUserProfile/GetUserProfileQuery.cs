using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Identity.Users.Dtos;

namespace Application.Features.Identity.Users.Queries.GetUserProfile
{
    /// <summary>
    /// Query to get user profile information
    /// Used for displaying user's personal profile data
    /// </summary>
    public record GetUserProfileQuery : IQuery<BaseResponse<UserProfileResponseDto>>
    {
        /// <summary>
        /// User ID to get profile for (optional - defaults to current user)
        /// </summary>
        public int? UserId { get; set; }
    }
}
