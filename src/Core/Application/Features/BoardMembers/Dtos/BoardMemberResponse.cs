namespace Application.Features.BoardMembers.Dtos
{
    /// <summary>
    /// Response DTO for board member operations
    /// Used for API responses with operation messages
    /// Based on requirements in Sprint.md (JDWA-596)
    /// </summary>
    public record BoardMemberResponse : BoardMemberDto
    {
        /// <summary>
        /// Success message for the operation
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
