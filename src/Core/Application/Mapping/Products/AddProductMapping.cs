using Application.Features.Catalog.Products.Commands.Add;
using Domain.Entities.Products;

namespace Application.Mapping
{
    public partial class ProductsProfile
    {
        public void AddProductMapping()
        {
            CreateMap<AddProductCommand, Product>();
        }
    }
}
