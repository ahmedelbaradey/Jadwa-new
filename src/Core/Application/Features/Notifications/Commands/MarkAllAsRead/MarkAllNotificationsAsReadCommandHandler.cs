using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Abstraction.Contract.Service;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Notifications.Commands.MarkAllAsRead
{
    /// <summary>
    /// Handler for marking all notifications as read for the current user
    /// Efficiently updates all unread notifications in a single operation
    /// </summary>
    public class MarkAllNotificationsAsReadCommandHandler : BaseResponseHandler, ICommandHandler<MarkAllNotificationsAsReadCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public MarkAllNotificationsAsReadCommandHandler(
            IRepositoryManager repository,
            ILoggerManager logger,
            ICurrentUserService currentUserService,
            IStringLocalizer<SharedResources> localizer)
        {
            _repository = repository;
            _logger = logger;
            _currentUserService = currentUserService;
            _localizer = localizer;
        }
        #endregion

        #region Handler
        public async Task<BaseResponse<string>> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Marking all notifications as read for user {_currentUserService.UserId}");

                // Get current user ID
                var currentUserId = _currentUserService.UserId;
                if (!currentUserId.HasValue)
                {
                    _logger.LogWarn("No authenticated user found");
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // Get all unread notifications for the current user
                var unreadNotifications = _repository.Notifications
                    .GetByCondition<Domain.Entities.Notifications.Notification>(
                        n => n.UserId == currentUserId.Value && !n.IsRead,
                        true);

                if (!unreadNotifications.Any())
                {
                    _logger.LogInfo($"No unread notifications found for user {currentUserId.Value}");
                    return Success<string>(_localizer[SharedResourcesKey.OperationCompletedSuccessfully]);
                }

                // Mark all as read
                foreach (var notification in unreadNotifications)
                {
                    notification.IsRead = true;
                }

                // Update all notifications
                await _repository.Notifications.UpdateRangeAsync(unreadNotifications.ToList());

                _logger.LogInfo($"Successfully marked {unreadNotifications.ToList().Count} notifications as read for user {currentUserId.Value}");
                return Success<string>(_localizer[SharedResourcesKey.OperationCompletedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking all notifications as read for user {_currentUserService.UserId}");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        #endregion
    }
}
