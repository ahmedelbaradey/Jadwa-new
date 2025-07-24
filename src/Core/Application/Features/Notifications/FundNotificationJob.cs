using Abstraction.Constants;
using Abstraction.Contracts.Repository;
using Domain.Entities.Notifications;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Application.Features.Notification
{
    public class FundNotificationJob : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromSeconds(30); // Run every 5 minutes

        public FundNotificationJob(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var repository = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
                    var UserManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var observable = new FundNotificationObservable();
                    var observer = new FundNotificationObserver(repository);
                    observable.Subscribe(observer);

                    // Example: Fetch pending notifications from DB or any source
                    var pendingNotifications = await repository.Notifications.GetPendingFundNotificationsAsync();

                    foreach (var notification in pendingNotifications)
                    {
                       var userClaims = await UserManager.GetClaimsAsync(notification.User);
                       var FCMWebToken = userClaims
                            .Where(c => c.Type == CustomClaimTypes.FCMWebToken)
                            .Select(c => c.Value)
                            .SingleOrDefault();
                        if(!string.IsNullOrWhiteSpace(FCMWebToken))
                        {
                            // Use the new localized notification method
                            // Parse parameters based on notification type
                            var parameters = ParseNotificationParameters(notification.Body, (NotificationType)notification.NotificationType);

                            await observable.NotifyAsync(
                                notification.UserId,
                                FCMWebToken,
                                (NotificationType)notification.NotificationType,
                                parameters);
                            await repository.Notifications.MarkAsSentAsync(notification.Id);
                        }
                        
                    }
                }
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private static object[] ParseNotificationParameters(string body, NotificationType notificationType)
        {
            if (string.IsNullOrEmpty(body))
                return Array.Empty<object>();
            return body.Contains('|') ? body.Split('|') : new object[] { body };
        }
    }
}
