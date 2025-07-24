using AutoMapper;

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper profile for Resolution entity mappings
    /// Contains all mapping configurations for Resolution-related DTOs
    /// </summary>
    public partial class ResolutionsProfile : Profile
    {
        public ResolutionsProfile()
        {
            GetResolutionMapping();
            CreateResolutionMapping();
            EditResolutionMapping();
            ResolutionItemMapping();
            ResolutionTypeMapping();
        }
    }
}
