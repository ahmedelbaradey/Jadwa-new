using Application.Common.Helpers;
using Application.Features.Funds.Dtos;
using Domain.Entities.FundManagement;
using Domain.Entities.Notifications;

namespace Application.Mapping
{
    public partial class FundsProfile
    {
        public void GetFundDetailsMapping()
        {
            CreateMap<Fund, FundDetailsResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => AppLang.IsArabic ? src.Status.NameAr :  src.Status.NameEn))
                .ForMember(dest => dest.ResolutionsCount, opt => opt.MapFrom(src => src.Resolutions.Count()))
                .ForMember(dest => dest.MembersCount, opt => opt.MapFrom(src => src.BoardMembers.Count()))
               // .ForMember(dest => dest.ResolutionsNotificationCount, opt => opt.MapFrom(src => src.Notifications.Where(c =>   c.NotificationModule == (int)NotificationModule.Resolutions).Count()))
               // .ForMember(dest => dest.MembersNotificationCount, opt => opt.MapFrom(src => src.Notifications.Where(c =>   c.NotificationModule == (int)NotificationModule.Members).Count()))
                .ForMember(dest => dest.DocumentsCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.EvaluationsCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.MeetingsCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.MeetingsNotificationCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.EvaluationsNotificationCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.DocumentsNotificationCount, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.StrategyName, opt => opt.MapFrom(src => AppLang.IsArabic ?  src.Strategy.NameAr : src.Strategy.NameEn))
                .ForMember(dest => dest.FundHistory, opt => opt.MapFrom(src => src.FundStatusHistories))
                .ForMember(dest => dest.FundNotifications, opt => opt.MapFrom(src => src.Notifications));


            CreateMap<FundStatusHistory, FundHistoryDto>()
               .ForMember(dest => dest.StatusId, opt => opt.MapFrom(src => src.StatusHistoryId))
               .ForMember(dest => dest.StatusName, opt => opt.MapFrom(src => AppLang.IsArabic ? src.StatusHistory.NameAr : src.StatusHistory.NameEn))
               .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.CreatedBy))
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.CreatedByUser.FullName));


            CreateMap<Notification, FundNotificationDto>()
              .ForMember(dest => dest.Message, opt => opt.MapFrom(src => src.Body))
              .ForMember(dest => dest.ModuleId, opt => opt.MapFrom(src => src.NotificationModule));
        }

    }
}
