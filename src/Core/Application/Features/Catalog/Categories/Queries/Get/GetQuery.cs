using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Catalog.Categories.Dtos;


namespace Application.Features.Catalog.Categories.Queries.Get
{
    public record GetQuery : IQuery<BaseResponse<SingleCategoryResponse>>
    {
        public int Id { get; set; }
    }
}
