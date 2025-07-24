using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using Application.Features.Funds.Dtos;
using AutoMapper;
using Abstraction.Contracts.Logger;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using MediatR;
using Domain.Entities.Notifications;
using Abstraction.Contract.Service.Notifications;
using Abstraction.Common.Wappers;

namespace Application.Features.Funds.Queries.GetFundBasicInfo
{
    /// <summary>
    /// Handler for GetFundBasicInfoQuery
    /// Retrieves the initiation date and voting type information for a specific fund
    /// </summary>
    public class GetFundNotificationQueryHandler : BaseResponseHandler, IQueryHandler<GetFundNotificationQuery, PaginatedResult<FundNotificationDto>>
    {
        private readonly ILoggerManager _logger;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly INotificationLocalizationService? _localizationService;


        public GetFundNotificationQueryHandler(IRepositoryManager repositoryManager,
                                               IMapper mapper,
                                               ILoggerManager logger,
                                               IStringLocalizer<SharedResources> localizer,
                                               INotificationLocalizationService? localizationService,
                                               ICurrentUserService currentUserService)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _logger = logger;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _localizationService = localizationService;
        }

        /// <summary>
        /// Handles the GetFundNotificationQuery to retrieve Notifications about a fund by its ID
        /// </summary>
        /// <param name="request">The query request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Fund basic information including initiation date and voting type</returns>
        public async Task<PaginatedResult<FundNotificationDto>> Handle(GetFundNotificationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Getting basic information for Fund ID: {request.FundId}");
                var result = _repositoryManager.Notifications.GetByCondition<Domain.Entities.Notifications.Notification>(x => x.FundId == request.FundId && x.UserId == _currentUserService.UserId);
                if (!result.Any())
                {
                    _logger.LogInfo($"No notifications found for Fund ID: {request.FundId}");
                    return new PaginatedResult<FundNotificationDto>();
                }
                var notifications = await _mapper.ProjectTo<FundNotificationDto>(result).ToPaginatedListAsync(request.PageNumber, request.PageSize, "CreatedAt Desc");
                foreach (var item in notifications.Data)
                {
                    var localizedNotification = await _localizationService.GetLocalizedNotificationAsync(
                      item.UserId,
                    (NotificationType)item.NotificationType,
                    item.Body.Contains('|') ? item.Body.Split('|') : new object[] { item.Body });
                    item.Message = localizedNotification.Body;
                    item.Title = localizedNotification.Title;
                }
                _logger.LogInfo($"Successfully retrieved basic information for Fund ID: {request.FundId}");
                return notifications;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting basic information for Fund ID: {request.FundId}");
                return PaginatedResult<FundNotificationDto>.ServerError(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);
            }
        }
    }
}
