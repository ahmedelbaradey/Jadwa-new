using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using Application.Common.Helpers;
using Application.Features.BoardMembers.Dtos;
using AutoMapper;
using Domain.Entities.FundManagement;
using Microsoft.Extensions.Localization;
using Resources;


namespace Application.Features.BoardMembers.Queries.List
{
    /// <summary>
    /// Handler for GetBoardMembersQuery
    /// Implements business logic for retrieving board members with filtering
    /// Follows Clean Architecture and CQRS patterns
    /// </summary>
    public class ListQueryHandler : BaseResponseHandler, IQueryHandler<ListQuery, PaginatedResult<BoardMemberDto>>
    {
        #region Fields

        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ILoggerManager _logger;

        #endregion

        #region Constructor

        public ListQueryHandler(
            IRepositoryManager repository,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            ILoggerManager logger)
        {
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _logger = logger;
        }

        #endregion

        #region Handler Implementation

        public async Task<PaginatedResult<BoardMemberDto>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Starting GetBoardMembers operation for FundId: {request.FundId}");
                if (request == null)
                    return PaginatedResult<BoardMemberDto>.ServerError(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);
                // 1. Validate fund exists
                var fund = await _repository.Funds.GetByIdAsync<Fund>(request.FundId, trackChanges: false);
                if (fund == null)
                {
                    _logger.LogWarn($"Fund not found with ID: {request.FundId}");
                    return PaginatedResult<BoardMemberDto>.ServerError(_localizer[SharedResourcesKey.FundNotFound]);
                }

                // 2. Get board members based on filters
                var result = _repository.BoardMembers.GetBoardMembersByTypeAsync(request.FundId);
                if (!result.Any())
                {
                    return PaginatedResult<BoardMemberDto>.EmptyCollection(_localizer[SharedResourcesKey.NoRecords]);
                }
                var memberList = await _mapper.ProjectTo<BoardMemberDto>(result).ToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy);

                memberList.Data.ForEach(x => x.MemberTypeDisplay = LocalizationHelper.GetBoardMemberTypeDisplay(x.MemberType, _localizer));

                return memberList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred while retrieving board members for FundId: {request.FundId}");
                return PaginatedResult<BoardMemberDto>.ServerError(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }

        #endregion
    }
}
