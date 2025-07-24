using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Catalog.Products.Dtos;

namespace Application.Features.Catalog.Products.Queries.Get
{
    public record GetQuery : IQuery<BaseResponse<SingleProductResponse>>
    {
        public int Id { get; set; }
    }
}
