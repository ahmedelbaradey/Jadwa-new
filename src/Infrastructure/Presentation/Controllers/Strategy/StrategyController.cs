using Abstraction.Base.Dto;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Abstraction.Contract.Service;
using Infrastructure.Dto.Strategies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Bases;

namespace Presentation.Controllers
{
    [Route("api/Strategies/[action]")]
    [ApiController]
    public class StrategyController : BaseController
    {

        IStrategyService _strategyService;
        public StrategyController(IStrategyService strategyService)
        {
            _strategyService = strategyService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status422UnprocessableEntity)]
        public virtual async Task<IActionResult> CreateStrategy([FromBody] StrategyDto entity)
        {
            var returnValue = await _strategyService.AddAsync(entity);
            return NewResult(returnValue);
        }

        [HttpPut]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse<string>), StatusCodes.Status422UnprocessableEntity)]
        public virtual async Task<IActionResult> UpdateStrategy([FromBody] StrategyEditDto entity)
        {
            var returnValue = await _strategyService.UpdateAsync(entity);
            return NewResult(returnValue);
        }
        [HttpGet]
        [ProducesResponseType(typeof(BaseResponse<StrategyDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStrategyById(int id)
        {
            var returnValue = await _strategyService.GetByIdAsync<StrategyDto>(id, false);
            return NewResult(returnValue);
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedResult<StrategyDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> StrategyList([FromQuery] BaseListDto query)
        {
            var returnValue = await _strategyService.GetAllPagedAsync<StrategyDto>(query.PageNumber, query.PageSize, query.OrderBy, false);
            return NewResult(returnValue);
        }
    }
}
