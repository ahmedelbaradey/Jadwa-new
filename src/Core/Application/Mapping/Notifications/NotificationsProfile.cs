using AutoMapper;

namespace Application.Mapping
{
    public partial class NotificationsProfile : Profile
    {
        public NotificationsProfile()
        {
            NotificationMapping();
            NotificationCountersMapping();
        }
    }
}
