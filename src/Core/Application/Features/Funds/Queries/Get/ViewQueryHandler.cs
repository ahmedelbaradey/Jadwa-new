using AutoMapper;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Funds.Dtos;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Domain.Entities.FundManagement;
using Abstraction.Contract.Service.Notifications;
using Domain.Entities.Notifications;
using Abstraction.Constants;
using Microsoft.Extensions.Localization;
using Resources;


namespace Application.Features.Funds.Queries.Get
{
    public class ViewQueryHandler : BaseResponseHandler, IQueryHandler<ViewQuery, BaseResponse<FundDetailsResponse>>
    {
        #region Fileds
        private readonly ILoggerManager _logger;
        protected IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly INotificationLocalizationService? _localizationService;
        private readonly IStringLocalizer<SharedResources> _localizer;

        #endregion

        #region Constructor(s)
        public ViewQueryHandler(IRepositoryManager repository,
                                         IMapper mapper,
                                         ILoggerManager logger,
                                         INotificationLocalizationService? localizationService,
                                         IStringLocalizer<SharedResources> localizer)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizationService = localizationService;
            _localizer = localizer;
        }
        #endregion

        #region Functions
        public async Task<BaseResponse<FundDetailsResponse>> Handle(ViewQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _repository.Funds.ViewFundDetails(request.Id, false);
                if (result == null)
                    return NotFound<FundDetailsResponse>("Fund with this Id not found!");
                foreach (var item in result.Notifications)
                {
                    var localizedNotification = await _localizationService.GetLocalizedNotificationAsync(
                      item.UserId,
                    (NotificationType)item.NotificationType,
                    item.Body.Contains('|') ? item.Body.Split('|') : new object[] { item.Body });
                    item.Body = localizedNotification.Body;
                    item.Title = localizedNotification.Title;
                }  
                var resultMapper = _mapper.Map<FundDetailsResponse>(result);
                resultMapper.MembersNotificationCount = await _repository.Notifications.GetNotificationCountByTypeAndFundId(request.Id, (int)NotificationModule.Members);
                resultMapper.ResolutionsNotificationCount = await _repository.Notifications.GetNotificationCountByTypeAndFundId(request.Id, (int)NotificationModule.Resolutions);
                resultMapper.FundHistory.ForEach(fh => fh.RoleName = GetRoleName(fh.UserId, result));
                return Success(resultMapper);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in GetResultByIdQuery");
                return ServerError<FundDetailsResponse>(ex.Message);
            }
        }

        #endregion
        private string GetRoleName(int userId, Fund fund)
        {
            return fund.FundManagers.Any(fb => fb.UserId == userId) ? _localizer[Roles.FundManager.ToString()] :
                fund.FundBoardSecretaries.Any(fb => fb.UserId == userId) ? _localizer[Roles.BoardSecretary.ToString()]
                : _localizer[Roles.LegalCouncil.ToString()];
        }
    }
}
