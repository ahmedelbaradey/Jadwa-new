using Application.Features.Catalog.Categories.Dtos;
using Domain.Entities.Products;

namespace Application.Mapping
{
    public partial class CategoriesProfile
    {
        public void GetCategroyMapping()
        {
            CreateMap<Category, SingleCategoryResponse>();
        }
    }
}
