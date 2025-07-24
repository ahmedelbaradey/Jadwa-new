using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Funds.Dtos;

namespace Application.Features.Funds.Commands.Edit
{

    public record EditExitDateCommand : ExistDateRequest, ICommand<BaseResponse<string>>
    {

    }
}
