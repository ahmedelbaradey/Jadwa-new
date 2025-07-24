using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Identity.Authorizations.Commands.AddRole;
using Application.Features.Identity.Authorizations.Commands.EditRole;
using Application.Features.Identity.Authorizations.Queries.GetRoleList;
using Application.Features.Identity.Authorizations.Queries.GetRoleById;
using Application.Features.Identity.Authorizations.Commands.DeleteRole;
using Presentation.Bases;
using Application.Features.Identity.Authorizations.Queries.GetClaimList;
using Abstraction.Base.Response;
using Microsoft.AspNetCore.Http;
using Application.Features.Identity.Authorizations.Queries.Responses;

namespace Identity.Presentation.Controllers
{
    [Route("api/Users/[controller]")]
    [ApiController]
  
    public class AuthorzationController : AppControllerBase
    {
        [HttpGet("RoleList")]
        [ProducesResponseType(typeof(BaseResponse<List<GetRoleListResponse>>), StatusCodes.Status201Created)]

        public async Task<IActionResult> GetRoleList()
        {
            var response = await Mediator.Send(new GetRoleListQuery());
            return NewResult(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleById(int id)
        {
            var response = await Mediator.Send(new GetRoleByIdQuery { Id=id});
            return NewResult(response);
        }

        [HttpGet("CalimList")]
        public async Task<IActionResult> ClaimList()
        {
            var response = await Mediator.Send(new GetClaimListQuery());
            return NewResult(response);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateRole([FromBody] AddRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> EditRole([FromBody] EditRoleCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var response = await Mediator.Send(new DeleteRoleCommand { Id=id});
            return NewResult(response);
        }

       
    }
}
