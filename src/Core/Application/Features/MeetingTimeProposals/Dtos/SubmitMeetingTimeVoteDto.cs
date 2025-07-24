using Abstraction.Base.Dto;

namespace Application.Features.MeetingTimeProposals.Dtos
{
    /// <summary>
    /// DTO for submitting a vote on meeting time proposal
    /// Used in User Story 2: Vote on Proposed Meeting Times
    /// Converted to record for immutability and better performance
    /// </summary>
    public record SubmitMeetingTimeVoteDto : BaseDto
    {
        /// <summary>
        /// Proposal identifier to vote on
        /// </summary>
        public int ProposalId { get; set; }

        /// <summary>
        /// List of proposed date IDs that the user is voting for
        /// At least one selection is required
        /// </summary>
        public List<int> SelectedProposedDateIds { get; set; } = new List<int>();
    }
}
