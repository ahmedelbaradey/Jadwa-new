using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Features.Funds.Commands.Edit;
using Application.Features.Funds.Commands.Add;
using Application.Features.Funds.Queries.List;
using Application.Features.Funds.Queries.Get;
using Presentation.Bases;
using Abstraction.Constants.ModulePermissions;
using Abstraction.Base.Response;
using Microsoft.AspNetCore.Http;
using Abstraction.Common.Wappers;
using Application.Features.Funds.Dtos;
using Application.Features.Funds.Queries.GetFundVoteType;
using Application.Features.Funds.Queries.GetFundBasicInfo;


namespace Presentation.Controllers
{
  //  [Route("api/Funds/[Action]")]
    [Route("api/[controller]")]
    [ApiController]
    public class FundsController : AppControllerBase
    {
       // [Authorize(Policy = FundManagerPermission.List)]
        [HttpGet("FundsList")]
        [ProducesResponseType(typeof(PaginatedResult<FundGroupDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFundsPaginatedList([FromQuery] ListQuery query)
        {
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        // [Authorize(Policy = FundManagerPermission.View)]
        [HttpGet("GetFundsById")]
        [ProducesResponseType(typeof(BaseResponse<GetFundResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFundsById(int id)
        {
            var response = await Mediator.Send(new GetQuery() { Id = id });
            return NewResult(response);
        }

       // [Authorize(Policy = FundManagerPermission.Create)]
        [HttpPost("AddFund")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> AddFund([FromBody] AddFundCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

       // [Authorize(Policy = LegalCouncilPermission.Edit)]
        [HttpPut("EditFund")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditFund([FromBody] SaveFundCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }



        // [Authorize(Policy = FundManagerPermission.View)]
        [HttpGet("GetFunDetails")]
        [ProducesResponseType(typeof(BaseResponse<FundDetailsResponse>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await Mediator.Send(new ViewQuery() { Id = id });
            return NewResult(response);
        }

       // [Authorize(Policy = LegalCouncilPermission.EditFundExitdate)]
        [HttpPut("EditFundExitDate")]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        public async Task<IActionResult> EditFundExitdate([FromBody] EditExitDateCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
         

        /// <summary>
        /// Gets fund vote type by fund ID
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>Fund vote type information</returns>
        [HttpGet("VoteType/{fundId}")]
        [ProducesResponseType(typeof(BaseResponse<FundVoteTypeDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFundVoteType(int fundId)
        {
            var query = new GetFundVoteTypeQuery(fundId);
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        /// <summary>
        /// Gets fund basic information (initiation date and voting type) by fund ID
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>Fund basic information including initiation date and voting type</returns>
        [HttpGet("BasicInfo/{fundId}")]
        [ProducesResponseType(typeof(BaseResponse<FundBasicInfoDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFundBasicInfo(int fundId)
        {
            var query = new GetFundBasicInfoQuery() { FundId = fundId };
            var response = await Mediator.Send(query);
            return NewResult(response);
        }

        /// <summary>
        /// Gets fund Notification by fund ID
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>Fund basic information including initiation date and voting type</returns>
        [HttpGet("getAllFundNotification/{fundId}")]
        [ProducesResponseType(typeof(BaseResponse<List<FundNotificationDto>>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFundNotification(int fundId , int pageSize , int pageNumber)
        {
            var query = new GetFundNotificationQuery() { FundId = fundId , PageNumber = pageNumber,PageSize = pageSize };
            var response = await Mediator.Send(query);
            return NewResult(response);
        }
    }
}
