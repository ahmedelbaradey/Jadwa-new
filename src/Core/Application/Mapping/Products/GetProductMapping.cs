using Application.Features.Catalog.Products.Dtos;
using Domain.Entities.Products;

namespace Application.Mapping
{
    public partial class ProductsProfile
    {
        public void GetProductMapping()
        {
            CreateMap<Product, SingleProductResponse>();
        }
    }
}
