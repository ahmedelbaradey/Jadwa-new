using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Features.Funds.Dtos;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Service;
using Microsoft.Extensions.Localization;
using Resources;
using Application.Features.Identity.Users.Queries.Responses;
using Abstraction.Contracts.Repository;
using Domain.Entities.FundManagement;
using System.Linq.Expressions;
using Abstraction.Contract.Service;



namespace Application.Features.Funds.Queries.List
{
    public class ListQueryHandler : BaseResponseHandler, IQueryHandler<ListQuery, PaginatedResult<FundGroupDto>>
    {
        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        ICurrentUserService _currentUserService;
        #endregion

        #region Constructor(s)
        public ListQueryHandler(IRepositoryManager repository,IServiceManager Service, IMapper mapper, ILoggerManager logger, IStringLocalizer<SharedResources> localizer, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Functions
        public async Task<PaginatedResult<FundGroupDto>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = _currentUserService.UserId.GetValueOrDefault();
                var result = _repository.Funds.GetAllAndInclude(false , userId);

                result = new (bool apply, Expression<Func<Fund, bool>> filter)[]
                {
                    (!string.IsNullOrWhiteSpace(request.Search), x => x.Name.Contains(request.Search)),
                    (request.StrategyId.HasValue && request.StrategyId != -1 && request.StrategyId != 0, x => x.StrategyId == request.StrategyId),
                    (request.CreationDateFrom.HasValue, x => x.InitiationDate.Date >= request.CreationDateFrom.Value.Date),
                    (request.CreationDateTo.HasValue, x => x.InitiationDate.Date <= request.CreationDateTo.Value.Date)
                }
                .Where(x => x.apply)
                .Aggregate(result, (q, x) => q.Where(x.filter));

                if (!result.Any())
                {
                    return new PaginatedResult<FundGroupDto>();
                }
                 
                var FundList = await _mapper.ProjectTo<SingleFundResponse>(result)
                    .GroupBy(f => f.StrategyName)
                    .Select(g => new FundGroupDto
                    {
                        Title = g.Key.ToString(),
                        Funds = g.OrderByDescending(f=>f.UpdatedAt).ToList(),
                        Count = g.Count()
                    })
                    .ToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy);

                return FundList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in ListQuery");
                return PaginatedResult<FundGroupDto>.ServerError(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);

            }
        }
        #endregion
    }
}
