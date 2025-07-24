using Abstraction.Base.Dto;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Features.Catalog.Categories.Dtos;

namespace Application.Features.Catalog.Categories.Queries.List
{
    public record ListQuery : BaseListDto, IQuery<BaseResponse<PaginatedResult<SingleCategoryResponse>>>
    {

    }
}
