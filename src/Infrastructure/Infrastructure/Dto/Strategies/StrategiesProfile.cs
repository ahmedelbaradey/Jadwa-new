using AutoMapper;
using Domain.Entities.Startegies;

namespace Infrastructure.Dto.Strategies
{
    public partial class StrategiesProfile : Profile
    {
        public StrategiesProfile()
        {
            CreateMap<Strategy, StrategyDto>().ReverseMap();
            CreateMap<Strategy, StrategyEditDto>().ReverseMap();
        }
    }
}
