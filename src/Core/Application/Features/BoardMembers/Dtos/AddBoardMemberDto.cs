using Domain.Entities.FundManagement;

namespace Application.Features.BoardMembers.Dtos
{
    /// <summary>
    /// Data Transfer Object for adding a new board member
    /// Inherits from BoardMemberDto following Clean DTOs template patterns
    /// Used in AddBoardMember command operations
    /// Based on requirements in Sprint.md (JDWA-596)
    /// </summary>
    public record AddBoardMemberDto : BoardMemberDto
    {
        // All properties inherited from BoardMemberDto
        // No additional properties needed for creation
        // Audit fields (CreatedAt, CreatedBy) are handled by the audit system
    }
 
}
