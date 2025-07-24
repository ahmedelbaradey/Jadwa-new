using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Notifications;

namespace Abstraction.Contract.Repository.Notifications
{
    public interface IFundNotificationObserver
    {
        Task OnSendNotification(MessageRequest message);
    }
}
