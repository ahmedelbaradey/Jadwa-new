using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using AutoMapper;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.FundManagement;
using Domain.Entities.Notifications;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Identity;

namespace Application.Features.Resolutions.Commands.Confirm
{
    /// <summary>
    /// Handler for ConfirmResolutionCommand
    /// Implements business logic for confirming resolutions waiting for confirmation
    /// Based on Sprint.md requirements (JDWA-570) and existing Resolution patterns
    /// </summary>
    public class ConfirmResolutionCommandHandler : BaseResponseHandler, ICommandHandler<ConfirmResolutionCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityServiceManager _identityService;
        #endregion

        #region Constructors
        public ConfirmResolutionCommandHandler(
            IRepositoryManager repository,
            IMapper mapper,
            ILoggerManager logger,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService,
            IIdentityServiceManager identityService)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(ConfirmResolutionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Starting ConfirmResolution operation for ID: {request.Id}");

                // 1. Validate request
                if (request == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.EmptyRequestValidation]);

                // 2. Get resolution entity with related data
                var resolution = await _repository.Resolutions.GetByIdAsync<Resolution>(request.Id, trackChanges: true);
                if (resolution == null)
                {
                    _logger.LogWarn($"Resolution not found with ID: {request.Id}");
                    return NotFound<string>(_localizer[SharedResourcesKey.ResolutionNotFound]);
                }

                // 3. Get fund information for validation and notifications
                var funddetails = await _repository.Funds.ViewFundUsers(resolution.FundId, trackChanges: false);
                if (funddetails == null)
                {
                    _logger.LogWarn($"Fund not found with ID: {resolution.FundId}");
                    return NotFound<string>(_localizer[SharedResourcesKey.FundNotFound]);
                }

                // 4. Get current user information
                var currentUserId = _currentUserService.UserId;
                var currentUserRoles = _currentUserService.Roles;

                if (!currentUserId.HasValue)
                {
                    _logger.LogWarn("Current user ID is null");
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // 5. Validate user permissions (only Fund Manager can confirm)
                if (!await HasConfirmPermission(funddetails, currentUserId.Value))
                {
                    _logger.LogWarn($"User {currentUserId.Value} does not have permission to confirm resolution {request.Id}");
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // 6. Validate resolution status (must be WaitingForConfirmation)
                if (resolution.Status != ResolutionStatusEnum.WaitingForConfirmation)
                {
                    _logger.LogError(null, $"Cannot confirm resolution {request.Id} with status {resolution.Status}");
                    return BadRequest<string>(_localizer[SharedResourcesKey.InvalidResolutionStatusForConfirmation]);
                }

                // 7. Initialize state pattern if not already initialized
                if (resolution.StateContext == null)
                {
                    resolution.InitializeState();
                }

                // 8. Get current user information for comprehensive audit trail (Sprint.md requirement)
                var currentUserName = _currentUserService.UserName ?? "Unknown User";
                var currentUserRole =  GetUserRoleForFund(funddetails, currentUserId.Value);
                var localizationKey = SharedResourcesKey.AuditActionResolutionConfirmation;

                // 9. Update resolution status to confirmed using enhanced state pattern audit with comprehensive details
                var comprehensiveDetails = $"Resolution confirmed and approved by {currentUserRole}: {currentUserName}. Status transitioned from {resolution.Status} to Confirmed. Resolution is now approved for implementation and execution. All stakeholders notified of confirmation.";
                var transitionSuccess = resolution.ChangeStatusWithAudit(
                    ResolutionStatusEnum.Confirmed,
                    ResolutionActionEnum.ResolutionConfirmation,
                    $"Resolution confirmed by fund manager ({currentUserName})",
                    localizationKey,
                    currentUserId.Value,
                    currentUserRole,
                    comprehensiveDetails);

                if (!transitionSuccess)
                {
                    _logger.LogError(null, $"Failed to transition resolution {request.Id} to Confirmed status");
                    return BadRequest<string>(_localizer[SharedResourcesKey.InvalidStatusTransition]);
                }

                // 10. Save changes
                var updateResult = await _repository.Resolutions.UpdateAsync(resolution);
                if (!updateResult)
                {
                    _logger.LogError(null, $"Failed to confirm resolution with ID: {request.Id}");
                    return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorUpdatingData]);
                }

                // 11. Send notifications to legal council and board secretary (MSG002)
                await AddNotification(funddetails, resolution, currentUserId.Value);

                _logger.LogInfo($"Resolution confirmed successfully with ID: {request.Id} by user: {currentUserName} (Role: {currentUserRole})");

                // 12. Return MSG001 success message as required by Sprint.md
                return Success<string>(_localizer[SharedResourcesKey.OperationCompletedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in ConfirmResolution for ID: {request.Id}");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorUpdatingData]);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Validates if the current user has permission to confirm the resolution
        /// Based on Sprint.md role-based access requirements - only fund managers can confirm
        /// </summary>
        private async Task<bool> HasConfirmPermission(Fund fundDetails, int currentUserId)
        {
            // Only Fund Manager can confirm resolutions
           
            return fundDetails?.FundManagers?.Any(fm => fm.UserId == currentUserId) ?? false;
        }

        /// <summary>
        /// Gets the user's role for the specific fund for audit trail logging
        /// Required by Sprint.md for action logging: action name, date, user name, role
        /// </summary>
        private string GetUserRoleForFund(Fund fund, int currentUserId)
        {
            // Since only Fund Manager can confirm, return Fund Manager role
            // This could be enhanced to check actual fund roles if needed
            return "Fund Manager";
        }

        /// <summary>
        /// Adds notifications for resolution confirmation following established pattern
        /// Based on Sprint.md MSG002 notification requirements for resolution confirmation
        /// Follows the same pattern as EditResolutionCommandHandler
        /// </summary>
        private async Task AddNotification(Fund fundDetails, Resolution resolution, int currentUserId)
        {
            try
            {
                var notifications = new List<Domain.Entities.Notifications.Notification>();

                // Get current user details
                var currentUser = await _identityService.UserManagmentService.FindByIdAsync(currentUserId.ToString());
                var currentUserName = currentUser?.FullName ?? _currentUserService.UserName ?? "Unknown User";

                // MSG002: Notify Legal Council attached to the fund
                if (fundDetails.LegalCouncilId > 0)
                {
                    notifications.Add(new Domain.Entities.Notifications.Notification
                    {
                        UserId = fundDetails.LegalCouncilId,
                        FundId = fundDetails.Id,
                        Title = _localizer[SharedResourcesKey.ResolutionConfirmedNotificationTitle],
                        Body = string.Format(_localizer[SharedResourcesKey.ResolutionConfirmedNotificationBody],
                            resolution.Code, fundDetails.Name, currentUserName),
                        NotificationType = (int)NotificationType.ResolutionConfirmed,
                        IsRead = false
                    });
                }

                // MSG002: Notify Board Secretaries attached to the fund
                var boardSecretaries = fundDetails.FundBoardSecretaries ?? new List<FundBoardSecretary>();
                foreach (var boardSecretary in boardSecretaries)
                {
                    notifications.Add(new Domain.Entities.Notifications.Notification
                    {
                        UserId = boardSecretary.UserId,
                        FundId = fundDetails.Id,
                        Title = _localizer[SharedResourcesKey.ResolutionConfirmedNotificationTitle],
                        Body = string.Format(_localizer[SharedResourcesKey.ResolutionConfirmedNotificationBody],
                            resolution.Code, fundDetails.Name, currentUserName),
                        NotificationType = (int)NotificationType.ResolutionConfirmed,
                        IsRead = false
                    });
                }

                // Save notifications using the same pattern as other handlers
                if (notifications.Any())
                {
                    await _repository.Notifications.AddRangeAsync(notifications);
                    _logger.LogInfo($"Resolution confirmation notifications added for Resolution ID: {resolution.Id}, Count: {notifications.Count}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending confirmation notifications for resolution {resolution.Id}");
                // Don't fail the main operation if notification fails
            }
        }
        #endregion
    }
}
