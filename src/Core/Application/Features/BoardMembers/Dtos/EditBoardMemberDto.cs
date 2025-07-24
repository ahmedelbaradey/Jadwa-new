namespace Application.Features.BoardMembers.Dtos
{
    /// <summary>
    /// Data Transfer Object for editing an existing board member
    /// Inherits from BoardMemberDto following Clean DTOs template patterns
    /// Used in EditBoardMember command operations
    /// Based on requirements in Sprint.md (JDWA-596)
    /// </summary>
    public record EditBoardMemberDto : BoardMemberDto
    {
        // All properties inherited from BoardMemberDto
        // Id property from BaseDto is used for identifying the entity to update
        // Audit fields (UpdatedAt, UpdatedBy) are handled by the audit system
    }
}
