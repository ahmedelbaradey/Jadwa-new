using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Resolutions.Dtos;

namespace Application.Features.Resolutions.Commands.Edit
{

    public record EditResolutionCommand : EditResolutionDto, ICommand<BaseResponse<string>>
    {

    }
}
