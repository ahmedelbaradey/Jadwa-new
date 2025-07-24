using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using AutoMapper;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.FundManagement;
using Domain.Services;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Domain.Entities.Notifications;


namespace Application.Features.Resolutions.Commands.Cancel
{
    /// <summary>
    /// Handler for CancelResolutionCommand
    /// Implements business logic for cancelling pending resolutions with notifications
    /// Based on Sprint.md requirements (JDWA-508) and existing Resolution patterns
    /// </summary>
    public class CancelResolutionCommandHandler : BaseResponseHandler, ICommandHandler<CancelResolutionCommand, BaseResponse<string>>
    {

        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public CancelResolutionCommandHandler(
            IRepositoryManager repository, 
            IMapper mapper, 
            ILoggerManager logger,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(CancelResolutionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Starting CancelResolution operation for ID: {request.Id}");

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

                // 3. Validate business rules using domain service
                if (!ResolutionDomainService.CanCancelResolution(resolution.Status))
                {
                    _logger.LogWarn($"Cannot cancel resolution with status: {resolution.Status} for ID: {request.Id}");
                    return BadRequest<string>(_localizer[SharedResourcesKey.CannotCancelNonPendingResolution]);
                }

                // 4. Get fund for access control validation and notification
                var fundDetails = await _repository.Funds.ViewFundUsers(resolution.FundId, trackChanges: false);

                if (fundDetails == null)
                {
                    _logger.LogWarn($"Fund not found for resolution ID: {request.Id}");
                    return NotFound<string>(_localizer[SharedResourcesKey.FundNotFound]);
                }

                // 5. Validate user permissions (only fund managers can cancel resolutions)
                var currentUserId = _currentUserService.UserId;
                if (!await HasCancelPermission(fundDetails, currentUserId))
                {
                    _logger.LogWarn($"User {currentUserId} does not have permission to cancel resolution {request.Id}");
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // 6. Initialize state pattern if not already initialized
                if (resolution.StateContext == null)
                {
                    resolution.InitializeState();
                }

                // 7. Validate if cancellation is allowed using state pattern
                if (!resolution.CanCancel())
                {
                    _logger.LogError(null, $"Cannot cancel resolution {request.Id} with status {resolution.Status}");
                    return BadRequest<string>(_localizer[SharedResourcesKey.CannotCancelInCurrentState]);
                }

                // 8. Get user context for comprehensive audit logging
                var currentUserName = _currentUserService.UserName ?? "Unknown User";
                var currentUserRole = "Fund Manager"; // Only fund managers can cancel resolutions
                var localizationKey = SharedResourcesKey.AuditActionResolutionCancellation;

                // 8. Update resolution status to cancelled using enhanced state pattern audit with comprehensive details
                var reason = $"Resolution cancelled by fund manager ({currentUserName})";
                var comprehensiveDetails = $"Resolution cancelled and terminated by {currentUserRole}: {currentUserName}. Status transitioned from {resolution.Status} to Cancelled. Resolution will not proceed further in the approval process. All stakeholders notified of cancellation. Resolution permanently removed from active workflow.";

                var transitionSuccess = resolution.ChangeStatusWithAudit(
                    ResolutionStatusEnum.Cancelled,
                    ResolutionActionEnum.ResolutionCancellation,
                    reason,
                    localizationKey,
                    currentUserId.Value,
                    currentUserRole,
                    comprehensiveDetails);

                if (!transitionSuccess)
                {
                    _logger.LogError(null, $"Failed to transition resolution {request.Id} to Cancelled status");
                    return BadRequest<string>(_localizer[SharedResourcesKey.InvalidStatusTransition]);
                }
                var updateResult = await _repository.Resolutions.UpdateAsync(resolution);
                if (!updateResult)
                {
                    _logger.LogError(null, $"Failed to cancel resolution with ID: {request.Id}");
                    return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorUpdatingData]);
                }
                
                // 8. Send notifications to legal council and board secretary (MSG004)
                await AddNotification(fundDetails, resolution);

                _logger.LogInfo($"Resolution cancelled successfully with ID: {request.Id}");
                return Success<string>(_localizer[SharedResourcesKey.ResolutionCancelledSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in CancelResolution for ID: {request.Id}");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorUpdatingData]);
            }
        }

        /// <summary>
        /// Validates if the current user has permission to cancel the resolution
        /// Based on Sprint.md role-based access requirements - only fund managers can cancel
        /// </summary>
        private async Task<bool> HasCancelPermission(Fund fund, int? currentUserId)
        {
            // Only Fund Manager can cancel resolutions
                // Check if user is fund manager for this fund
                return fund?.FundManagers?.Any(fm => fm.UserId == currentUserId) ?? false;
        }

     
         /// <summary>
        /// Adds notifications for resolution cancel following AddFundCommandHandler pattern
        /// Based on Sprint.md MSG004 notification requirements
        /// </summary>
        private async Task AddNotification(Fund fundDetails, Resolution resolution)
        {
            var notifications = new List<Domain.Entities.Notifications.Notification>();

            // MSG002: Notify Legal Council attached to the fund
            if (fundDetails.LegalCouncilId > 0)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    Title = string.Empty,
                    Body = $"{resolution.Code}|{fundDetails.Name}|{_currentUserService.UserName}",
                    FundId = fundDetails.Id,
                    UserId = fundDetails.LegalCouncilId,
                    NotificationType = (int)NotificationType.ResolutionCancelled, // MSG004
                });
            }

            // MSG002: Notify Board Secretaries attached to the fund
            var boardSecretaries = fundDetails.FundBoardSecretaries ?? new List<FundBoardSecretary>();
            foreach (var boardSecretary in boardSecretaries)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    Title = string.Empty,
                    Body = $"{resolution.Code}|{fundDetails.Name}|{_currentUserService.UserName}",
                    FundId = fundDetails.Id,
                    UserId = boardSecretary.UserId,
                    NotificationType = (int)NotificationType.ResolutionCancelled, // MSG004
                });
            }

            if (notifications.Any())
            {
                await _repository.Notifications.AddRangeAsync(notifications);
                _logger.LogInfo($"Resolution cancelled notifications added for Resolution ID: {resolution.Id}, Count: {notifications.Count}");
            }
        }


        #endregion

    }
}
