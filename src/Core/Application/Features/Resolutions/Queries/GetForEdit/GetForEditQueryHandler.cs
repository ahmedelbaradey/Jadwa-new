using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;

using Application.Features.Resolutions.Dtos;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.Shared;
using Domain.Services;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Abstraction.Constants;
using Domain.Entities.FundManagement;
using System;
using Abstraction.Contract.Service.Storage;

namespace Application.Features.Resolutions.Queries.GetForEdit
{
    /// <summary>
    /// Handler for GetForEditQuery to retrieve minimal resolution data for edit screen
    /// Implements role-based access control with optimized data loading
    /// Returns only essential fields needed for editing to minimize response size
    /// </summary>
    public class GetForEditQueryHandler : BaseResponseHandler, IQueryHandler<GetForEditQuery, BaseResponse<ResolutionForEditDto>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPreviewUrlHelper _previewUrlHelper;
        #endregion

        #region Constructor(s)
        public GetForEditQueryHandler(
            IRepositoryManager repository,
            IMapper mapper,
            ILoggerManager logger,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService,
            IPreviewUrlHelper previewUrlHelper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _previewUrlHelper = previewUrlHelper;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<ResolutionForEditDto>> Handle(GetForEditQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Starting GetResolutionForEdit operation for ID: {request.Id}");

                // 1. Validate request
                if (request.Id <= 0)
                    return BadRequest<ResolutionForEditDto>(_localizer[SharedResourcesKey.InvalidIdValidation]);

                // 2. Get current user information for role-based access control
                var currentUserId = _currentUserService.UserId;
                var currentUserRoles = _currentUserService.Roles;

                // 3. Retrieve resolution with minimal required data for editing
                var resolution = await _repository.Resolutions.GetResolutionForEditAsync(request.Id, trackChanges: false);
                if (resolution == null)
                {
                    _logger.LogWarn($"Resolution not found with ID: {request.Id}");
                    return NotFound<ResolutionForEditDto>(_localizer[SharedResourcesKey.ResolutionNotFound]);
                }
    
                // 4. Get fund information for access control
                var fund = await _repository.Funds.ViewFundUsers(resolution.FundId, trackChanges: false);
                if (fund == null)
                {
                    _logger.LogWarn($"Fund not found for resolution ID: {request.Id}");
                    return NotFound<ResolutionForEditDto>(_localizer[SharedResourcesKey.FundNotFound]);
                }

                var userRole = await GetUserFundRole(fund, _currentUserService.UserId.Value);
                // 5. Role-based access control validation with status filtering
                if (!await HasAccessToResolution(resolution, fund, currentUserId, userRole))
                {
                    _logger.LogWarn($"User {currentUserId} does not have access to resolution {request.Id}");
                    return Unauthorized<ResolutionForEditDto>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // 6. Build lightweight resolution response for edit screen
                var response = _mapper.Map<ResolutionForEditDto>(resolution);

                // 7. Populate preview URLs for attachments
                await PopulateAttachmentPreviewUrls(response, cancellationToken);

                _logger.LogInfo($"Resolution edit data retrieved successfully for ID: {request.Id}");
                return Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in GetResolutionForEdit for ID: {request.Id}");
                return ServerError<ResolutionForEditDto>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        #endregion

        /// <summary>
        /// Validates if the current user has access to view the resolution
        /// Based on Sprint.md role-based access requirements with status filtering
        /// </summary>
        private async Task<bool> HasAccessToResolution(Resolution resolution, Fund fund, int? currentUserId, Roles userRole)
        {
            // First check fund association for all roles
            bool hasFundAccess = false;

            // Fund Manager: Can view resolutions for their funds with specific statuses
            if (userRole == Roles.FundManager)
            {
                // Check if user is fund manager for this fund
                // var fundDetails = await _repository.Funds.ViewFundDetails(fund.Id, trackChanges: false);
                hasFundAccess = fund?.FundManagers?.Any(fm => fm.UserId == currentUserId) ?? false;

                if (hasFundAccess)
                {
                    // Fund Manager can view all statuses
                    return true;
                }
            }

            // Legal Council: Can view resolutions with specific statuses
            // Board Secretary: Can view resolutions with specific statuses
            if (userRole == Roles.LegalCouncil || userRole == Roles.BoardSecretary)
            {
                // Legal Council can view: Pending, Cancelled, CompletingData, WaitingForConfirmation, Confirmed, Rejected
                var allowedStatuses = new[]
                {
                    ResolutionStatusEnum.Pending,
                    ResolutionStatusEnum.Cancelled,
                    ResolutionStatusEnum.CompletingData,
                    ResolutionStatusEnum.WaitingForConfirmation,
                    ResolutionStatusEnum.Confirmed,
                    ResolutionStatusEnum.Approved,
                    ResolutionStatusEnum.NotApproved,
                    ResolutionStatusEnum.Rejected,
                    ResolutionStatusEnum.VotingInProgress
                };
                return allowedStatuses.Contains(resolution.Status);
            }

            // Board Member: Can view resolutions for funds they are members of (voting statuses only)
            if (userRole == Roles.BoardMember)
            {
                var boardMembers = await _repository.BoardMembers.GetActiveBoardMembersByFundIdAsync(fund.Id);
                hasFundAccess = boardMembers.Any(bm => bm.UserId == currentUserId && bm.IsActive);

                if (hasFundAccess)
                {
                    // Board members can only see voting and completed resolutions
                    var allowedStatuses = new[]
                    {
                        ResolutionStatusEnum.VotingInProgress,
                        ResolutionStatusEnum.Approved,
                        ResolutionStatusEnum.NotApproved
                    };
                    return allowedStatuses.Contains(resolution.Status);
                }
            }

            return false; // No access by default
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
        /// Populates preview URLs for all attachments in the edit response
        /// </summary>
        private async Task PopulateAttachmentPreviewUrls(ResolutionForEditDto response, CancellationToken cancellationToken)
        {
            try
            {
                // Populate main attachment preview URL
                if (response.Attachment != null)
                {
                    var attachment = await _repository.Attachments.GetByIdAsync<Attachment>(response.Attachment.Id, trackChanges: false);
                    if (attachment != null)
                    {
                        response.Attachment.FilePath = await _previewUrlHelper.GeneratePreviewUrlAsync(
                            attachment.Path,
                            attachment.ModuleId,
                            cancellationToken);
                    }
                }

                // Populate other attachments preview URLs
                if (response.OtherAttachments?.Any() == true)
                {
                    foreach (var attachmentDto in response.OtherAttachments)
                    {
                        var attachment = await _repository.Attachments.GetByIdAsync<Attachment>(attachmentDto.Id, trackChanges: false);
                        if (attachment != null)
                        {
                            attachmentDto.FilePath = await _previewUrlHelper.GeneratePreviewUrlAsync(
                                attachment.Path,
                                attachment.ModuleId,
                                cancellationToken);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error populating attachment preview URLs for resolution edit");
                // Don't fail the entire request if preview URL generation fails
            }
        }

    }
}
