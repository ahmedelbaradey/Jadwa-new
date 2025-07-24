using Abstraction.Base.Dto;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
 

namespace Application.Features.Resolutions.Commands.Delete
{

    public record DeleteResolutionCommand : BaseDto, ICommand<BaseResponse<string>>
    {

    }
}
