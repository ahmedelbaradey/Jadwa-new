using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Funds.Dtos;

namespace Application.Features.Funds.Commands.Add
{

    public record AddFundCommand : AddFundRequest, ICommand<BaseResponse<string>>
    {

    }
}
