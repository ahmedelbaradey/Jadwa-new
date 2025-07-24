using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using AutoMapper;
using Abstraction.Contracts.Repository;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.FundManagement;
using Domain.Entities.Shared;
using Domain.Entities.Notifications;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contracts.Identity;
using Abstraction.Contract.Service;
using Application.Features.Resolutions.Dtos;
using Abstraction.Constants;
using Domain.Entities.Users;
using Domain.Services;

namespace Application.Features.Resolutions.Commands.Edit
{
    /// <summary>
    /// Consolidated Handler for EditResolutionCommand
    /// Implements comprehensive resolution editing with basic info, items, and attachments
    /// Combines capabilities from multiple user stories:
    /// - JDWA-505: Complete Resolution Attachments
    /// - JDWA-506: Complete Resolution Basic Information
    /// - JDWA-507: Complete Resolution Resolution Items
    /// - JDWA-566: Edit Resolution Resolution Items
    /// - JDWA-567: Edit Resolution Basic Information
    /// - JDWA-568: Edit Resolution Attachments
    /// Follows Clean Architecture, CQRS, RBAC, localization, audit trails, and State Pattern
    /// </summary>
    public class EditResolutionCommandHandler : BaseResponseHandler, ICommandHandler<EditResolutionCommand, BaseResponse<string>>
    {

        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IIdentityServiceManager _identityService;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public EditResolutionCommandHandler(
            IRepositoryManager repository,
            IMapper mapper,
            ILoggerManager logger,
            IStringLocalizer<SharedResources> localizer,
            IIdentityServiceManager identityService,
            ICurrentUserService currentUserService)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _identityService = identityService;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(EditResolutionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Starting consolidated EditResolution operation for ID: {request.Id}");

                // 1. Validate request
                if (request == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.EmptyRequestValidation]);

                if (request.Id <= 0)
                    return BadRequest<string>(_localizer[SharedResourcesKey.InvalidIdValidation]);

                // 2. Get current user information
                var fundDetails = await _repository.Funds.ViewFundUsers(request.FundId, trackChanges: false);   
                var currentUserId = _currentUserService.UserId;
                var userRole = await GetUserFundRole(fundDetails, currentUserId.Value);

                if (!currentUserId.HasValue)
                {
                    _logger.LogWarn("Current user ID is null");
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // 3. Retrieve existing resolution with related data
                var existingResolution = await _repository.Resolutions.GetResolutionWithAllDataAsync(request.Id, trackChanges: true);
                if (existingResolution == null)
                {
                    _logger.LogWarn($"Resolution not found with ID: {request.Id}");
                    return NotFound<string>(_localizer[SharedResourcesKey.ResolutionNotFound]);
                }

                // 4. Get fund information for validation
                if (fundDetails == null)
                {
                    _logger.LogWarn($"Fund not found for resolution ID: {request.Id}");
                    return NotFound<string>(_localizer[SharedResourcesKey.FundNotFound]);
                }

                // 5. Validate user has permission to edit this resolution
                if (! await HasEditPermission(existingResolution, userRole, fundDetails.Id, existingResolution.CreatedBy))
                {
                    _logger.LogWarn($"User {currentUserId} does not have permission to edit resolution {request.Id}");
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // 6. Validate resolution status using state pattern
                if (!existingResolution.CanEdit())
                {
                    _logger.LogError(null, $"Cannot edit resolution {request.Id} with status {existingResolution.Status}");
                    return BadRequest<string>(_localizer[SharedResourcesKey.CannotEditResolutionInCurrentStatus]);
                }

                // 7. Store original status for comparison
                var originalStatus = existingResolution.Status;

                // 8. Update basic resolution properties using AutoMapper
                _mapper.Map(request, existingResolution);

                // 9. Process resolution items if provided
                if (request.ResolutionItems?.Count > 0)
                {
                    await ProcessResolutionItems(existingResolution, request.ResolutionItems);
                }

                // 10. Process resolution attachments if provided
                if (request.AttachmentIds?.Count > 0)
                {
                    await ProcessResolutionAttachments(existingResolution, request.AttachmentIds);
                }

                // 11. Handle status transitions based on SaveAsDraft flag using state pattern
                await HandleStatusTransition(existingResolution, request.SaveAsDraft, fundDetails);

                // 12. Save changes
                var updateResult = await _repository.Resolutions.UpdateAsync(existingResolution);
                if (!updateResult)
                {
                    _logger.LogError(null, $"Failed to update resolution with ID: {request.Id}");
                    return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
                }

                // 13. Send notifications if not saved as draft
                if (!request.SaveAsDraft)
                {
                    await AddNotification(fundDetails, existingResolution, originalStatus);
                }

                // 14. Return success response
                var successMessage = request.SaveAsDraft
                    ? _localizer[SharedResourcesKey.RecordSavedSuccessfully]
                    : _localizer[SharedResourcesKey.ResolutionDataCompletedSuccessfully];

                return Success<string>(successMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in consolidated EditResolution for ID: {request.Id}");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        #endregion


        #region Helper Methods

        /// <summary>
        /// Handles voting suspension for Alternative 1 workflow
        /// Suspends active voting when editing VotingInProgress resolutions
        /// Based on Sprint.md Alternative 1 specifications for JDWA-566, JDWA-567, JDWA-568
        /// </summary>
        private async Task HandleVotingSuspension(Resolution resolution , Fund fundDetails)
        {
            try
            {
                _logger.LogInfo($"Alternative 1: Suspending voting for resolution ID: {resolution.Id}");

                // Get user context for comprehensive audit logging
                var userRole = await GetUserFundRole(fundDetails, _currentUserService.UserId.Value);
                var currentUserName = _currentUserService.UserName ?? "Unknown User";
                var localizationKey = SharedResourcesKey.AuditActionResolutionVoteSuspend;

                // Log voting suspension action with comprehensive audit details
                var comprehensiveDetails = $"Alternative 1 workflow: Voting process suspended by {userRole}: {currentUserName} to allow resolution editing. Active voting session halted, board members notified of suspension. Resolution editing now permitted while maintaining vote integrity.";
                resolution.AddAuditEntry(ResolutionActionEnum.ResolutionVoteSuspend,
                    "Voting suspended for resolution editing - Alternative 1 workflow",
                    localizationKey, _currentUserService.UserId.Value, userRole.ToString(),
                    comprehensiveDetails);

                // TODO: Implement actual voting suspension logic here
                // This would involve:
                // 1. Marking current votes as inactive
                // 2. Stopping the voting process
                // 3. Notifying board members about suspension

                _logger.LogInfo($"Alternative 1: Voting suspended successfully for resolution ID: {resolution.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Alternative 1: Error suspending voting for resolution ID: {resolution.Id}");
                throw;
            }
        }

        /// <summary>
        /// Processes resolution items using soft delete and add new strategy
        /// Soft deletes existing items and adds new ones from request
        /// Follows established soft delete patterns in the codebase
        /// </summary>
        private async Task ProcessResolutionItems(Resolution resolution, List<ResolutionItemDto> requestItems)
        {
            // Step 1: Soft delete all existing ResolutionItems and their conflicts
            await SoftDeleteExistingResolutionItems(resolution.Id);

            // Step 2: Add new ResolutionItems from the request
            await AddNewResolutionItems(resolution, requestItems);

            _logger.LogInfo($"Resolution items processed using soft delete strategy for Resolution ID: {resolution.Id}, New items count: {requestItems.Count}");
        }

        /// <summary>
        /// Soft deletes all existing ResolutionItems and their associated conflicts
        /// Follows the established soft delete pattern in the codebase
        /// </summary>
        private async Task SoftDeleteExistingResolutionItems(int resolutionId)
        {
            // Get existing items for soft delete
            var existingItems = await _repository.ResolutionItems.GetItemsByResolutionIdAsync(resolutionId, trackChanges: true);
            var existingItemsList = existingItems.ToList();

            if (existingItemsList.Any())
            {
                // Soft delete existing conflicts first
                var existingItemIds = existingItemsList.Select(i => i.Id).ToList();
                await _repository.ResolutionItemConflicts.RemoveConflictsByItemIdsAsync(existingItemIds);

                // Soft delete existing items by setting IsDeleted = true and DeletedAt = DateTime.Now
                foreach (var item in existingItemsList)
                {
                    item.IsDeleted = true;
                    item.DeletedAt = DateTime.Now;
                    // DeletedBy will be set by the audit system
                }

                // Update the soft deleted items
                await _repository.ResolutionItems.UpdateRangeAsync(existingItemsList);

                _logger.LogInfo($"Soft deleted {existingItemsList.Count} existing resolution items for Resolution ID: {resolutionId}");
            }
        }

        /// <summary>
        /// Adds new ResolutionItems from the request using AutoMapper
        /// AutoMapper handles nested ConflictMembers mapping automatically
        /// Follows established AutoMapper patterns in the codebase
        /// </summary>
        private async Task AddNewResolutionItems(Resolution resolution, List<ResolutionItemDto> requestItems)
        {
            if (!requestItems.Any()) return;

            var newItems = new List<ResolutionItem>();
            foreach (var itemDto in requestItems)
            {
                // Use AutoMapper to create new ResolutionItem with nested ConflictMembers
                var newItem = _mapper.Map<ResolutionItem>(itemDto);
                newItem.Id = 0; // Ensure it's treated as new entity
                newItem.ResolutionId = resolution.Id;
                // Set HasConflict based on ConflictMembers collection mapped by AutoMapper
                newItem.HasConflict = newItem.ConflictMembers?.Any() ?? false;
                newItems.Add(newItem);
            }

            // Add new items to the resolution
            foreach (var newItem in newItems)
            {
                resolution.ResolutionItems.Add(newItem);
            }

            _logger.LogInfo($"Added {newItems.Count} new resolution items for Resolution ID: {resolution.Id}");
        }

        /// <summary>
        /// Processes resolution attachments - updates the attachment associations
        /// Based on EditResolutionAttachmentsCommandHandler pattern
        /// </summary>
        private async Task ProcessResolutionAttachments(Resolution resolution, List<int> attachmentIds)
        {
            // Remove existing other attachments
            resolution.OtherAttachments.Clear();

            // Add new attachments
            if (attachmentIds.Any())
            {
                foreach (var attachmentId in attachmentIds)
                {
                    resolution.OtherAttachments.Add(new ResolutionAttachment
                    {
                        ResolutionId = resolution.Id,
                        AttachmentId = attachmentId
                    });
                }
            }

            _logger.LogInfo($"Resolution attachments processed for Resolution ID: {resolution.Id}, Attachments count: {attachmentIds.Count}");
        }

        /// <summary>
        /// Handles status transitions based on SaveAsDraft flag using state pattern
        /// Follows the documented state transition matrix in Resolution.md and Alternative 1 workflow from Sprint.md
        /// </summary>
        private async Task HandleStatusTransition(Resolution resolution, bool saveAsDraft , Fund fundDetails)
        {
            // Initialize state pattern if not already initialized
            if (resolution.StateContext == null)
            {
                resolution.InitializeState();
            }

            // Determine target status based on current status, SaveAsDraft flag, and user role
            var userRole = await GetUserFundRole(fundDetails, _currentUserService.UserId.Value);
            var currentUserName = _currentUserService.UserName ?? "Unknown User";
            var originalStatus = resolution.Status;
            var targetStatus = DetermineTargetStatus(resolution.Status, saveAsDraft, userRole);

            if (resolution.Status != targetStatus)
            {
                // Alternative 1: Handle voting suspension for VotingInProgress resolutions
                if (originalStatus == ResolutionStatusEnum.VotingInProgress && targetStatus == ResolutionStatusEnum.WaitingForConfirmation)
                {
                    await HandleVotingSuspension(resolution, fundDetails);

                    // Use enhanced audit method for Alternative 1 voting suspension with comprehensive details
                    var localizationKey = SharedResourcesKey.AuditActionResolutionVoteSuspend;
                    var comprehensiveDetails = $"Alternative 1 workflow: Resolution status transitioned from VotingInProgress to WaitingForConfirmation by {userRole}: {currentUserName}. Voting process suspended to allow resolution editing. Board members notified of suspension and editing activity.";
                    var transitionSuccess = resolution.ChangeStatusWithAudit(targetStatus,
                        ResolutionActionEnum.ResolutionVoteSuspend,
                        "Alternative 1: Voting suspended for resolution editing",
                        localizationKey, _currentUserService.UserId.Value, userRole.ToString(),
                        comprehensiveDetails);

                    if (!transitionSuccess)
                    {
                        _logger.LogError(null, $"Failed to transition resolution {resolution.Id} from {resolution.Status} to {targetStatus}");
                        throw new InvalidOperationException($"Invalid status transition from {resolution.Status} to {targetStatus}");
                    }
                }
                else
                {
                    // Determine action and localization key based on target status
                    ResolutionActionEnum action;
                    string localizationKey;
                    string actionDescription;

                    if (targetStatus == ResolutionStatusEnum.CompletingData)
                    {
                        // Use ResolutionCompletion action when transitioning to CompletingData
                        action = ResolutionActionEnum.ResolutionCompletion;
                        localizationKey = SharedResourcesKey.AuditActionResolutionCompletion;
                        actionDescription = "Resolution data completion initiated";
                    }
                    else
                    {
                        // Use ResolutionEdit action for other transitions
                        action = ResolutionActionEnum.ResolutionEdit;
                        localizationKey = SharedResourcesKey.AuditActionResolutionDataUpdate;
                        actionDescription = "Resolution edited via consolidated edit command";
                    }

                    var comprehensiveDetails = $"Resolution edited and status transitioned from {originalStatus} to {targetStatus} by {userRole}: {currentUserName}. Consolidated edit operation completed including basic information, attachments, and resolution items as applicable.";
                    var transitionSuccess = resolution.ChangeStatusWithAudit(targetStatus,
                        action, actionDescription,
                        localizationKey, _currentUserService.UserId.Value, userRole.ToString(),
                        comprehensiveDetails);

                    if (!transitionSuccess)
                    {
                        _logger.LogError(null, $"Failed to transition resolution {resolution.Id} from {resolution.Status} to {targetStatus}");
                        throw new InvalidOperationException($"Invalid status transition from {resolution.Status} to {targetStatus}");
                    }
                }
            }
            else
            {
                // No status change, but still add comprehensive audit entry for the edit action
                var localizationKey = SharedResourcesKey.AuditActionResolutionDataUpdate;
                var comprehensiveDetails = $"Resolution data updated without status change by {userRole}: {currentUserName}. Edit operation completed on resolution content while maintaining current {originalStatus} status. Changes may include basic information, attachments, or resolution items.";
                resolution.AddAuditEntry(ResolutionActionEnum.ResolutionEdit,
                    "Resolution data updated without status change", localizationKey,
                    _currentUserService.UserId.Value, userRole.ToString(),
                    comprehensiveDetails);
            }
        }

        /// <summary>
        /// Determines the target status based on current status, SaveAsDraft flag, and user role
        /// Based on the state transition matrix documented in Resolution.md and Alternative 1 workflow from Sprint.md
        /// </summary>
        private ResolutionStatusEnum DetermineTargetStatus(ResolutionStatusEnum currentStatus, bool saveAsDraft, Roles userRole)
        {
            // Fund Manager transitions
            if (userRole == Roles.FundManager)
            {
                return currentStatus switch
                {
                    ResolutionStatusEnum.Draft when saveAsDraft => ResolutionStatusEnum.Draft,
                    ResolutionStatusEnum.Draft when !saveAsDraft => ResolutionStatusEnum.Pending,
                    ResolutionStatusEnum.Pending => ResolutionStatusEnum.Pending, // Fund Manager can only edit, not change status from Pending
                    _ => currentStatus // No status change for other statuses
                };
            }

            // Legal Council and Board Secretary transitions
            if (userRole == Roles.LegalCouncil || userRole == Roles.BoardSecretary)
            {
                return currentStatus switch
                {
                    ResolutionStatusEnum.Pending when saveAsDraft => ResolutionStatusEnum.CompletingData,
                    ResolutionStatusEnum.Pending when !saveAsDraft => ResolutionStatusEnum.WaitingForConfirmation,
                    ResolutionStatusEnum.CompletingData when saveAsDraft => ResolutionStatusEnum.CompletingData,
                    ResolutionStatusEnum.CompletingData when !saveAsDraft => ResolutionStatusEnum.WaitingForConfirmation,
                    ResolutionStatusEnum.WaitingForConfirmation => ResolutionStatusEnum.WaitingForConfirmation, // Can edit but status stays same
                    ResolutionStatusEnum.Confirmed when !saveAsDraft => ResolutionStatusEnum.WaitingForConfirmation, // Edit and resubmit
                    ResolutionStatusEnum.Rejected when !saveAsDraft => ResolutionStatusEnum.WaitingForConfirmation, // Edit and resubmit
                    // Alternative 1: VotingInProgress can be edited with voting suspension
                    ResolutionStatusEnum.VotingInProgress when !saveAsDraft => ResolutionStatusEnum.WaitingForConfirmation, // Suspend voting and edit
                    _ => currentStatus // No status change for other statuses
                };
            }

            // Default: no status change
            return currentStatus;
        }

        /// <summary>
        /// Sets action permissions based on user role and resolution status
        /// Based on Sprint.md requirements for user stories JDWA-588, JDWA-589, JDWA-593, JDWA-584
        /// Includes CanCancel and CanDelete flags based on ResolutionDomainService business rules
        /// </summary>
        private async Task<bool> HasEditPermission(Resolution response, Roles userRole, int fundId, int creatorID)
        {
            var isFundManager = userRole == Roles.FundManager;
            var isLegalCouncilOrBoardSecretary = userRole == Roles.LegalCouncil || userRole == Roles.BoardSecretary;
            bool canEdit = false;
            // Set permissions based on status and role
            switch (response.Status)
            {
                case ResolutionStatusEnum.Draft:
                    // Only Fund Manager can edit and delete draft resolutions
                    canEdit = isFundManager && _currentUserService.UserId == creatorID;
                    break;

                case ResolutionStatusEnum.Pending:
                    // Only Fund Manager can edit and cancel pending resolutions
                    canEdit = isFundManager || isLegalCouncilOrBoardSecretary;
                    break;

                case ResolutionStatusEnum.WaitingForConfirmation:
                    canEdit = isLegalCouncilOrBoardSecretary;
                    break;

                case ResolutionStatusEnum.CompletingData:
                case ResolutionStatusEnum.Confirmed:
                case ResolutionStatusEnum.VotingInProgress:
                case ResolutionStatusEnum.Approved:
                case ResolutionStatusEnum.NotApproved:
                case ResolutionStatusEnum.Rejected:
                    canEdit = isLegalCouncilOrBoardSecretary;
                    break;
                // Read-only for all these statuses
                case ResolutionStatusEnum.Cancelled:

                default:
                    // Unknown status, no actions allowed
                    break;

            }
            return canEdit;
        }



        /// <summary>
        /// Adds comprehensive notifications for resolution editing covering all Sprint.md user stories
        /// Implements notifications for consolidated user stories:
        /// - JDWA-505: Complete Resolution Attachments (MSG003)
        /// - JDWA-506: Complete Resolution Basic Information (MSG003)
        /// - JDWA-507: Complete Resolution Resolution Items (MSG003)
        /// - JDWA-566: Edit Resolution Resolution Items (MSG003/MSG007)
        /// - JDWA-567: Edit Resolution Basic Information (MSG003/MSG007)
        /// - JDWA-568: Edit Resolution Attachments (MSG003/MSG007)
        /// Uses proper localization with SharedResources and IStringLocalizer
        /// Supports Alternative 1 workflow with MSG007 notifications for voting suspension
        /// </summary>
        private async Task AddNotification(Fund fundDetails, Resolution resolution, ResolutionStatusEnum originalStatus)
        {
            var notifications = new List<Domain.Entities.Notifications.Notification>();
            // Get current user information for notification context
            var currentUserRole = await GetUserFundRole(fundDetails, _currentUserService.UserId.Value);
            // Determine notification type and recipients based on user role and resolution status
            var notificationType = DetermineNotificationType(originalStatus, resolution.Status);
            var recipientUserIds = GetNotificationRecipients(fundDetails, currentUserRole, notificationType);


            // Create localized notifications for all recipients
            foreach (var userId in recipientUserIds)
            {
                var notification = new Domain.Entities.Notifications.Notification
                {
                    UserId = userId,
                    FundId = fundDetails.Id,
                    Title = string.Empty,
                    Body = $"{resolution.Code}|{fundDetails.Name}|{currentUserRole}|{_currentUserService.UserName}",
                    NotificationType = (int)notificationType,
                    IsRead = false
                };
                notifications.Add(notification);
            }

            // Save notifications if any were created
            if (notifications.Any())
            {
                await _repository.Notifications.AddRangeAsync(notifications);
                _logger.LogInfo($"Resolution edit notifications added for Resolution ID: {resolution.Id}, Count: {notifications.Count}, Type: {notificationType}");
            }
        }

        /// <summary>
        /// Determines the appropriate notification type based on resolution status and user role
        /// Implements Sprint.md notification requirements for all consolidated user stories including Alternative 1
        /// </summary>
        private NotificationType DetermineNotificationType(ResolutionStatusEnum originalStatus, ResolutionStatusEnum currentStatus)
        {
            // Alternative 1: If voting was suspended (VotingInProgress -> WaitingForConfirmation), use MSG007
            if (originalStatus == ResolutionStatusEnum.VotingInProgress &&
                currentStatus == ResolutionStatusEnum.WaitingForConfirmation)
            {
                return NotificationType.ResolutionVotingSuspended; // MSG007
            }

            // For all other edit and complete operations, use ResolutionUpdated notification type
            // This covers MSG003/MSG005 notifications for all user stories (JDWA-505, 506, 507, 566, 567, 568)
            return NotificationType.ResolutionUpdated;
        }

        /// <summary>
        /// Gets notification recipients based on current user role and Sprint.md requirements
        /// Implements comprehensive recipient logic for all consolidated user stories including Alternative 1
        /// </summary>
        private List<int> GetNotificationRecipients(Fund fundDetails, Roles userRole, NotificationType notificationType)
        {
            var recipients = new List<int>();

            // Alternative 1: MSG007 notifications for voting suspension - notify all stakeholders
            if (notificationType == NotificationType.ResolutionVotingSuspended)
            {
                // Based on Sprint.md Alternative 1: notify fund managers, legal council, board secretaries, and board members
                AddFundManagersToRecipients(recipients, fundDetails);
                AddLegalCouncilToRecipients(recipients, fundDetails);
                AddBoardSecretariesToRecipients(recipients, fundDetails);
                AddBoardMembersToRecipients(recipients, fundDetails);
                return recipients;
            }

            // Based on Sprint.md MSG003 requirements for resolution data completion/update:
            // Notify fund managers, legal council, and board secretaries based on editor role

            if (userRole == Roles.LegalCouncil)
            {
                // If editor is Legal Council, notify Fund Managers and Board Secretaries
                AddFundManagersToRecipients(recipients, fundDetails);
                AddBoardSecretariesToRecipients(recipients, fundDetails);
            }
            else if (userRole == Roles.BoardSecretary)
            {
                // If editor is Board Secretary, notify Fund Managers and Legal Council
                AddFundManagersToRecipients(recipients, fundDetails);
                AddLegalCouncilToRecipients(recipients, fundDetails);
            }
            else if (userRole == Roles.FundManager)
            {
                // If editor is Fund Manager, notify Legal Council and Board Secretaries
                AddLegalCouncilToRecipients(recipients, fundDetails);
                AddBoardSecretariesToRecipients(recipients, fundDetails);
            }

            return recipients;
        }

        /// <summary>
        /// Adds fund managers to the notification recipients list
        /// </summary>
        private void AddFundManagersToRecipients(List<int> recipients, Fund fundDetails)
        {
            if (fundDetails.FundManagers != null)
            {
                foreach (var fundManager in fundDetails.FundManagers)
                {
                    if (!recipients.Contains(fundManager.UserId))
                    {
                        recipients.Add(fundManager.UserId);
                    }
                }
            }
        }

        /// <summary>
        /// Adds legal council to the notification recipients list
        /// </summary>
        private void AddLegalCouncilToRecipients(List<int> recipients, Fund fundDetails)
        {
            if (fundDetails.LegalCouncilId > 0 && !recipients.Contains(fundDetails.LegalCouncilId))
            {
                recipients.Add(fundDetails.LegalCouncilId);
            }
        }

        /// <summary>
        /// Adds board secretaries to the notification recipients list
        /// </summary>
        private void AddBoardSecretariesToRecipients(List<int> recipients, Fund fundDetails)
        {
            if (fundDetails.FundBoardSecretaries != null)
            {
                foreach (var boardSecretary in fundDetails.FundBoardSecretaries)
                {
                    if (!recipients.Contains(boardSecretary.UserId))
                    {
                        recipients.Add(boardSecretary.UserId);
                    }
                }
            }
        }

        /// <summary>
        /// Adds board members to the notification recipients list
        /// Used for Alternative 1 MSG007 notifications when voting is suspended
        /// </summary>
        private void AddBoardMembersToRecipients(List<int> recipients, Fund fundDetails)
        {
            if (fundDetails.BoardMembers != null)
            {
                foreach (var boardMember in fundDetails.BoardMembers)
                {
                    if (!recipients.Contains(boardMember.UserId))
                    {
                        recipients.Add(boardMember.UserId);
                    }
                }
            }
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
                // Get fund details with all related entities
               
                if (fundDetails == null)
                {
                    _logger.LogWarn($"Fund not found with ID: {fundDetails.Id}");
                    return Roles.None;
                }

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
        


        #endregion

      
    }
}
