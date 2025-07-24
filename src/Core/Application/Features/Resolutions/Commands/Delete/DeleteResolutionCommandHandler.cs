using Abstraction.Base.Response;
using Abstraction.Constants;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using AutoMapper;
using Domain.Entities.FundManagement;
using Domain.Entities.ResolutionManagement;
using Domain.Services;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Resolutions.Commands.Delete
{
    /// <summary>
    /// Handler for DeleteResolutionCommand
    /// Implements business logic for deleting draft resolutions with proper validation
    /// Based on Sprint.md requirements (JDWA-510) and existing Resolution patterns
    /// </summary>
    public class DeleteResolutionCommandHandler : BaseResponseHandler, ICommandHandler<DeleteResolutionCommand, BaseResponse<string>>
    {

        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public DeleteResolutionCommandHandler(
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
        public async Task<BaseResponse<string>> Handle(DeleteResolutionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Starting DeleteResolution operation for ID: {request.Id}");

                // 1. Validate request
                if (request == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.EmptyRequestValidation]);

                // 2. Get resolution entity
                var resolution = await _repository.Resolutions.GetByIdAsync<Resolution>(request.Id, trackChanges: true);
                if (resolution == null)
                {
                    _logger.LogWarn($"Resolution not found with ID: {request.Id}");
                    return NotFound<string>(_localizer[SharedResourcesKey.ResolutionNotFound]);
                }

                // 3. Validate business rules using domain service
                if (!ResolutionDomainService.CanDeleteResolution(resolution.Status))
                {
                    _logger.LogWarn($"Cannot delete resolution with status: {resolution.Status} for ID: {request.Id}");
                    return BadRequest<string>(_localizer[SharedResourcesKey.CannotDeleteNonDraftResolution]);
                }

                // 4. Get fund for access control validation
                var fundDetails = await _repository.Funds.ViewFundUsers(resolution.FundId, trackChanges: false);
                if (fundDetails == null)
                {
                    _logger.LogWarn($"Fund not found for resolution ID: {request.Id}");
                    return NotFound<string>(_localizer[SharedResourcesKey.FundNotFound]);
                }

                // 5. Validate user permissions (only fund managers can delete resolutions)
                var currentUserId = _currentUserService.UserId;

                if (!await HasDeletePermission(fundDetails, currentUserId))
                {
                    _logger.LogWarn($"User {currentUserId} does not have permission to delete resolution {request.Id}");
                    return Unauthorized<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                // 6. Add comprehensive audit entry before deletion
                var currentUserName = _currentUserService.UserName;
                var currentUserRole = Roles.FundManager;
                var localizationKey = SharedResourcesKey.AuditActionResolutionDeletion;

                // Initialize state pattern if not already initialized
                if (resolution.StateContext == null)
                {
                    resolution.InitializeState();
                }

                // Add comprehensive audit entry for deletion action
                var comprehensiveDetails = $"Resolution permanently deleted by {currentUserRole}: {currentUserName}. Resolution was in {resolution.Status} status before deletion. All associated data including resolution items, attachments, and history will be removed from the system. This action is irreversible and complies with data retention policies.";
                resolution.AddAuditEntry(ResolutionActionEnum.ResolutionDeletion,
                    $"Resolution deleted by fund manager ({currentUserName})",
                    localizationKey, currentUserId.Value, currentUserRole.ToString(), comprehensiveDetails);

                // 7. Delete resolution
                resolution.IsDeleted = true; // Mark as deleted
                var deleteResult = await _repository.Resolutions.UpdateAsync(resolution);
                //var deleteResult = await _repository.Resolutions.DeleteAsync(resolution);
                if (!deleteResult)
                {
                    _logger.LogError(null, $"Failed to delete resolution with ID: {request.Id}");
                    return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorDeletingData]);
                }

                _logger.LogInfo($"Resolution deleted successfully with ID: {request.Id}");
                return Success<string>(_localizer[SharedResourcesKey.ItemDeletedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteResolution for ID: {request.Id}");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorDeletingData]);
            }
        }

        /// <summary>
        /// Validates if the current user has permission to delete the resolution
        /// Based on Sprint.md role-based access requirements - only fund managers can delete
        /// </summary>
        private async Task<bool> HasDeletePermission(Fund fund, int? currentUserId)
        {

            // Check if user is fund manager for this fund
            return fund?.FundManagers?.Any(fm => fm.UserId == currentUserId) ?? false;
        }

        #endregion

    }
}
