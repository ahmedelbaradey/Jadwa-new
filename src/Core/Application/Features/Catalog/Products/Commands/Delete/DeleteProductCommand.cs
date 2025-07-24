using Abstraction.Base.Dto;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
 

namespace Application.Features.Catalog.Products.Commands.Delete
{

    public record DeleteProductCommand : BaseDto, ICommand<BaseResponse<string>>
    {

    }
}
