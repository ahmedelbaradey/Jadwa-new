using Abstraction.Base.Dto;

namespace Application.Features.Catalog.Products.Dtos
{
    public record ProductDto : BaseDto
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
    }
}
