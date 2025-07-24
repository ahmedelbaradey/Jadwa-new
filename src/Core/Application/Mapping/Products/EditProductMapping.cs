using Application.Features.Catalog.Products.Commands.Edit;
using Domain.Entities.Products;

namespace Application.Mapping
{
    public partial class ProductsProfile
    {
        public void EditProductMapping()
        {
            CreateMap<EditProductCommand, Product>(); 
        }
    }
}
