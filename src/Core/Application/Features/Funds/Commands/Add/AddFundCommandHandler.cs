using Abstraction.Base.Response;
using Abstraction.Constants;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Abstraction.Contracts.Service;
using Application.Base.Abstracts;
using AutoMapper;
using Domain.Entities.FundManagement;
using Domain.Entities.FundManagement.State;
using Domain.Entities.Notifications;
using Microsoft.Extensions.Localization;
using Resources;



namespace Application.Features.Funds.Commands.Add
{
    public class AddFundCommandHandler : BaseResponseHandler, ICommandHandler<AddFundCommand, BaseResponse<string>>
    {

        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IStringLocalizer<SharedResources> _localizer;
        protected IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructors
        public AddFundCommandHandler(IRepositoryManager repository, IMapper mapper, IServiceManager Service, IStringLocalizer<SharedResources> localizer, ILoggerManager logger, ICurrentUserService currentUserService)
        {
            _logger = logger;
            _localizer = localizer;
            _repository = repository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(AddFundCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);
                var originalEntity = _mapper.Map<Fund>(request);
                if (_currentUserService.Roles.Contains(Roles.LegalCouncil.ToString().ToLower()))
                    originalEntity.SetState(new UnderConstructionFund());
                else
                    originalEntity.SetState(new NewFund());
                originalEntity.Handle();
                var fund = await _repository.Funds.AddAsync(originalEntity, cancellationToken);
                if (fund is null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);
                await AddNotification(request, fund.Name, fund.Id);
                return Success<string>(_localizer[SharedResourcesKey.FundSavedSuccessfully]);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in AddFundCommand");
                return ServerError<string>(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);
            }
        }
        private async Task AddNotification(AddFundCommand request, string fundName, int fundId)
        {
            try
            {
                var notifications = new List<Domain.Entities.Notifications.Notification>();
                var currentUserId = _currentUserService.UserId;

            var fundManagers = request.FundManagers?.Select(m => m) ?? Enumerable.Empty<int>();
            var boardSecretaries = request.FundBoardSecretaries?.Select(m => m) ?? Enumerable.Empty<int>();

                if (RoleHelper.IsFundManager(_currentUserService.Roles))
                {
                    // JDWA-187: Fund Manager creates fund
                    _logger.LogInfo($"Fund Manager creating fund notifications for Fund ID: {fundId}");

                    foreach (var managerId in fundManagers)
                    {
                        notifications.Add(new Domain.Entities.Notifications.Notification
                        {
                            Title = string.Empty, // Will be localized at send time
                            Body = $"{fundName}|{_currentUserService.UserName}|{Roles.FundManager}",
                            FundId = fundId,
                            UserId = managerId,
                            NotificationType = (int)NotificationType.AddedToFund, // MSG005
                            NotificationModule = (int)NotificationModule.Funds,
                        });
                    }

                    foreach (var secretaryId in boardSecretaries)
                    {
                        notifications.Add(new Domain.Entities.Notifications.Notification
                        {
                            Title = string.Empty, // Will be localized at send time
                            Body = $"{fundName}|{_currentUserService.UserName}|{Roles.BoardSecretary}",
                            FundId = fundId,
                            UserId = secretaryId,
                            NotificationType = (int)NotificationType.AddedToFundForManager, // MSG002
                            NotificationModule = (int)NotificationModule.Funds,
                        });
                    }

                    // MSG002: Notify legal council with "complete fund info" message
                    if (request.LegalCouncilId > 0)
                    {
                        notifications.Add(new Domain.Entities.Notifications.Notification
                        {
                            Title = string.Empty, // Will be localized at send time
                            Body = $"{fundName}|{_currentUserService.UserName}|{Roles.LegalCouncil}",
                            FundId = fundId,
                            UserId = request.LegalCouncilId,
                            NotificationType = (int)NotificationType.AddedToFundForManager, // MSG002
                            NotificationModule = (int)NotificationModule.Funds,
                        });
                    }
                }
                else if (RoleHelper.IsLegalCouncil(_currentUserService.Roles))
                {
                    // JDWA-186: Legal Council creates fund
                    _logger.LogInfo($"Legal Council creating fund notifications for Fund ID: {fundId}");

                    // MSG005: Notify fund managers
                    foreach (var managerId in fundManagers)
                    {
                        notifications.Add(new Domain.Entities.Notifications.Notification
                        {
                            Title = string.Empty, // Will be localized at send time
                            Body = $"{fundName}|{_currentUserService.UserName}|{Roles.FundManager}",
                            FundId = fundId,
                            UserId = managerId,
                            NotificationType = (int)NotificationType.AddedToFund, // MSG005
                            NotificationModule = (int)NotificationModule.Funds,
                        });
                    }

                    // MSG005: Notify board secretaries
                    foreach (var secretaryId in boardSecretaries)
                    {
                        notifications.Add(new Domain.Entities.Notifications.Notification
                        {
                            Title = string.Empty, // Will be localized at send time
                            Body = $"{fundName}|{_currentUserService.UserName}|{Roles.BoardSecretary}",
                            FundId = fundId,
                            UserId = secretaryId,
                            NotificationType = (int)NotificationType.AddedToFund, // MSG005
                            NotificationModule = (int)NotificationModule.Funds,
                        });
                    }
                }

                // Save notifications if any were created
                if (notifications.Any())
                {
                    await _repository.Notifications.AddRangeAsync(notifications);
                    _logger.LogInfo($"Fund creation notifications added for Fund ID: {fundId}, Count: {notifications.Count}");
                }
                else
                {
                    _logger.LogInfo($"No notifications to send for Fund ID: {fundId}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding fund creation notifications for Fund ID: {fundId}");
                // Don't throw - notification failure shouldn't break fund creation
            }
        }

        #endregion


    }
}
