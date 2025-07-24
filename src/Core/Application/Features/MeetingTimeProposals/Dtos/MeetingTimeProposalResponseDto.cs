using Abstraction.Base.Dto;

namespace Application.Features.MeetingTimeProposals.Dtos
{
    /// <summary>
    /// Response DTO for meeting time proposal details
    /// Used for displaying proposal information
    /// </summary>
    public record MeetingTimeProposalResponseDto :BaseDto
    {

        /// <summary>
        /// Fund identifier
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Fund name for display
        /// </summary>
        public string FundName { get; set; } = string.Empty;

        /// <summary>
        /// Subject of the meeting proposal
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Description of the meeting proposal
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Status of the proposal (Under Voting, Completed)
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// Name of the user who created this proposal
        /// </summary>
        public string CreatedByUserName { get; set; } = string.Empty;

        /// <summary>
        /// Creation date of the proposal
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// List of proposed date/time options
        /// </summary>
        public List<ProposedDateResponseDto> ProposedDates { get; set; } = new List<ProposedDateResponseDto>();

        /// <summary>
        /// Attachment information if available
        /// </summary>
        public AttachmentResponseDto? Attachment { get; set; }

        /// <summary>
        /// Total number of votes cast
        /// </summary>
        public int TotalVotes { get; set; }

        /// <summary>
        /// Total number of board members (for completion check)
        /// </summary>
        public int TotalBoardMembers { get; set; }

        /// <summary>
        /// Whether the current user has voted
        /// </summary>
        public bool HasCurrentUserVoted { get; set; }
    }

    /// <summary>
    /// Response DTO for proposed date information
    /// </summary>
    public class ProposedDateResponseDto
    {
        /// <summary>
        /// Proposed date identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Proposed date and time
        /// </summary>
        public DateTime ProposedDateTime { get; set; }

        /// <summary>
        /// Number of votes for this proposed date
        /// </summary>
        public int VoteCount { get; set; }
    }

    /// <summary>
    /// Response DTO for attachment information
    /// </summary>
    public class AttachmentResponseDto
    {
        /// <summary>
        /// Attachment identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; set; } = string.Empty;

        /// <summary>
        /// File URL for download/preview
        /// </summary>
        public string FileUrl { get; set; } = string.Empty;
    }
}
