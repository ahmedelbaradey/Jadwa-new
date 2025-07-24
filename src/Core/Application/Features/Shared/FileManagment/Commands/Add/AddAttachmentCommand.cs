using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Application.Features.Shared.FileManagment.Dtos;


namespace Application.Features.Shared.FileManagment.Commands.Add
{

    public record AddAttachmentCommand : AddAttachment, ICommand<BaseResponse<AttachmentDTO>>
    {

    }
}
