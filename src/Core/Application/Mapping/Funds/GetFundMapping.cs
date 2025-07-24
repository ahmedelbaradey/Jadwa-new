using Application.Common.Helpers;
using Application.Features.Funds.Dtos;
using Domain.Entities.FundManagement;

namespace Application.Mapping
{
    public partial class FundsProfile
    {
        public void GetFundMapping()
        {
            CreateMap<Fund, SingleFundResponse>()
                .ForMember(dest => dest.StrategyName, opt => opt.MapFrom(src => AppLang.IsArabic ? src.Strategy.NameAr : src.Strategy.NameEn))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppLang.IsArabic ? src.Status.NameAr : src.Status.NameEn))
                .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.Status.Id));

            CreateMap<Fund, GetFundResponse>()
                .ForMember(dest => dest.AttachmentPath, opt => opt.Ignore()) // Will be populated with preview URL in handler
                .ForMember(dest => dest.AttachmentName, opt => opt.MapFrom(src => src.Attachment.FileName))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppLang.IsArabic ? src.Status.NameAr : src.Status.NameEn))
                .ForMember(dest => dest.StrategyName, opt => opt.MapFrom(src => AppLang.IsArabic ? src.Strategy.NameAr : src.Strategy.NameEn))
                .ForMember(dest => dest.FundManagers, opt => opt.MapFrom(src => src.FundManagers.Where(c => c.IsDeleted != true).Select(c => c.UserId)))
                .ForMember(dest => dest.FundBoardSecretaries, opt => opt.MapFrom(src => src.FundBoardSecretaries.Where(c => c.IsDeleted != true).Select(c => c.UserId)));
        }
    }
}
