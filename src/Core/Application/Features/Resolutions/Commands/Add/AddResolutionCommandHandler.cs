using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using AutoMapper;
using Domain.Entities;
using Abstraction.Contracts.Repository;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.FundManagement;
using Domain.Services;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contracts.Identity;
using Domain.Entities.Notifications;
using Abstraction.Contract.Service;
using Abstraction.Constants;

namespace Application.Features.Resolutions.Commands.Add
{
    /// <summary>
    /// Handler for AddResolutionCommand
    /// Implements comprehensive resolution creation with business rules, validation, and notifications
    /// Based on Sprint.md requirements (JDWA-511)
    /// </summary>
    public class AddResolutionCommandHandler : BaseResponseHandler, ICommandHandler<AddResolutionCommand, BaseResponse<string>>
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
        public AddResolutionCommandHandler(
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
        public async Task<BaseResponse<string>> Handle(AddResolutionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Starting AddResolution operation for FundId: {request.FundId}");

                // 1. Validate request
                if (request == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);

                // 2. Validate current user roles and authorization
                var currentUserId = _currentUserService.UserId;
                var fundDetails = await _repository.Funds.ViewFundUsers(request.FundId, trackChanges: false);
                var userRole = await GetUserFundRole(fundDetails, currentUserId.Value);
                if (!currentUserId.HasValue)
                {
                    _logger.LogWarn("Current user ID is null");
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // 3. Validate fund exists and is active
                var fund = await _repository.Funds.GetByIdAsync<Fund>(request.FundId, trackChanges: false);
                if (fund == null)
                {
                    _logger.LogWarn($"Fund not found with ID: {request.FundId}");
                    return NotFound<string>(_localizer[SharedResourcesKey.FundNotFound]);
                }

                // 4. Validate that creator is a fund manager for this specific fund
                // This ensures only fund managers can create resolutions for their funds
                if (!await HasCreatePermission(fund, currentUserId.Value, userRole) && request.Status != ResolutionStatusEnum.Approved && request.Status != ResolutionStatusEnum.NotApproved)
                {
                    _logger.LogWarn($"User {currentUserId} does not have permission to create resolution for fund {request.FundId}");
                    return BusinessValidation<string>(_localizer[SharedResourcesKey.OnlyFundManagerCanCreateResolution]);
                }

                // 4. Handle Alternative 2: Creating new resolution from approved/not approved resolution
                Resolution originalResolution = null;
                bool isAlternative2 = false;

                if (request.OriginalResolutionId.HasValue)
                {
                    // For Alternative 2, we need the original resolution with basic info for validation
                    // The detailed items will be loaded separately in CopyResolutionItemsFromOriginal method
                    originalResolution = await _repository.Resolutions.GetByIdAsync<Resolution>(request.OriginalResolutionId.Value, trackChanges: false);
                    if (originalResolution == null)
                    {
                        _logger.LogWarn($"Original resolution not found with ID: {request.OriginalResolutionId.Value}");
                        return NotFound<string>(_localizer[SharedResourcesKey.ResolutionNotFound]);
                    }

                    // Validate that original resolution is approved or not approved (Alternative 2 requirement)
                    if (originalResolution.Status != ResolutionStatusEnum.Approved &&
                        originalResolution.Status != ResolutionStatusEnum.NotApproved)
                    {
                        _logger.LogWarn($"Cannot create new resolution from resolution with status: {originalResolution.Status}");
                        return BadRequest<string>(_localizer[SharedResourcesKey.CannotEditApprovedOrRejectedResolution]);
                    }

                    // Validate user has permission to create new resolution from approved/not approved
                    if (userRole != Roles.LegalCouncil     && userRole !=Roles.BoardSecretary)
                    {
                        _logger.LogWarn($"User {currentUserId} attempted Alternative 2 without proper role");
                        return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                    }

                    isAlternative2 = true;
                    _logger.LogInfo($"Alternative 2: Creating new resolution from original resolution ID: {originalResolution.Id}");
                }


                // 5. Generate resolution code using domain service
                var fundresolutions = _repository.Resolutions.GetByCondition<Resolution>(r => r.FundId == request.FundId && r.ResolutionDate.Year == request.ResolutionDate.Year).ToList();
                var resolutionDomainService = new ResolutionDomainService();
                var resolutionCode = ResolutionDomainService.GenerateResolutionCode(request.FundId.ToString(), fundresolutions, request.ResolutionDate.Year);
                if (resolutionCode == null)
                {
                    _logger.LogError(null, $"Failed to generate resolution code for FundId: {request.FundId}, Year: {request.ResolutionDate.Year}");
                    return ServerError<string>(_localizer[SharedResourcesKey.ResolutionCodeGenerationFailed]);
                }

                // 6. Create resolution entity
                var resolution = _mapper.Map<Resolution>(request);
                resolution.Code = resolutionCode.Value;

                // Alternative 2: Set relationship to original resolution and copy items
                if (isAlternative2)
                {
                    resolution.ParentResolutionId = originalResolution.Id;
                    _logger.LogInfo($"Alternative 2: Linking new resolution to original resolution ID: {originalResolution.Id}, Code: {originalResolution.Code}");

                    // Copy ResolutionItems and ResolutionItemConflicts from original resolution
                    await CopyResolutionItemsFromOriginal(resolution, originalResolution);
                }

                // Get user context for comprehensive audit logging
                var currentUserName = _currentUserService.UserName ?? "Unknown User";
                var localizationKey = SharedResourcesKey.AuditActionResolutionCreation;

                // Initialize state based on Alternative 2 or normal flow
                if (isAlternative2)
                {
                    // Alternative 2: Always start with WaitingForConfirmation status (Sprint.md requirement)
                    resolution.Status = ResolutionStatusEnum.WaitingForConfirmation;
                    resolution.InitializeState();

                    // Add comprehensive audit entry for Alternative 2 creation
                    var alternative2Details = $"Alternative 2: Resolution created from original approved/not approved resolution {originalResolution.Code} by {userRole}: {currentUserName}. New resolution will require confirmation before proceeding.";
                    resolution.AddAuditEntry(ResolutionActionEnum.ResolutionCreation,
                        "Alternative 2: Resolution created from approved/not approved resolution",
                        localizationKey, currentUserId.Value, userRole.ToString(), alternative2Details);

                    _logger.LogInfo($"Alternative 2: Resolution created with WaitingForConfirmation status");
                }
                else
                {
                    // Normal flow: Initialize with draft status and use state pattern for transition
                    resolution.Status = ResolutionStatusEnum.Draft;
                    resolution.InitializeState();

                    if (request.SaveAsDraft)
                    {
                        var draftCreationDetails = $"Resolution created in Draft status by {userRole}: {currentUserName}. Resolution initialized and ready for editing or submission for review.";
                        resolution.AddAuditEntry(ResolutionActionEnum.ResolutionCreation,
                            "Resolution created in Draft status", localizationKey,
                            currentUserId.Value, userRole.ToString(), draftCreationDetails);
                    }
                    else
                    {
                        var comprehensiveDetails = $"Resolution created and sent for review by {userRole}: {currentUserName}. Status transitioned from Draft to Pending for legal review and approval process.";
                        var transitionSuccess = resolution.ChangeStatusWithAudit(ResolutionStatusEnum.Pending,
                            ResolutionActionEnum.ResolutionCreation, "Resolution sent for review",
                            localizationKey, currentUserId.Value, userRole.ToString(),
                            comprehensiveDetails);
                        if (!transitionSuccess)
                        {
                            _logger.LogError(null, $"Failed to transition resolution to Pending status");
                            return ServerError<string>(_localizer[SharedResourcesKey.InvalidStatusTransition]);
                        }
                    }    
                }

                // 7. Save resolution
                var addedResolution = await _repository.Resolutions.AddAsync(resolution, cancellationToken);
                _logger.LogInfo($"Resolution created successfully with ID: {addedResolution.Id}, Code: {resolutionCode.Value}");

                // 8. Send notifications based on flow type
                if (isAlternative2)
                {
                    // Alternative 2: Always send MSG009 notifications
                    await AddAlternative2Notification(fundDetails, addedResolution, originalResolution , userRole);
                }
                else if (!request.SaveAsDraft)
                {
                    // Normal flow: Send MSG002 notifications if not saved as draft
                    await AddNotification(fund.Id, addedResolution);
                }

                // 9. Return success response
                string successMessage;
                if (isAlternative2)
                {
                    successMessage = _localizer[SharedResourcesKey.ResolutionCreatedSuccessfully];
                }
                else
                {
                    successMessage = request.SaveAsDraft
                        ? _localizer[SharedResourcesKey.ResolutionSavedAsDraft]
                        : _localizer[SharedResourcesKey.ResolutionSentForReview];
                }

                return Success<string>(successMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in AddResolution: {ex.Message}");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        #endregion

        #region Private Methods 
        /// <summary>
        /// Copies ResolutionItems and ResolutionItemConflicts from original resolution for Alternative 2 functionality
        /// Uses AutoMapper to properly handle entity mapping and relationship setup
        /// Maintains Clean Architecture principles and proper audit trail
        /// </summary>
        private async Task CopyResolutionItemsFromOriginal(Resolution newResolution, Resolution originalResolution)
        {
            try
            {
                // Retrieve original resolution with all items and conflicts
                var originalWithItems = await _repository.Resolutions.GetResolutionWithItemsAsync(originalResolution.Id, trackChanges: false);
                if (originalWithItems?.ResolutionItems == null || !originalWithItems.ResolutionItems.Any())
                {
                    _logger.LogInfo($"Alternative 2: No resolution items found in original resolution ID: {originalResolution.Id}");
                    return;
                }

                _logger.LogInfo($"Alternative 2: Copying {originalWithItems.ResolutionItems.Count} resolution items from original resolution ID: {originalResolution.Id}");

                // Copy each ResolutionItem using AutoMapper
                foreach (var originalItem in originalWithItems.ResolutionItems.OrderBy(ri => ri.DisplayOrder))
                {
                    // Use AutoMapper to copy the ResolutionItem with its ConflictMembers
                    var newItem = _mapper.Map<ResolutionItem>(originalItem);

                    // Set the new resolution ID and reset entity ID
                    newItem.Id = 0; // Ensure it's treated as new entity
                    newItem.ResolutionId = newResolution.Id;

                    // Ensure ConflictMembers are properly copied and reset
                    if (newItem.ConflictMembers?.Any() == true)
                    {
                        foreach (var conflict in newItem.ConflictMembers)
                        {
                            conflict.Id = 0; // Reset ID for new entity
                            conflict.ResolutionItemId = 0; // Will be set when item is saved
                        }

                        _logger.LogInfo($"Alternative 2: Copied {newItem.ConflictMembers.Count} conflicts for item '{newItem.Title}'");
                    }

                    // Add the copied item to the new resolution
                    newResolution.ResolutionItems.Add(newItem);

                    _logger.LogInfo($"Alternative 2: Copied resolution item '{newItem.Title}' with {newItem.ConflictMembers?.Count ?? 0} conflicts");
                }

                _logger.LogInfo($"Alternative 2: Successfully copied all resolution items and conflicts to new resolution");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Alternative 2: Error copying resolution items from original resolution ID: {originalResolution.Id}");
                throw; // Re-throw to ensure transaction rollback
            }
        }

        /// <summary>
        /// Adds notifications for resolution creation following AddFundCommandHandler pattern
        /// Based on Sprint.md MSG002 notification requirements
        /// </summary>
        private async Task AddNotification(int fundId, Resolution resolution)
        {
            var notifications = new List<Domain.Entities.Notifications.Notification>();

            // Get fund details to access legal council and board secretaries
            var fundDetails = await _repository.Funds.ViewFundUsers(fundId, trackChanges: false);
            if (fundDetails == null) return;

            // MSG002: Notify Legal Council attached to the fund
            if (fundDetails.LegalCouncilId > 0)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    Title = string.Empty,
                    Body = $"{fundDetails.Name}|{_currentUserService.UserName}",
                    FundId = fundDetails.Id,
                    UserId = fundDetails.LegalCouncilId,
                    NotificationType = (int)NotificationType.ResolutionCreated, // MSG002
                });
            }

            // MSG002: Notify Board Secretaries attached to the fund
            var boardSecretaries = fundDetails.FundBoardSecretaries ?? new List<FundBoardSecretary>();
            foreach (var boardSecretary in boardSecretaries)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    Title = string.Empty,
                    Body = $"{fundDetails.Name}|{_currentUserService.UserName}",
                    FundId = fundDetails.Id,
                    UserId = boardSecretary.UserId,
                    NotificationType = (int)NotificationType.ResolutionCreated, // MSG002
                });
            }

            if (notifications.Any())
            {
                await _repository.Notifications.AddRangeAsync(notifications);
                _logger.LogInfo($"Resolution created notifications added for Resolution ID: {resolution.Id}, Count: {notifications.Count}");
            }
        }

        /// <summary>
        /// Adds Alternative 2 notifications for creating new resolution from approved/not approved resolution
        /// Implements MSG009 notification requirements from Sprint.md
        /// Follows established localization patterns with SharedResources and IStringLocalizer
        /// </summary>
        private async Task AddAlternative2Notification(Fund fundDetails, Resolution newResolution, Resolution originalResolution , Roles userRole)
        {
            var notifications = new List<Domain.Entities.Notifications.Notification>();
            // Determine user role for notification body

            // MSG009: Notify all stakeholders (Fund Managers, Legal Council, Board Secretaries)
            var recipients = GetAlternative2NotificationRecipients(fundDetails);

            foreach (var userId in recipients)
            {
                var notification = new Domain.Entities.Notifications.Notification
                {
                    UserId = userId,
                    FundId = fundDetails.Id,
                    Title = string.Empty,
                    Body = $"{fundDetails.Name}|{userRole}|{_currentUserService.UserName}|{originalResolution.Code}",
                    NotificationType = (int)NotificationType.NewResolutionCreatedFromApproved, // MSG009
                    IsRead = false
                };

                notifications.Add(notification);
            }

            if (notifications.Any())
            {
                await _repository.Notifications.AddRangeAsync(notifications);
                _logger.LogInfo($"Alternative 2 notifications added for Resolution ID: {newResolution.Id}, Original Resolution: {originalResolution.Code}, Count: {notifications.Count}");
            }
        }

        /// <summary>
        /// Gets notification recipients for Alternative 2 (MSG009)
        /// Notifies Fund Managers, Legal Council, and Board Secretaries
        /// </summary>
        private List<int> GetAlternative2NotificationRecipients(Fund fundDetails)
        {
            var recipients = new List<int>();

            // Add Fund Managers
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

            // Add Legal Council
            if (fundDetails.LegalCouncilId > 0 && !recipients.Contains(fundDetails.LegalCouncilId))
            {
                recipients.Add(fundDetails.LegalCouncilId);
            }

            // Add Board Secretaries
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

            return recipients;
        }

        /// <summary>
        /// Validates if the current user has permission to create a resolution for the specified fund
        /// Ensures that only fund managers can create resolutions for their funds
        /// Implements proper authorization as required by JDWA-509 and Sprint.md requirements
        /// </summary>
        /// <param name="fund">The fund for which the resolution is being created</param>
        /// <param name="currentUserId">The current user ID</param>
        /// <param name="currentUserRoles">The current user's roles</param>
        /// <returns>True if user has permission to create resolution, false otherwise</returns>
        private async Task<bool> HasCreatePermission(Fund fund, int currentUserId, Roles userRole)
        {
            // For normal resolution creation, only Fund Managers can create resolutions
            if (userRole == Roles.FundManager)
            {
                return true;
            }
            return false;
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
