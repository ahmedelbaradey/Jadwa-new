using Abstraction.Base.Dto;

namespace Application.Features.MeetingTimeProposals.Dtos
{
    /// <summary>
    /// DTO for creating a new meeting time proposal
    /// Used in User Story 1: Propose Meeting Times for Voting
    /// Converted to record for immutability and better performance
    /// </summary>
    public record CreateMeetingTimeProposalDto : BaseDto
    {
        /// <summary>
        /// Fund identifier that this proposal belongs to
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Subject of the meeting proposal
        /// Required field, maximum 255 characters
        /// </summary>
        public string Subject { get; set; } = string.Empty;

        /// <summary>
        /// Optional description of the meeting proposal
        /// Maximum 1000 characters
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// List of proposed date/time options
        /// Minimum 1, Maximum 4 proposed dates allowed
        /// </summary>
        public List<ProposedDateDto> ProposedDates { get; set; } = new List<ProposedDateDto>();

        /// <summary>
        /// Optional attachment ID for PDF files
        /// Uses the established AttachmentId pattern
        /// </summary>
        public int? AttachmentId { get; set; }
    }

    /// <summary>
    /// DTO for proposed date and time options
    /// Converted to record for immutability and better performance
    /// </summary>
    public record ProposedDateDto
    {
        /// <summary>
        /// Proposed date and time for the meeting
        /// Must be in the future
        /// </summary>
        public DateTime ProposedDateTime { get; set; }
    }
}
