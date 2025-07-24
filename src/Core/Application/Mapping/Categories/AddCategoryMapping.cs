using Application.Features.Catalog.Categories.Commands.Add;
using Domain.Entities.Products;

namespace Application.Mapping
{
    public partial class CategoriesProfile
    {
        public void AddCategoryMapping()
        {
            CreateMap<AddCategoryCommand, Category>();
        }
    }
}
