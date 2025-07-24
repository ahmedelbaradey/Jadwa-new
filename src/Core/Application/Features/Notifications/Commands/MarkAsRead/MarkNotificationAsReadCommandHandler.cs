using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Abstraction.Contract.Service;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Notifications.Commands.MarkAsRead
{
    /// <summary>
    /// Handler for marking a single notification as read
    /// Implements authorization to ensure users can only mark their own notifications as read
    /// </summary>
    public class MarkNotificationAsReadCommandHandler : BaseResponseHandler, ICommandHandler<MarkNotificationAsReadCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly ICurrentUserService _currentUserService;
        private readonly IStringLocalizer<SharedResources> _localizer;
        #endregion

        #region Constructor
        public MarkNotificationAsReadCommandHandler(
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
        public async Task<BaseResponse<string>> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Marking notification {request.NotificationId} as read for user {_currentUserService.UserId}");

                // Get current user ID
                var currentUserId = _currentUserService.UserId;
                if (!currentUserId.HasValue)
                {
                    _logger.LogWarn("No authenticated user found");
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // Get the notification
                var notification = await _repository.Notifications.GetByIdAsync<Domain.Entities.Notifications.Notification>(request.NotificationId, true);
                if (notification == null)
                {
                    _logger.LogWarn($"Notification with ID {request.NotificationId} not found");
                    return NotFound<string>("Notification not Found");
                }

                // Authorization check - users can only mark their own notifications as read
                if (notification.UserId != currentUserId.Value)
                {
                    _logger.LogWarn($"User {currentUserId.Value} attempted to mark notification {request.NotificationId} belonging to user {notification.UserId}");
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // Check if already read
                if (notification.IsRead)
                {
                    _logger.LogInfo($"Notification {request.NotificationId} is already marked as read");
                    return Success<string>(_localizer[SharedResourcesKey.OperationCompletedSuccessfully]);
                }

                // Mark as read
                notification.IsRead = true;

                var result = await _repository.Notifications.UpdateAsync(notification);
                if (!result)
                {
                    _logger.LogWarn($"Failed to update notification {request.NotificationId}");
                    return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
                }

                _logger.LogInfo($"Successfully marked notification {request.NotificationId} as read for user {currentUserId.Value}");
                return Success<string>(_localizer[SharedResourcesKey.OperationCompletedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error marking notification {request.NotificationId} as read");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        #endregion
    }
}
