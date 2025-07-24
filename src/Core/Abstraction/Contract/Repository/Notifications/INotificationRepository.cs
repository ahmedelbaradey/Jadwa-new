using Abstraction.Contracts.Repository;
using Domain.Entities.Notifications;

namespace Abstraction.Contract.Repository.Notifications
{
    public interface INotificationRepository : IGenericRepository
    {
        Task<IEnumerable<Notification>> GetPendingFundNotificationsAsync();
        Task<bool> MarkAsSentAsync(int id);
        Task<string> SendNotification(MessageRequest message);
        Task<int> GetNotificationCountByTypeAndFundId(int fundId, int NotificationModule);
        Task<int> GetUnReadCountByUserId(int userId ,bool trackChanges);
    }
}
