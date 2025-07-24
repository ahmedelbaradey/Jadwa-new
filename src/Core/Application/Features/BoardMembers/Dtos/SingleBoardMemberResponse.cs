namespace Application.Features.BoardMembers.Dtos
{
    /// <summary>
    /// Single board member response DTO following Clean DTOs template patterns
    /// Used for single entity API responses with display properties
    /// Based on requirements in Sprint.md (JDWA-595)
    /// </summary>
    public record SingleBoardMemberResponse : BoardMemberDto
    {
        /// <summary>
        /// Fund name for display
        /// </summary>
        public string FundName { get; set; } = string.Empty;

        /// <summary>
        /// User name for display
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// User full name for display
        /// </summary>
        public string UserFullName { get; set; } = string.Empty;

        /// <summary>
        /// Last update date for sorting and display
        /// </summary>
        public DateTime? LastUpdated { get; set; }
    }
}
