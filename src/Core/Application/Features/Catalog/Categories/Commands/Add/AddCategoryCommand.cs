using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Catalog.Categories.Dtos;
 

namespace Application.Features.Catalog.Categories.Commands.Add
{

    public record AddCategoryCommand : AddCategoryDto, ICommand<BaseResponse<string>>
    {

    }
}
