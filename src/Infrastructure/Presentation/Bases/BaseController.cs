using Abstraction.Base.Dto;
using Abstraction.Base.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Abstraction.Contracts.Service;


namespace Presentation.Bases
{
    [ApiController]
   // [Authorize]
    [Produces("application/json")]
    [Consumes("application/json")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public class BaseController : ControllerBase  
    {


        public ObjectResult NewResult<T>(BaseResponse<T> response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    return new OkObjectResult(response);
                case HttpStatusCode.Created:
                    return new OkObjectResult(response);
                case HttpStatusCode.Accepted:
                    return new AcceptedResult();
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(response);
                case HttpStatusCode.Unauthorized:
                    return new UnauthorizedObjectResult(response);
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(response);
                case HttpStatusCode.Conflict:
                    return new ConflictObjectResult(response);
                case HttpStatusCode.InternalServerError:
                    return new ObjectResult(response) { StatusCode = StatusCodes.Status500InternalServerError };
                default:
                    return new ObjectResult(response) { StatusCode = (int)response.StatusCode };
            }
        }

    }
}
