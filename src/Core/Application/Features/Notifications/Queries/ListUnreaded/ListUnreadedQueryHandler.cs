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
using Domain.Entities.Notifications;
using Abstraction.Contract.Service.Notifications;



namespace Application.Features.Notifications.Queries.List
{
    public class ListUnreadedQueryHandler : BaseResponseHandler, IQueryHandler<ListUnreadedQuery, PaginatedResult<NotificationDto>>
    {
        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationLocalizationService? _localizationService;


        #endregion

        #region Constructor(s)
        public ListUnreadedQueryHandler(IRepositoryManager repository,
                                IServiceManager Service,
                                IMapper mapper,
                                ILoggerManager logger,
                                IStringLocalizer<SharedResources> localizer,
                                INotificationLocalizationService? localizationService,
                                ICurrentUserService currentUserService)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _localizationService = localizationService;
        }
        #endregion

        #region Functions
        public async Task<PaginatedResult<NotificationDto>> Handle(ListUnreadedQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = _repository.Notifications.GetAll<Domain.Entities.Notifications.Notification>(false);

                result = result.Where(n => n.IsRead == false && n.UserId == _currentUserService.UserId.GetValueOrDefault());

                if (!result.Any())
                {
                    return new PaginatedResult<NotificationDto>();
                }
                 
                var NotificationList = await _mapper.ProjectTo<NotificationDto>(result).ToPaginatedListAsync(request.PageNumber, request.PageSize, request.OrderBy);
                foreach (var item in NotificationList.Data)
                {
                    var localizedNotification = await _localizationService.GetLocalizedNotificationAsync(item.UserId,(NotificationType)item.NotificationType,
                    item.Body.Contains('|') ? item.Body.Split('|') : new object[] { item.Body });
                    item.Body = localizedNotification.Body;
                    item.Title = localizedNotification.Title;
                }
                return NotificationList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in ListQuery");
                return PaginatedResult<NotificationDto>.ServerError(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);

            }
        }
        #endregion
    }
}
