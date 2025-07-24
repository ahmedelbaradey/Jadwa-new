using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Catalog.Products.Dtos;

namespace Application.Features.Catalog.Products.Commands.Edit
{

    public record EditProductCommand : EditProductDto, ICommand<BaseResponse<string>>
    {

    }
}
