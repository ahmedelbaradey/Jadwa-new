using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Abstraction.Constants.ModulePermissions;
using Application.Features.Resolutions.Commands.Add;
using Application.Features.Resolutions.Commands.Cancel;
using Application.Features.Resolutions.Commands.Confirm;
using Application.Features.Resolutions.Commands.Delete;
using Application.Features.Resolutions.Commands.Edit;
using Application.Features.Resolutions.Commands.Reject;
using Application.Features.Resolutions.Commands.SendToVote;
using Application.Features.Resolutions.Dtos;
using Application.Features.Resolutions.Queries.Get;
using Application.Features.Resolutions.Queries.GetForEdit;
using Application.Features.Resolutions.Queries.GetResolutionAuditHistory;
using Application.Features.Resolutions.Queries.GetResolutionTypes;
using Application.Features.Resolutions.Queries.GetStatuses;
using Application.Features.Resolutions.Queries.List;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Bases;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResolutionsController : AppControllerBase
    {
        //[Authorize(Policy = ResolutionPermission.List)]
        [HttpGet("ResolutionsList")]
        [ProducesResponseType(typeof(PaginatedResult<SingleResolutionResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResolutionsPaginatedList([FromQuery] ListQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        //[Authorize(Policy = ResolutionPermission.View)]
        [HttpGet("GetResolutionById/{id}")]
        [ProducesResponseType(typeof(BaseResponse<SingleResolutionResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResolutionsById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetQuery() { Id = id });
            return NewResult(response);
        }

        //[Authorize(Policy = ResolutionPermission.View)]
        [HttpGet("GetResolutionForEdit/{id}")]
        [ProducesResponseType(typeof(BaseResponse<ResolutionForEditDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResolutionForEdit([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetForEditQuery() { Id = id });
            return NewResult(response);
        }

        //[Authorize(Policy = ResolutionPermission.Create)]
        [HttpPost("AddResolution")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddResolution([FromBody] AddResolutionCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        //[Authorize(Policy = ResolutionPermission.Edit)]
        [HttpPut("EditResolution")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> EditResolution([FromBody] EditResolutionCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        //[Authorize(Policy = ResolutionPermission.Delete)]
        [HttpDelete("DeleteResolution/{id}")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteResolution([FromRoute] int id)
        {
            var response = await Mediator.Send(new DeleteResolutionCommand() { Id = id });
            return NewResult(response);
        }

        //[Authorize(Policy = ResolutionPermission.Cancel)]
        [HttpPatch("CancelResolution/{id}")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> CancelResolution([FromRoute] int id)
        {
            var response = await Mediator.Send(new CancelResolutionCommand() { Id = id });
            return NewResult(response);
        }

        // New endpoints for JDWA-570 and JDWA-569
        //[Authorize(Policy = ResolutionPermission.Edit)]
        [HttpPatch("ConfirmResolution/{id}")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmResolution([FromRoute] int id)
        {
            var response = await Mediator.Send(new ConfirmResolutionCommand { Id = id });
            return NewResult(response);
        }

        //[Authorize(Policy = ResolutionPermission.Edit)]
        [HttpPatch("RejectResolution/{id}")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> RejectResolution([FromRoute] int id, [FromBody] RejectResolutionCommand command)
        {
            command.Id = id; // Ensure the ID from route is used
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        //[Authorize(Policy = ResolutionPermission.Edit)]
        [HttpPatch("SendToVote/{id}")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> SendToVote([FromRoute] int id)
        {
            var response = await Mediator.Send(new SendToVoteCommand { Id = id });
            return NewResult(response);
        }

        /// <summary>
        /// Gets all active resolution types
        /// </summary>
        /// <returns>List of active resolution types</returns>
        [HttpGet("ActiveTypes")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ResolutionTypeDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResolutionTypes()
        {
            var query = new GetResolutionTypesQuery();
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        /// <summary>
        /// Gets all resolution types including inactive ones
        /// </summary>
        /// <returns>List of all resolution types</returns>
        [HttpGet("Types/All")]
        [ProducesResponseType(typeof(BaseResponse<IEnumerable<ResolutionTypeDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllResolutionTypes()
        {
            var query = new GetResolutionTypesQuery { IncludeInactive = true };
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        /// <summary>
        /// Gets all available resolution statuses By Fund Id To Handle Roles
        /// Used for populating dropdowns and filters in the frontend
        /// Returns localized status names based on current culture
        /// If no fund ID is provided, returns statuses based on user's global role
        /// </summary>
        /// <returns>List of all resolution statuses with localized names</returns>
        [HttpGet("Statuses/{id?}")]
        [ProducesResponseType(typeof(BaseResponse<List<ResolutionStatusDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResolutionStatuses([FromRoute] int? id = null)
        {
            var query = new GetResolutionStatusesQuery() { FundId = id };
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        //[Authorize(Policy = ResolutionPermission.View)]
        [HttpGet("GetResolutionStatusById/{id}")]
        [ProducesResponseType(typeof(BaseResponse<ResolutionStatusHistoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetResolutionStatusById([FromRoute] int id)
        {
            var response = await Mediator.Send(new GetResolutionAuditHistoryQuery() {ResolutionId   = id });
            return NewResult(response);
        }

    }
}
