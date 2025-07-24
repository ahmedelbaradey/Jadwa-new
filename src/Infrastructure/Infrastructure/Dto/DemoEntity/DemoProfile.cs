using AutoMapper;

namespace Infrastructure.Dto.DemoEntity
{
    public partial class DemoProfile : Profile
    {
        public DemoProfile()
        {
            CreateMap<Domain.Entities.Products.DemoEntity, DemoEntityDto>().ReverseMap();
        }
    }
}
