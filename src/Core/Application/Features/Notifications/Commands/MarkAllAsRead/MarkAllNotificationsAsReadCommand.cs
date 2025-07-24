using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Notifications.Commands.MarkAllAsRead
{
    /// <summary>
    /// Command to mark all notifications as read for the current user
    /// </summary>
    public record MarkAllNotificationsAsReadCommand : ICommand<BaseResponse<string>>
    {
        // No properties needed as it operates on current user's notifications
    }
}
