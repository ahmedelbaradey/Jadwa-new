using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Application.Features.Resolutions.Dtos;
using Domain.Entities.ResolutionManagement;
using System.ComponentModel;
using Abstraction.Contract.Service;
using Abstraction.Constants;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;

namespace Application.Features.Resolutions.Queries.GetStatuses
{
    /// <summary>
    /// Handler for getting all available resolution statuses
    /// Returns localized status information for frontend dropdowns and filters
    /// Uses LocalizedDto pattern for automatic culture-based localization
    /// Implements role-based filtering: Fund Manager (all), Legal Council/Board Secretary (all except draft), Board Members (voting statuses only)
    /// </summary>
    public class GetResolutionStatusesQueryHandler : BaseResponseHandler, IQueryHandler<GetResolutionStatusesQuery, BaseResponse<List<ResolutionStatusDto>>>
    {
        #region Fields
        private readonly ICurrentUserService _currentUserService;
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        #endregion

        #region Constructor
        public GetResolutionStatusesQueryHandler(ICurrentUserService currentUserService,
              ILoggerManager logger,
              IRepositoryManager repository
            )
        {
            _currentUserService = currentUserService;
            _logger = logger;
            _repository = repository;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<List<ResolutionStatusDto>>> Handle(GetResolutionStatusesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var statuses = new List<ResolutionStatusDto>();

                // Get all enum values
                var enumValues = Enum.GetValues<ResolutionStatusEnum>();

                // Get user role within specific fund context
                Roles userRole = await GetUserFundRole(request.FundId.Value, _currentUserService.UserId.Value);
                var allowedStatuses = GetAllowedStatusesForUser(userRole);

                foreach (var status in enumValues)
                {
                    // Filter statuses based on user role
                    if (!allowedStatuses.Contains(status))
                        continue;

                    var statusDto = new ResolutionStatusDto
                    {
                        Value = status,
                        Id = (int)status,
                        NameEn = GetEnglishStatusName(status),
                        NameAr = GetArabicStatusName(status),
                        Description = GetStatusDescription(status)
                    };

                    statuses.Add(statusDto);
                }

                // Sort by ID for consistent ordering
                statuses = statuses.OrderBy(s => s.Id).ToList();

                return Success(statuses);
            }
            catch (Exception ex)
            {
                return ServerError<List<ResolutionStatusDto>>($"Error retrieving resolution statuses: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets allowed statuses based on user role
        /// Fund Manager: All statuses
        /// Legal Council/Board Secretary: All statuses except Draft
        /// Board Members: VotingInProgress, Approved, NotApproved
        /// </summary>
        private HashSet<ResolutionStatusEnum> GetAllowedStatusesForUser(Roles userRole)
        {
            switch (userRole)
            {
                case Roles.FundManager:
                    return new HashSet<ResolutionStatusEnum>(Enum.GetValues<ResolutionStatusEnum>());
                case Roles.LegalCouncil:
                case Roles.BoardSecretary:
                    var allStatuses = new HashSet<ResolutionStatusEnum>(Enum.GetValues<ResolutionStatusEnum>());
                    allStatuses.Remove(ResolutionStatusEnum.Draft);
                    return allStatuses;
                case Roles.BoardMember:
                    return new HashSet<ResolutionStatusEnum>
                {
                    ResolutionStatusEnum.VotingInProgress,
                    ResolutionStatusEnum.Approved,
                    ResolutionStatusEnum.NotApproved
                };
                default:
                    // Default: All statuses
                    return new HashSet<ResolutionStatusEnum>(Enum.GetValues<ResolutionStatusEnum>());
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


                // 4. Check if user is a Board Memeber for the fund
                if (fundDetails.BoardMembers != null && fundDetails.BoardMembers.Count > 0)
                {
                    var isBoardMember = fundDetails.BoardMembers.Any(bs => bs.UserId == currentUserId);
                    if (isBoardMember)
                    {
                        userRole = Roles.BoardMember;
                        _logger.LogInfo($"User ID: {currentUserId} is Board Member for Fund ID: {fundId}");
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
        /// Gets the English status name
        /// </summary>
        private string GetEnglishStatusName(ResolutionStatusEnum status)
        {
            return status switch
            {
                ResolutionStatusEnum.Draft => "Draft",
                ResolutionStatusEnum.Pending => "Pending",
                ResolutionStatusEnum.CompletingData => "Completing Data",
                ResolutionStatusEnum.WaitingForConfirmation => "Waiting for Confirmation",
                ResolutionStatusEnum.Confirmed => "Confirmed",
                ResolutionStatusEnum.Rejected => "Rejected",
                ResolutionStatusEnum.VotingInProgress => "Voting in Progress",
                ResolutionStatusEnum.Approved => "Approved",
                ResolutionStatusEnum.NotApproved => "Not Approved",
                ResolutionStatusEnum.Cancelled => "Cancelled",
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Gets the Arabic status name
        /// </summary>
        private string GetArabicStatusName(ResolutionStatusEnum status)
        {
            return status switch
            {
                ResolutionStatusEnum.Draft => "مسودة",
                ResolutionStatusEnum.Pending => "معلق",
                ResolutionStatusEnum.CompletingData => "استكمال البيانات",
                ResolutionStatusEnum.WaitingForConfirmation => "في انتظار التأكيد",
                ResolutionStatusEnum.Confirmed => "مؤكد",
                ResolutionStatusEnum.Rejected => "مرفوض",
                ResolutionStatusEnum.VotingInProgress => "التصويت قيد التقدم",
                ResolutionStatusEnum.Approved => "معتمد",
                ResolutionStatusEnum.NotApproved => "غير معتمد",
                ResolutionStatusEnum.Cancelled => "ملغي",
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Gets the status description from the Description attribute
        /// </summary>
        private string GetStatusDescription(ResolutionStatusEnum status)
        {
            var field = status.GetType().GetField(status.ToString());
            var attribute = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                                 .FirstOrDefault() as DescriptionAttribute;
            return attribute?.Description ?? status.ToString();
        }

        #endregion
    }
}
