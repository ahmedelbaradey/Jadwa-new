namespace Application.Features.BoardMembers.Dtos
{
    /// <summary>
    /// Response DTO for board member list operations
    /// Contains collection of board members with metadata
    /// Based on requirements in Sprint.md (JDWA-595)
    /// </summary>
    public record BoardMembersListResponse
    {
        /// <summary>
        /// Collection of board members
        /// </summary>
        public IEnumerable<BoardMemberDto> BoardMembers { get; set; } = [];

        /// <summary>
        /// Total count of board members
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Count of independent members
        /// </summary>
        public int IndependentCount { get; set; }

        /// <summary>
        /// Count of non-independent members
        /// </summary>
        public int NonIndependentCount { get; set; }

        /// <summary>
        /// Indicates if fund has a chairman
        /// </summary>
        public bool HasChairman { get; set; }

        /// <summary>
        /// Fund name for context
        /// </summary>
        public string FundName { get; set; } = string.Empty;
    }
}
