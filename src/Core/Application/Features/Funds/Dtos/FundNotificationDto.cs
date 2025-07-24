using Abstraction.Base.Dto;
using Domain.Entities.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Funds.Dtos
{
    public record class FundNotificationDto : BaseDto
    {
        public int UserId { get; set; }
        public int NotificationType { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Message { get; set; }
        public NotificationModule ModuleId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int NotificationModule { get; set; } // Assuming NotificationModule is an enum, you can change this to the appropriate type
    }
}
