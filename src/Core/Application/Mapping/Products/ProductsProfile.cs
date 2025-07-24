using AutoMapper;

namespace Application.Mapping
{
    public partial class ProductsProfile : Profile
    {
        public ProductsProfile()
        {

            AddProductMapping();
            EditProductMapping();
            GetProductMapping();
        }
    }
}
