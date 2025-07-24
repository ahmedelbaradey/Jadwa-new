using Abstraction.Base.Response;
using Application.Features.MeetingTimeProposals.Commands.Create;
using Application.Features.MeetingTimeProposals.Commands.Vote;
using Application.Features.MeetingTimeProposals.Queries.GetDetails;
using Application.Features.MeetingTimeProposals.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Bases;

namespace Presentation.Controllers.Meeting
{
    /// <summary>
    /// Controller for managing meeting time proposals and voting
    /// Implements User Stories 1 and 2 from Meetings.md
    /// Follows AppControllerBase pattern with proper authorization
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MeetingTimeProposalsController : AppControllerBase
    {
        /// <summary>
        /// Creates a new meeting time proposal
        /// User Story 1: Propose Meeting Times for Voting
        /// Accessible by Legal Counsel and Board Secretary only
        /// </summary>
        /// <param name="command">Meeting time proposal creation data</param>
        /// <returns>Success message if proposal created successfully</returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateProposal([FromBody] CreateMeetingTimeProposalCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        /// <summary>
        /// Gets detailed information about a meeting time proposal
        /// Used for displaying proposal details and voting interface
        /// Accessible by all authenticated users in the fund
        /// </summary>
        /// <param name="proposalId">Proposal identifier</param>
        /// <returns>Detailed proposal information including voting status</returns>
        [HttpGet("{proposalId}")]
        [ProducesResponseType(typeof(BaseResponse<MeetingTimeProposalResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<MeetingTimeProposalResponseDto>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(BaseResponse<MeetingTimeProposalResponseDto>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<MeetingTimeProposalResponseDto>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProposalDetails(int proposalId)
        {
            var query = new GetMeetingTimeProposalDetailsQuery(proposalId);
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        /// <summary>
        /// Submits a vote on a meeting time proposal
        /// User Story 2: Vote on Proposed Meeting Times
        /// Accessible by Board Members only
        /// </summary>
        /// <param name="command">Vote submission data</param>
        /// <returns>Success message if vote submitted successfully</returns>
        [HttpPost("vote")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SubmitVote([FromBody] SubmitMeetingTimeVoteCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
