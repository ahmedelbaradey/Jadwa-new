using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Application.Features.MeetingTimeProposals.Dtos;

namespace Application.Features.MeetingTimeProposals.Commands.Create
{
    /// <summary>
    /// Command for creating a new meeting time proposal
    /// Implements User Story 1: Propose Meeting Times for Voting
    /// Inherits from CreateMeetingTimeProposalDto following Resolution module pattern
    /// Allows Legal Counsel and Board Secretary to create proposals with multiple time options
    /// </summary>
    public record CreateMeetingTimeProposalCommand : CreateMeetingTimeProposalDto, ICommand<BaseResponse<string>>
    {
        // All properties inherited from CreateMeetingTimeProposalDto
        // Following the exact pattern of AddResolutionCommand : AddResolutionDto
    }
}
