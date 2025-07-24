namespace Application.Features.BoardMembers.Dtos
{
    /// <summary>
    /// Data Transfer Object for board member details response
    /// Includes user information and statistics following Clean DTOs patterns
    /// Combines BoardMember and User data for detailed views
    /// Based on requirements in Sprint.md (JDWA-595)
    /// </summary>
    public record BoardMemberDetailsDto : BoardMemberDto
    {
        /// <summary>
        /// User information embedded as separate DTO
        /// Follows composition over inheritance principle
        /// </summary>
        public BoardMemberUserDto User { get; set; } = new();

        /// <summary>
        /// Fund information for context
        /// </summary>
        public string FundName { get; set; } = string.Empty;

        /// <summary>
        /// Date when the board member was added
        /// </summary>
        public DateTime JoinedDate { get; set; }

        /// <summary>
        /// Number of resolution conflicts this member has
        /// </summary>
        public int ConflictCount { get; set; }

        /// <summary>
        /// Number of votes cast by this member
        /// </summary>
        public int VoteCount { get; set; }
    }
}
