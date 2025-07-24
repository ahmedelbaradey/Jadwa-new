using Application.Features.Catalog.Products.Commands.Add;
using Application.Features.Notifications.Dtos;
using AutoMapper;
using Domain.Entities.Notifications;
using Domain.Entities.Products;

namespace Application.Mapping
{
    public partial class NotificationsProfile
    {
        public void NotificationMapping()
        {
            CreateMap<Notification, NotificationDto>();

        }
    }
}
