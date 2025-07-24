using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Base;
using Domain.Entities.Users;

namespace Domain.Entities.Notifications
{
    public class Notification : CreationAuditedEntity
    {
        public int UserId { get; set; }
        public int FundId { get; set; }
        public DateTime SentDate { get; set; }
        public bool IsRead { get; set; } = false;
        public bool IsSent { get; set; } = false; // Assuming this is a boolean, adjust as necessary for your use case
        public string Title { get; set; }
        public string Body { get; set; }
        [NotMapped]
        public string DeviceToken { get; set; } // Assuming this is a string, adjust as necessary for your use case
        public int NotificationType { get; set; } // Assuming NotificationType is an enum, you can change this to the appropriate type
        public int NotificationModule { get; set; } // Assuming NotificationModule is an enum, you can change this to the appropriate type

        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
    }
}
