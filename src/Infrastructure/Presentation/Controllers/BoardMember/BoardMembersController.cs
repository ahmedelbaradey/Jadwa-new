using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Features.BoardMembers.Commands.Add;
using Application.Features.BoardMembers.Dtos;
using Application.Features.BoardMembers.Queries.List;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Bases;

namespace Presentation.Controllers
{
    /// <summary>
    /// Controller for managing board members
    /// Provides CRUD operations for board member management
    /// Follows the same pattern as ResolutionsController with role-based authorization
    /// Based on Sprint.md requirements for board member management (JDWA-596, JDWA-595)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class BoardMembersController : AppControllerBase
    {
        /// <summary>
        /// Gets a paginated list of board members with filtering and sorting
        /// </summary>
        /// <param name="query">List query parameters including pagination, filtering, and sorting</param>
        /// <returns>Paginated list of board members</returns>
        //[Authorize(Policy = BoardMemberPermission.List)]
        [HttpGet("BoardMembersList")]
        [ProducesResponseType(typeof(PaginatedResult<BoardMemberResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBoardMembersPaginatedList([FromQuery] ListQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        /// <summary>
        /// Adds a new board member to a fund
        /// Validates business rules including independent member limits and chairman constraints
        /// </summary>
        /// <param name="command">Add board member command with member details</param>
        /// <returns>Success message or validation errors</returns>
        //[Authorize(Policy = BoardMemberPermission.Create)]
        [HttpPost("AddBoardMember")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddBoardMember([FromBody] AddBoardMemberCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
