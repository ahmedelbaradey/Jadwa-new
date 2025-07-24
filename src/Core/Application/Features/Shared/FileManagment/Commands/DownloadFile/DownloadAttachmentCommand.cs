using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Application.Features.Shared.FileManagment.Dtos;

namespace Application.Features.Shared.FileManagment.Commands.NewFolder
{
    public record DownloadAttachmentCommand : DownloadAttachment, ICommand<BaseResponse<DownloadAttachmentDTO>>
    {
    }
}
