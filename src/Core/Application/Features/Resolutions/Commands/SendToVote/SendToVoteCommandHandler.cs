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
using Abstraction.Constants;

namespace Application.Features.Resolutions.Commands.SendToVote
{
    /// <summary>
    /// Handler for SendToVoteCommand
    /// Implements business logic for sending confirmed resolutions to vote
    /// Based on Sprint.md requirements (JDWA-569) and existing Resolution patterns
    /// </summary>
    public class SendToVoteCommandHandler : BaseResponseHandler, ICommandHandler<SendToVoteCommand, BaseResponse<string>>
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
        public SendToVoteCommandHandler(
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
        public async Task<BaseResponse<string>> Handle(SendToVoteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Starting SendToVote operation for resolution ID: {request.Id}");

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
                var fund = await _repository.Funds.ViewFundUsers(resolution.FundId, trackChanges: false);
                if (fund == null)
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

                // 5. Validate user permissions (only Legal Council/Board Secretary can send to vote)
                if (!await HasSendToVotePermission(fund, currentUserId.Value, currentUserRoles))
                {
                    _logger.LogWarn($"User {currentUserId.Value} does not have permission to send resolution {request.Id} to vote");
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // 6. Validate resolution status (must be Confirmed)
                if (resolution.Status != ResolutionStatusEnum.Confirmed)
                {
                    _logger.LogError(null, $"Cannot send resolution {request.Id} to vote with status {resolution.Status}");
                    return BadRequest<string>(_localizer[SharedResourcesKey.InvalidResolutionStatusForVoting]);
                }

                // 7. Initialize state pattern if not already initialized
                if (resolution.StateContext == null)
                {
                    resolution.InitializeState();
                }

                // 8. Get current user information for comprehensive audit trail (Sprint.md requirement)
                var currentUserName = _currentUserService.UserName ?? "Unknown User";
                var currentUserRole = await GetUserFundRole(fund, currentUserId.Value);
                var localizationKey = SharedResourcesKey.AuditActionResolutionSentToVote;

                // 9. Update resolution status to VotingInProgress using enhanced state pattern audit with comprehensive details
                var reason = $"Resolution sent to vote by {currentUserRole.ToString().ToLower()} ({currentUserName})";
                var comprehensiveDetails = $"Resolution sent to voting process by {currentUserRole}: {currentUserName}. Status transitioned from {resolution.Status} to VotingInProgress. Voting session initiated for board members. All eligible voters notified and voting period commenced according to fund governance rules.";

                var transitionSuccess = resolution.ChangeStatusWithAudit(
                    ResolutionStatusEnum.VotingInProgress,
                    ResolutionActionEnum.SentToVote,
                    reason,
                    localizationKey,
                    currentUserId.Value,
                    currentUserRole.ToString(),
                    comprehensiveDetails);

                if (!transitionSuccess)
                {
                    _logger.LogError(null, $"Failed to transition resolution {request.Id} to VotingInProgress status");
                    return BadRequest<string>(_localizer[SharedResourcesKey.InvalidStatusTransition]);
                }

                // 9. Initialize voting session (votes will be created when members actually vote)
                // The resolution status change to VotingInProgress indicates voting is active

                // 10. Save changes
                var updateResult = await _repository.Resolutions.UpdateAsync(resolution);
                if (!updateResult)
                {
                    _logger.LogError(null, $"Failed to send resolution to vote with ID: {request.Id}");
                    return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorUpdatingData]);
                }

                // 11. Send notifications to all stakeholders (MSG002)
                await AddNotification(fund, resolution, currentUserId.Value);

                _logger.LogInfo($"Resolution sent to vote successfully with ID: {request.Id} by user: {currentUserName} (Role: {currentUserRole})");

                // 12. Return MSG001 success message as required by Sprint.md
                return Success<string>(_localizer[SharedResourcesKey.OperationCompletedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in SendToVote for resolution ID: {request.Id}");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorUpdatingData]);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Validates if the current user has permission to send the resolution to vote
        /// Based on Sprint.md role-based access requirements - only Legal Council/Board Secretary can send to vote
        /// </summary>
        private async Task<bool> HasSendToVotePermission(Fund fund, int currentUserId, IList<string> userRoles)
        {
            // First check if user has the required roles
            if (!RoleHelper.IsLegalCouncil(userRoles) && !RoleHelper.IsBoardSecretary(userRoles))
            {
                return false;
            }

            var fundDetails = await _repository.Funds.ViewFundUsers(fund.Id, trackChanges: false);

            // Check if user is Legal Council for this fund
            var isLegalCouncil = fundDetails?.LegalCouncilId == currentUserId ? true : false ;

            // Check if user is Board Secretary for this fund
            var isBoardSecretary = fundDetails?.FundBoardSecretaries?.Any(bs => bs.UserId == currentUserId) ?? false;

            return isLegalCouncil || isBoardSecretary;
        }

        /// <summary>
        /// Determines the current user's role within a specific fund context
        /// Checks FundBoardSecretary table, Fund.LegalCouncilId field, and FundManager table
        /// Follows Clean Architecture and CQRS patterns with proper repository usage
        /// </summary>
        /// <param name="fundId">The fund ID to check roles for</param>
        /// <param name="currentUserId">The current user ID to check roles for</param>
        /// <returns>Comma-separated string of roles the user has in the fund, or empty string if no roles</returns>
        private async Task<Roles> GetUserFundRole(Fund fundDetails, int currentUserId)
        {
            try
            {
                _logger.LogInfo($"Checking fund roles for User ID: {currentUserId} in Fund ID: {fundDetails.Id}");

                var userRole = Roles.None;

                // 1. Check if user is Legal Council for the fund
                if (fundDetails.LegalCouncilId == currentUserId)
                {
                    userRole = Roles.LegalCouncil;
                    _logger.LogInfo($"User ID: {currentUserId} is Legal Council for Fund ID: {fundDetails.Id}");
                }

                // 2. Check if user is a Fund Manager for the fund
                if (fundDetails.FundManagers != null && fundDetails.FundManagers.Count > 0)
                {
                    var isFundManager = fundDetails.FundManagers.Any(fm => fm.UserId == currentUserId);
                    if (isFundManager)
                    {
                        userRole = Roles.FundManager;
                        _logger.LogInfo($"User ID: {currentUserId} is Fund Manager for Fund ID: {fundDetails.Id}");
                    }
                }

                // 3. Check if user is a Board Secretary for the fund
                if (fundDetails.FundBoardSecretaries != null && fundDetails.FundBoardSecretaries.Count > 0)
                {
                    var isBoardSecretary = fundDetails.FundBoardSecretaries.Any(bs => bs.UserId == currentUserId);
                    if (isBoardSecretary)
                    {
                        userRole = Roles.BoardSecretary;
                        _logger.LogInfo($"User ID: {currentUserId} is Board Secretary for Fund ID: {fundDetails.Id}");
                    }
                }

                // 4. Check if user is a Board Member for the fund
                if (fundDetails.BoardMembers != null && fundDetails.BoardMembers.Count > 0)
                {
                    var isBoardMember = fundDetails.BoardMembers.Any(bs => bs.UserId == currentUserId);
                    if (isBoardMember)
                    {
                        userRole = Roles.BoardMember;
                        _logger.LogInfo($"User ID: {currentUserId} is Board Member for Fund ID: {fundDetails.Id}");
                    }
                }

                // Return comma-separated roles or empty string if no roles found

                _logger.LogInfo($"User ID: {currentUserId} has roles in Fund ID: {fundDetails.Id}: '{userRole}'");

                return userRole;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking fund roles for User ID: {currentUserId} in Fund ID: {fundDetails.Id}");
                return Roles.None;
            }
        }



        /// <summary>
        /// Adds notifications for resolution sent to vote following established pattern
        /// Based on Sprint.md MSG002 notification requirements for resolution sent to vote
        /// Follows the same pattern as EditResolutionCommandHandler
        /// </summary>
        private async Task AddNotification(Fund fundDetails, Resolution resolution, int currentUserId)
        {
            try
            {
                var notifications = new List<Domain.Entities.Notifications.Notification>();
                // Get current user details
                var currentUser = await _identityService.UserManagmentService.FindByIdAsync(currentUserId.ToString());
                var currentUserName = currentUser?.FullName ?? _currentUserService.UserName;

                // MSG002: Notify Fund Managers attached to the fund
                foreach (var fundManager in fundDetails.FundManagers)
                {
                    notifications.Add(new Domain.Entities.Notifications.Notification
                    {
                        UserId = fundManager.UserId,
                        FundId = fundDetails.Id,
                        Title = string.Empty,
                        Body = $"{resolution.Code}|{fundDetails.Name}|{_currentUserService.UserName}",
                        NotificationType = (int)NotificationType.ResolutionSentToVote,
                        IsRead = false
                    });
                }

                // MSG002: Notify Board Members attached to the fund
                foreach (var boardMember in fundDetails.BoardMembers.Where(bm => bm.IsActive))
                {
                    notifications.Add(new Domain.Entities.Notifications.Notification
                    {
                        UserId = boardMember.UserId,
                        FundId = fundDetails.Id,
                        Title = string.Empty,
                        Body = $"{resolution.Code}|{fundDetails.Name}|{_currentUserService.UserName}",
                        NotificationType = (int)NotificationType.ResolutionSentToVote,
                        IsRead = false
                    });
                }

                // MSG002: Notify Legal Council attached to the fund
                if (fundDetails.LegalCouncilId > 0)
                {
                    notifications.Add(new Domain.Entities.Notifications.Notification
                    {
                        UserId = fundDetails.LegalCouncilId,
                        FundId = fundDetails.Id,
                        Title = string.Empty,
                        Body = $"{resolution.Code}|{fundDetails.Name}|{_currentUserService.UserName}",
                        NotificationType = (int)NotificationType.ResolutionSentToVote,
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
                        Title = string.Empty,
                        Body = $"{resolution.Code}|{fundDetails.Name}|{_currentUserService.UserName}",
                        NotificationType = (int)NotificationType.ResolutionSentToVote,
                        IsRead = false
                    });
                }

                // Save notifications using the same pattern as other handlers
                if (notifications.Count > 0)
                {
                    await _repository.Notifications.AddRangeAsync(notifications);
                    _logger.LogInfo($"Resolution sent to vote notifications added for Resolution ID: {resolution.Id}, Count: {notifications.Count}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending vote notifications for resolution {resolution.Id}");
                // Don't fail the main operation if notification fails
            }
        }
        #endregion
    }
}
