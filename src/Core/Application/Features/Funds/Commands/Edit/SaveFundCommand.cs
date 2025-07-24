using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Funds.Dtos;

namespace Application.Features.Funds.Commands.Edit
{

    public record SaveFundCommand : EditFundRequest, ICommand<BaseResponse<string>>
    {

    }
}
