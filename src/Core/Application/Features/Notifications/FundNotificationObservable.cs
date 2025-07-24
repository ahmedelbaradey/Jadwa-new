using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abstraction.Contract.Repository.Notifications;
using Domain.Entities.Notifications;

namespace Application.Features.Notification
{
    public class FundNotificationObservable
    {
        private readonly List<IFundNotificationObserver> _observers = new();

        public void Subscribe(IFundNotificationObserver observer)
        {
            _observers.Add(observer);
        }

        
        // New method for localized notifications
        public async Task NotifyAsync(int userId, string deviceToken, NotificationType notificationType, params object[] parameters)
        {
            foreach (var observer in _observers)
            {
                await observer.OnSendNotification(new MessageRequest
                {
                    UserId = userId,
                    DeviceToken = deviceToken,
                    Type = notificationType,
                    Parameters = parameters,
                    Title = string.Empty, // Will be localized by the service
                    Body = string.Empty   // Will be localized by the service
                });
            }
        }

        // Backward compatibility method
        public async Task NotifyAsync(string title, string body, string deviceToken, NotificationType notificationType)
        {
            foreach (var observer in _observers)
            {
                await observer.OnSendNotification(new MessageRequest
                {
                    Body = body,
                    DeviceToken = deviceToken,
                    Title = title,
                    Type = notificationType,
                    UserId = 0 // Default value for backward compatibility
                });
            }
        }
    }
}
