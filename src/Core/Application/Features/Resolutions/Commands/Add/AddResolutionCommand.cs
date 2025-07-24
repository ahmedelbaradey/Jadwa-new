using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Resolutions.Dtos;

namespace Application.Features.Resolutions.Commands.Add
{

    public record AddResolutionCommand : AddResolutionDto, ICommand<BaseResponse<string>>
    {

    }
}
