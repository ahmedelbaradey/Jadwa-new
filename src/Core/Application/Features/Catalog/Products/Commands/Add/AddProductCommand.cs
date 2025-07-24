using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Catalog.Products.Dtos;

namespace Application.Features.Catalog.Products.Commands.Add
{

    public record AddProductCommand : AddProductDto, ICommand<BaseResponse<string>>
    {

    }
}
