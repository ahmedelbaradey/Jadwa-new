using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Identity.Users.Queries.Responses;

namespace Application.Features.Identity.Users.Queries.CheckSingleHolderRoleAvailability
{
    /// <summary>
    /// Query to check the availability status of single-holder roles in the system
    /// Used for determining which single-holder roles are currently assigned to active users
    /// Supports frontend workflows for role assignment decision-making
    /// </summary>
    public record CheckRoleAvailabilityQuery : IQuery<BaseResponse<SingleHolderRoleAvailabilityResponse>>
    {
        // No parameters needed - this query checks all single-holder roles
    }
}
