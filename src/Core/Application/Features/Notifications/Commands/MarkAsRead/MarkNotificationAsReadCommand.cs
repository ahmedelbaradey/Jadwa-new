using Application.Base.Abstracts;
using Abstraction.Base.Response;

namespace Application.Features.Notifications.Commands.MarkAsRead
{
    /// <summary>
    /// Command to mark a single notification as read
    /// </summary>
    public record MarkNotificationAsReadCommand : ICommand<BaseResponse<string>>
    {
        /// <summary>
        /// ID of the notification to mark as read
        /// </summary>
        public int NotificationId { get; set; }
    }
}
