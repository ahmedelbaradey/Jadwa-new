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
using Application.Features.Notifications.Dtos;
using Abstraction.Contract.Service;



namespace Application.Features.Notifications.Queries.List
{
    public class ListQueryHandler : BaseResponseHandler, IQueryHandler<ListQuery, BaseResponse<int>>
    {
        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;

        #endregion

        #region Constructor(s)
        public ListQueryHandler(IRepositoryManager repository,
                                IServiceManager Service,
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
        public async Task<BaseResponse<int>> Handle(ListQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.Notifications.GetUnReadCountByUserId(_currentUserService.UserId.Value,false);

                return Success(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in ListQuery");
                return ServerError<int>(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);
            }
        }
        #endregion
    }
}
