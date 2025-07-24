using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Abstraction.Contracts.Identity;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using Application.Features.Identity.Users.Queries.Responses;
using AutoMapper;
using Domain.Entities.FundManagement;


namespace Application.Features.Identity.Users.Queries.List
{

    public class ListForBoardMembersQueryHandler : BaseResponseHandler, IQueryHandler<ListForBoardMembersQuery, PaginatedResult<GetUserListResponse>>
    {
        private readonly IRepositoryManager _repository;
        #region Fields
        private readonly IIdentityServiceManager _service;
        private readonly IMapper _mapper;
        private readonly ILoggerManager _logger;
        #endregion

        #region Constructors
        public ListForBoardMembersQueryHandler(IRepositoryManager repository, IIdentityServiceManager service, IMapper mapper, ILoggerManager logger)
        {
            _mapper = mapper;
            _repository = repository;
            _service = service;
            _logger = logger;
        }
        #endregion

        #region Functions
        public async Task<PaginatedResult<GetUserListResponse>> Handle(ListForBoardMembersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var fund = await _repository.Funds.ViewFundUsers(request.FundId, false);
                List<int> fundUserIds = new();
                fundUserIds.AddRange(fund.BoardMembers.Select(x => x.UserId).ToList());
                fundUserIds.AddRange(fund.FundBoardSecretaries.Select(x => x.UserId).ToList());
                fundUserIds.AddRange(fund.FundManagers.Select(x => x.UserId).ToList());
                fundUserIds.Add(fund.LegalCouncilId);

                var users = _service.UserManagmentService.Users().Where(x => !fundUserIds.Contains(x.Id)).AsQueryable();
                if (!users.Any())
                {
                    return PaginatedResult<GetUserListResponse>.EmptyCollection();
                }

                var paginatedList = await _mapper.ProjectTo<GetUserListResponse>(users).ToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy);
                return paginatedList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetUserPaginatedListQuery");
                return PaginatedResult<GetUserListResponse>.ServerError("Error in GetUserPaginatedListQuery");
            }
        }
        #endregion
    }
}
