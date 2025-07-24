using Application.Common.Dtos;

namespace Application.Features.BoardMembers.Dtos
{
    /// <summary>
    /// User information DTO for board member context
    /// Inherits from UserBaseDto following Clean DTOs template patterns
    /// Contains user-specific properties for board member operations
    /// </summary>
    public record BoardMemberUserDto : UserBaseDto
    {
        // All user properties inherited from UserBaseDto
        // No additional properties needed for board member user context
    }
}
