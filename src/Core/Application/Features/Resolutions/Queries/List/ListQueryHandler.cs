using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using Application.Features.Resolutions.Dtos;
using AutoMapper;
using Domain.Entities.ResolutionManagement;
using Domain.Services;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Constants;
namespace Application.Features.Resolutions.Queries.List
{
    /// <summary>
    /// Handler for ListQuery to retrieve paginated resolution list
    /// Implements role-based access control and comprehensive filtering
    /// Based on Sprint.md requirements (JDWA-582)
    /// </summary>
    public class ListQueryHandler : BaseResponseHandler, IQueryHandler<ListQuery, PaginatedResult<SingleResolutionResponse>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructor(s)
        public ListQueryHandler(
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

        #region Functions
        public async Task<PaginatedResult<SingleResolutionResponse>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo("Starting GetResolutionsList operation");

                // 1. Get current user information for role-based filtering
                var currentUserId = _currentUserService.UserId;
                var userRole = await GetUserFundRole(request.FundId.Value, _currentUserService.UserId.Value);

                _logger.LogInfo($"Current User - ID: {currentUserId}, Roles: {string.Join(", ", userRole)}");

                // Specify the type argument explicitly to resolve CS0411  

                var result = _repository.Resolutions.GetResolutionsByRoleAsync(request.FundId.Value, request.Search , request.ResolutionTypeId, request.Status , request.FromDate , request.ToDate, userRole.ToString(), false)
                            .OrderByDescending(x => x.UpdatedAt)
                            .Where(x => x.IsDeleted == false || x.IsDeleted == null);

                //   var result = _repository.Resolutions.GetResolutionsByRoleAsync(request.FundId.Value, userRole.ToString(), false);
                // .OrderByDescending(x => x.UpdatedAt)
                //.Where(x => x.IsDeleted == false || x.IsDeleted == null);

                // Start with base query - ResolutionType will be loaded separately for mapping
                //var result = _repository.Resolutions.GetAll<Resolution>(false)
                //    .OrderByDescending(x => x.UpdatedAt)
                //    .Where(x => x.IsDeleted == false || x.IsDeleted == null);

                //result = new (bool apply, Expression<Func<Resolution, bool>> filter)[]
                //{
                //    (!string.IsNullOrWhiteSpace(request.Search), x => x.Code.Contains(request.Search ?? "")),
                //    (request.FundId.HasValue , x => x.FundId == request.FundId),
                //    (request.ResolutionTypeId.HasValue , x => x.ResolutionTypeId == request.ResolutionTypeId),
                //    (request.Status.HasValue , x => x.Status == request.Status),
                //    (request.FromDate.HasValue, x => x.ResolutionDate.Date >= request.FromDate.Value.Date),
                //    (request.ToDate.HasValue, x => x.ResolutionDate.Date <= request.ToDate.Value.Date)
                //}
                //.Where(x => x.apply)
                //.Aggregate(result, (q, x) => q.Where(x.filter));

                if (!result.Any())
                {
                    return PaginatedResult<SingleResolutionResponse>.EmptyCollection(_localizer[SharedResourcesKey.NoRecords]);
                }

                // 6. Project to DTO and paginate
                var ResolutionList = await _mapper.ProjectTo<SingleResolutionResponse>(result).ToPaginatedListAsync(request.PageNumber, request.PageSize, "LastUpdated desc");

                // 7. Set action permissions for all resolutions based on user role
                await SetActionPermissionsForList(ResolutionList.Data, currentUserId, userRole,request.FundId.GetValueOrDefault());

                _logger.LogInfo($"Retrieved {ResolutionList.Data.Count} resolutions out of {ResolutionList.TotalCount} total");

                return ResolutionList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetResolutionsList");
                return PaginatedResult<SingleResolutionResponse>.ServerError(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);
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
        private async Task<Roles> GetUserFundRole(int fundId, int currentUserId)
        {
            try
            {
                _logger.LogInfo($"Checking fund roles for User ID: {currentUserId} in Fund ID: {fundId}");

                var userRole = Roles.None;
                // Get fund details with all related entities
                var fundDetails = await _repository.Funds.ViewFundUsers(fundId, trackChanges: false);
                if (fundDetails == null)
                {
                    _logger.LogWarn($"Fund not found with ID: {fundId}");
                    return Roles.None;
                }

                // 1. Check if user is Legal Council for the fund
                if (fundDetails.LegalCouncilId == currentUserId)
                {
                    userRole = Roles.LegalCouncil;
                    _logger.LogInfo($"User ID: {currentUserId} is Legal Council for Fund ID: {fundId}");
                }

                // 2. Check if user is a Fund Manager for the fund
                if (fundDetails.FundManagers != null && fundDetails.FundManagers.Count > 0)
                {
                    var isFundManager = fundDetails.FundManagers.Any(fm => fm.UserId == currentUserId);
                    if (isFundManager)
                    {
                        userRole = Roles.FundManager;
                        _logger.LogInfo($"User ID: {currentUserId} is Fund Manager for Fund ID: {fundId}");
                    }
                }

                // 3. Check if user is a Board Secretary for the fund
                if (fundDetails.FundBoardSecretaries != null && fundDetails.FundBoardSecretaries.Count > 0)
                {
                    var isBoardSecretary = fundDetails.FundBoardSecretaries.Any(bs => bs.UserId == currentUserId);
                    if (isBoardSecretary)
                    {
                        userRole = Roles.BoardSecretary;
                        _logger.LogInfo($"User ID: {currentUserId} is Board Secretary for Fund ID: {fundId}");
                    }
                }

                // Return comma-separated roles or empty string if no roles found

                _logger.LogInfo($"User ID: {currentUserId} has roles in Fund ID: {fundId}: '{userRole}'");

                return userRole;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking fund roles for User ID: {currentUserId} in Fund ID: {fundId}");
                return Roles.None;
            }
        }

        /// <summary>
        /// Sets action permissions for all resolutions in the list based on user role and resolution status
        /// Optimized to minimize database calls by grouping resolutions by fund
        /// </summary>
        /// <param name="resolutions">List of resolution DTOs to set permissions for</param>
        /// <param name="userRole">Current user's role</param>
        /// <param name="currentUserId">Current user's ID</param>
        private async Task SetActionPermissionsForList(IEnumerable<SingleResolutionResponse> resolutions, int? currentUserId , Roles userFundRole,int fundId)
        {
            if (!resolutions.Any() || !currentUserId.HasValue) return;

            foreach (var resolution in resolutions)
            {
                await SetActionPermissionsAsync(resolution, userFundRole,fundId,resolution.CreatorID);
            }
        }

        /// <summary>
        /// Sets action permissions based on user role and resolution status
        /// Based on Sprint.md requirements for user stories JDWA-588, JDWA-589, JDWA-593, JDWA-584
        /// Includes CanCancel and CanDelete flags based on ResolutionDomainService business rules
        /// </summary>
        private async Task SetActionPermissionsAsync(SingleResolutionResponse response, Roles userRole,int fundId , int creatorID)
        {
            var isFundManager = userRole == Roles.FundManager;
            var isResolutionOwner = userRole == Roles.FundManager;
            var isLegalCouncilOrBoardSecretary = userRole == Roles.LegalCouncil || userRole == Roles.BoardSecretary;

            // Default all actions to false
            response.CanConfirm = false;
            response.CanReject = false;
            response.CanEdit = false;
            response.CanCancel = false;
            response.CanDelete = false;
            response.CanView = false;

            // Set permissions based on status and role
            switch (response.Status)
            {
                case ResolutionStatusEnum.Draft:
                    response.CanEdit = isFundManager && _currentUserService.UserId == creatorID;
                    response.CanDelete = _currentUserService.UserId == creatorID && isFundManager && ResolutionDomainService.CanDeleteResolution(response.Status);
                    response.CanView = isFundManager && _currentUserService.UserId == creatorID;
                    break;

                case ResolutionStatusEnum.Pending:
                    response.CanEdit = isFundManager || isLegalCouncilOrBoardSecretary;
                    response.CanCancel = isFundManager && ResolutionDomainService.CanCancelResolution(response.Status);
                    response.CanView = isFundManager || isLegalCouncilOrBoardSecretary;
                    break;

                case ResolutionStatusEnum.WaitingForConfirmation:
                    response.CanEdit = isLegalCouncilOrBoardSecretary;
                    response.CanConfirm = isFundManager && _currentUserService.UserId == creatorID;
                    response.CanReject = isFundManager && _currentUserService.UserId == creatorID;  
                    break;

                case ResolutionStatusEnum.VotingInProgress:
                case ResolutionStatusEnum.Approved:
                case ResolutionStatusEnum.NotApproved:
                case ResolutionStatusEnum.CompletingData:
                case ResolutionStatusEnum.Confirmed:
                case ResolutionStatusEnum.Rejected:
                    response.CanEdit = isLegalCouncilOrBoardSecretary;
                    response.CanView = isLegalCouncilOrBoardSecretary;
                    break;
                // Read-only for all these statuses
                case ResolutionStatusEnum.Cancelled:
                    response.CanView = isFundManager;
                    break;
                default:
                    // Unknown status, no actions allowed
                    break;
            }
        }
         


        #endregion
    }
}

