
namespace Application.Features.Catalog.Products.Dtos
{
    public record AddProductDto : ProductDto
    {
        public string Description { get; set; }
        public int CategoryId { get; set; }
    }
}
