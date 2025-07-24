using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Application.Features.MeetingTimeProposals.Dtos;

namespace Application.Features.MeetingTimeProposals.Queries.GetDetails
{
    /// <summary>
    /// Query for getting meeting time proposal details
    /// Used for displaying proposal information for voting
    /// </summary>
    public record GetMeetingTimeProposalDetailsQuery : IQuery<BaseResponse<MeetingTimeProposalResponseDto>>
    {
        /// <summary>
        /// Proposal identifier to retrieve
        /// </summary>
        public int ProposalId { get; set; }

 
    }
}
