using Abstraction.Contract.Repository.Notifications;
using Abstraction.Contract.Service;
using Abstraction.Contract.Service.Notifications;
using Abstraction.Contracts.Logger;
using Domain.Entities.Notifications;
using FirebaseAdmin.Messaging;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Repository.Notifications
{
    public class NotificationRepository : GenericRepository, INotificationRepository
    {
        private readonly INotificationLocalizationService? _localizationService;
        private readonly ILoggerManager _logger;

        // Constructor for dependency injection with localization service
        public NotificationRepository(
            AppDbContext repositoryContext,
            INotificationLocalizationService localizationService,
            ILoggerManager logger, ICurrentUserService currentUserService)
            : base(repositoryContext, currentUserService)
        {
            _localizationService = localizationService;
            _logger = logger;
        }
 

        public async Task<IEnumerable<Domain.Entities.Notifications.Notification>> GetPendingFundNotificationsAsync()
        {
            return await base.GetByCondition<Domain.Entities.Notifications.Notification>(c => !c.IsSent, false).Include(c=>c.User).ToListAsync();
        }

        public async Task<int> GetNotificationCountByTypeAndFundId(int fundId, int NotificationModule)
        {
            return await base.GetByCondition<Domain.Entities.Notifications.Notification>(c => c.FundId.Equals(fundId) && c.NotificationModule.Equals(NotificationModule) , false).CountAsync();
        }

        public async Task<int> GetUnReadCountByUserId(int userId, bool trackChanges)
        {
            return await GetByCondition<Domain.Entities.Notifications.Notification>(c => c.UserId.Equals(userId) && !c.IsRead, trackChanges).CountAsync();
        }

        public async Task<bool> MarkAsSentAsync(int id)
        {
            var notification = await base.GetByIdAsync<Domain.Entities.Notifications.Notification>(id, true);
            notification.IsSent = true;
            notification.SentDate = DateTime.Now;    
            return  await base.UpdateAsync(notification);
        }

        public async Task<string> SendNotification(MessageRequest message)
        {
            try
            {
                // Get localized message if localization service is available and message needs localization
                string title = message.Title;
                string body = message.Body;

                if (_localizationService != null && (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(body)))
                {
                    var localizedMessage = await _localizationService.GetLocalizedNotificationAsync(
                        message.UserId,
                        message.Type,
                        message.Parameters ?? Array.Empty<object>());

                    title = localizedMessage.Title;
                    body = localizedMessage.Body;
                }

                var _message = new Message()
                {
                    Notification = new FirebaseAdmin.Messaging.Notification
                    {
                        Title = title,
                        Body = body,
                    },
                    Token = message.DeviceToken
                };

                var messaging = FirebaseMessaging.DefaultInstance;
                var result = "";

                _logger.LogInfo("Notification sent successfully to user {UserId} with message ID {MessageId}");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification to user {UserId}");
                throw;
            }
        }

        public override Task<T> AddAsync<T>(T entity, CancellationToken cancellationToken = default)
        {
            // Set NotificationModule based on NotificationType if entity is a Notification
            if (entity is Domain.Entities.Notifications.Notification notification)
            {
                notification.NotificationModule = GetNotificationModule(notification.NotificationType);
            }

            return base.AddAsync(entity, cancellationToken);
        }

        public override Task AddRangeAsync<T>(ICollection<T> entities, CancellationToken cancellationToken = default)
        {
            // Set NotificationModule based on NotificationType for each Notification entity
            foreach (var entity in entities)
            {
                if (entity is Domain.Entities.Notifications.Notification notification)
                {
                    notification.NotificationModule = GetNotificationModule(notification.NotificationType);
                }
            }

            return base.AddRangeAsync(entities, cancellationToken);
        }

        /// <summary>
        /// Maps NotificationType to appropriate NotificationModule
        /// </summary>
        /// <param name="notificationType">The notification type</param>
        /// <returns>The corresponding notification module</returns>
        private int GetNotificationModule(int notificationType)
        {
            var type = (NotificationType)notificationType;

            return type switch
            {
                // Fund-related notifications
                NotificationType.AddedToFund => (int)NotificationModule.Funds,
                NotificationType.RemoveFromFund => (int)NotificationModule.Funds,
                NotificationType.ChangeExitDate => (int)NotificationModule.Funds,
                NotificationType.CompeleteFund => (int)NotificationModule.Funds,
                NotificationType.FundActivated => (int)NotificationModule.Funds,
                NotificationType.AddedToFundForManager => (int)NotificationModule.Funds,

                // Board Member-related notifications
                NotificationType.BoardMemberAdded => (int)NotificationModule.Members,
                NotificationType.BoardMemberAddedToFund => (int)NotificationModule.Members,

                // Resolution-related notifications
                NotificationType.ResolutionCreated => (int)NotificationModule.Resolutions,
                NotificationType.ResolutionUpdated => (int)NotificationModule.Resolutions,
                NotificationType.ResolutionCancelled => (int)NotificationModule.Resolutions,
                NotificationType.ResolutionConfirmed => (int)NotificationModule.Resolutions,
                NotificationType.ResolutionRejected => (int)NotificationModule.Resolutions,
                NotificationType.ResolutionSentToVote => (int)NotificationModule.Resolutions,
                NotificationType.NewResolutionCreatedFromApproved => (int)NotificationModule.Resolutions,
                NotificationType.ResolutionVotingSuspended => (int)NotificationModule.Resolutions,
                NotificationType.ResolutionDataCompleted => (int)NotificationModule.Resolutions,

                // Default to Funds module for unknown types
                _ => (int)NotificationModule.Funds
            };
        }

    }
}
