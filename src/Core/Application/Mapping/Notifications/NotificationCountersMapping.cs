using Application.Features.Notifications.Dtos;
using AutoMapper;
using Domain.Entities.Notifications;
using Domain.Entities.FundManagement;
using System.Linq;

namespace Application.Mapping
{
    /// <summary>
    /// AutoMapper configuration for Notification Counters mapping
    /// Handles entity-to-DTO transformations for notification counter data
    /// Based on JDWA-996 requirements and existing notification mapping patterns
    /// Follows Clean Architecture principles with proper separation of concerns
    /// </summary>
    public partial class NotificationsProfile
    {
        /// <summary>
        /// Configures mapping for Notification Counters
        /// Maps Notification entities to NotificationCountersDto with computed properties
        /// Includes role-based filtering and categorization by notification type
        /// </summary>
        public void NotificationCountersMapping()
        {

            // Enhanced NotificationDto mapping with additional properties for counters
            CreateMap<Notification, NotificationDto>()
                .ForMember(dest => dest.IsRead, opt => opt.MapFrom(src => src.IsRead))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Body, opt => opt.MapFrom(src => src.Body))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}
