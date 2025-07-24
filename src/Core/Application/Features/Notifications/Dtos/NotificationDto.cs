using Domain.Entities.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Notifications.Dtos
{
    public class NotificationDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int NotificationType { get; set; }
        public string Message { get; set; }
        public NotificationModule ModuleId { get; set; }
        public int NotificationModule { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
