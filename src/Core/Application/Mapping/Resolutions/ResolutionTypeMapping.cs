using Application.Features.Resolutions.Dtos;
using Domain.Entities.ResolutionManagement;

namespace Application.Mapping
{
    /// <summary>
    /// Mapping configurations for ResolutionType entity
    /// Maps between ResolutionType domain entity and ResolutionTypeDto
    /// </summary>
    public partial class ResolutionsProfile
    {
        public void ResolutionTypeMapping()
        {
            // ResolutionType to ResolutionTypeDto
            CreateMap<ResolutionType, ResolutionTypeDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.NameAr, opt => opt.MapFrom(src => src.NameAr))
                .ForMember(dest => dest.NameEn, opt => opt.MapFrom(src => src.NameEn))
                .ForMember(dest => dest.DisplayOrder, opt => opt.MapFrom(src => src.DisplayOrder))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
