using Application.Features.Shared.FileManagment.Commands.Add;
using Application.Features.Shared.FileManagment.Commands.NewFolder;
using Microsoft.AspNetCore.Mvc;
using Presentation.Bases;

namespace Presentation.Controllers.Shared
{
    [Route("api/Users/[controller]")]
    [ApiController]
    //[Authorize]
    public class FileManagmentController : AppControllerBase
    {
        [Consumes("multipart/form-data")]
        [HttpPost("UploadFile")]
        public async Task<IActionResult> UploadFilepublic(AddAttachmentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }

        [HttpPost("DownloadFile")]
        public async Task<IActionResult> DownloadFilepublic(DownloadAttachmentCommand command)
        {
            var response = await Mediator.Send(command);
            return NewResult(response);
        }
    }
}
