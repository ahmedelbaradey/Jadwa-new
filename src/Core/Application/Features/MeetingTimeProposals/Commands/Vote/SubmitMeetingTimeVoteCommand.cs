using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Application.Features.MeetingTimeProposals.Dtos;

namespace Application.Features.MeetingTimeProposals.Commands.Vote
{
    /// <summary>
    /// Command for submitting a vote on meeting time proposal
    /// Implements User Story 2: Vote on Proposed Meeting Times
    /// Inherits from SubmitMeetingTimeVoteDto following Resolution module pattern
    /// Allows board members to vote on their preferred time options
    /// </summary>
    public record SubmitMeetingTimeVoteCommand : SubmitMeetingTimeVoteDto, ICommand<BaseResponse<string>>
    {
        // All properties inherited from SubmitMeetingTimeVoteDto
        // Following the exact pattern of AddResolutionCommand : AddResolutionDto
    }
}
