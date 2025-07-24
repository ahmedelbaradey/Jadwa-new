using Abstraction.Base.Dto;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Features.Catalog.Products.Dtos;

namespace Application.Features.Catalog.Products.Queries.List
{
    public record ListQuery : BaseListDto, IQuery<BaseResponse<PaginatedResult<SingleProductResponse>>>
    {

    }
}
