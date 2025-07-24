using Abstraction.Base.Response;
using Abstraction.Constants;
using Abstraction.Contract.Service;
using Abstraction.Contracts.Logger;
using Abstraction.Contracts.Repository;
using Application.Base.Abstracts;
using Application.Services;
using AutoMapper;
using Domain.Entities.FundManagement;
using Domain.Entities.FundManagement.State;
using Domain.Entities.Notifications;
using Microsoft.Extensions.Localization;
using Resources;
namespace Application.Features.Funds.Commands.Edit
{
    public class SaveFundCommandHandler : BaseResponseHandler, ICommandHandler<SaveFundCommand, BaseResponse<string>>
    {

        #region Fileds
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;

        #endregion

        #region Constructors
        public SaveFundCommandHandler(IRepositoryManager repository,
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

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(SaveFundCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.EmptyRequestValidation]);

                // RBAC validation - Only Legal Council can edit fund data
                if (!_currentUserService.Roles.Contains(Roles.LegalCouncil.ToString().ToLower()))
                {
                    return BadRequest<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                var originalEntity = await _repository.Funds.EditFundById(request.Id, true);
                if (originalEntity == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);

                // Initialize FundStateContext for enhanced audit logging and notifications
                var fundStateContext = new FundStateContext(
                    originalEntity,
                    _localizer,
                    _repository,
                    _currentUserService
                );

                var currentStatus = originalEntity.GetCurrentStatusEnum();
                var isCompletionAction = currentStatus == FundStatusEnum.UnderConstruction ;
                #region Manage FundManagers
                var existingFundManagers = originalEntity.FundManagers.Where(u => u.IsDeleted is null || !u.IsDeleted.Value).ToList();
                // Use HashSet for faster lookup
                var newFundManagersIds = request.FundManagers.ToHashSet();
                var existingFundManagersIds = existingFundManagers.Select(u => u.UserId).ToHashSet();
                // Clone non-deleted users
                var CurrentFundManagers = existingFundManagers
                    .Where(u => u.IsDeleted is null || !u.IsDeleted.Value)
                    //.Select(u => new FundManager { UserId = u.UserId, IsDeleted = u.IsDeleted is null ? false : u.IsDeleted.Value })
                    .ToList();
                var undeletedFundMangers = existingFundManagersIds.Intersect(newFundManagersIds).ToList();
                var deletedFundManagers = existingFundManagersIds.Except(newFundManagersIds).ToList();
                var newlyAddedFundManagers = newFundManagersIds.Except(existingFundManagersIds).ToList();
                // Delete and Add new
                existingFundManagers.Where(u => deletedFundManagers.Contains(u.UserId)).ToList().ForEach(u => u.IsDeleted = true);
                newlyAddedFundManagers.ForEach(r => { existingFundManagers.Add(new FundManager { FundId = originalEntity.Id, UserId = r, IsDeleted = false }); });

                #endregion


                #region Manage FundBoardSecretaries
                var existingBoardSecretaries = originalEntity.FundBoardSecretaries.Where(u => u.IsDeleted is null || !u.IsDeleted.Value).ToList();
                // Use HashSet for faster lookup
                var newBoardSecretariesIds = request.FundBoardSecretaries.ToHashSet();
                var existingBoardSecretariesIds = existingBoardSecretaries.Select(u => u.UserId).ToHashSet();
                // Clone non-deleted users
                var CurrentBoardSecretaries = existingBoardSecretaries
                    .Where(u => u.IsDeleted is null || !u.IsDeleted.Value)
                  //  .Select(u => new FundManager { UserId = u.UserId, IsDeleted = u.IsDeleted is null ? false : u.IsDeleted.Value })
                    .ToList();
                var undeletedBoardSecretariesIds = existingBoardSecretariesIds.Intersect(newBoardSecretariesIds).ToList();
                var deletedSecretaries = existingBoardSecretariesIds.Except(newBoardSecretariesIds).ToList();
                var newlyAddedBoardSecretariesIds = newBoardSecretariesIds.Except(existingBoardSecretariesIds).ToList();

                // Mark all as deleted, then un-delete matched
                existingBoardSecretaries.Where(u => deletedSecretaries.Contains(u.UserId)).ToList().ForEach(u => u.IsDeleted = true);
                newlyAddedBoardSecretariesIds.ForEach(r => { existingBoardSecretaries.Add(new FundBoardSecretary { FundId = originalEntity.Id, UserId = r, IsDeleted = false }); });

                #endregion


                //if (matchedIds.Any())
                //    //send delete notification to oldNotMatchedIds MSG003 
                //else
                //    //send notification to CurrentFundManagers MSG003

                // Then send notification to the matchedIds  MSG005
                // Then send notification to the matchedIds  MSG006

                // Apply fund data changes using AutoMapper
                var fundMapper = _mapper.Map(request, originalEntity);
                fundMapper.FundManagers.Clear();
                fundMapper.FundBoardSecretaries.Clear();
                fundMapper.FundManagers = existingFundManagers;
                fundMapper.FundBoardSecretaries = existingBoardSecretaries;

                // Handle state transitions with enhanced audit logging
                if (isCompletionAction)
                {
                    // Fund data completion: transition from UnderConstruction to WaitingForAddingMembers
                    var waitingState = new WaitingForAddingMembersFund();
                    var transitionSuccess = fundStateContext.ChangeStatusWithAudit(
                        waitingState,
                        FundActionEnum.FundDataCompletion,
                        _localizer[SharedResourcesKey.FundDataCompletionAction],
                        SharedResourcesKey.FundDataCompletionAction,
                        sendNotifications: false
                    );

                    if (!transitionSuccess)
                    {
                        _logger.LogWarn($"Failed to transition Fund {originalEntity.Id} from UnderConstruction to WaitingForAddingMembers");
                    }
                }
                else
                {
                    // Fund data edit: add audit entry without state change
                    fundStateContext.AddAuditEntry(
                        FundActionEnum.FundDataEdit,
                        _localizer[SharedResourcesKey.FundDataEditAction],
                        SharedResourcesKey.FundDataEditAction
                    );
                }

                var status = await _repository.Funds.UpdateAsync(fundMapper);

                if (!status)
                    return BadRequest<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);

                var allDeletedMembers = deletedFundManagers.Concat(deletedSecretaries);
                var allNewMembers = newlyAddedFundManagers.Concat(newlyAddedBoardSecretariesIds);

                await SendNotificationToAddedMembers(originalEntity.Id, originalEntity.Name, newlyAddedFundManagers, Roles.FundManager);
                await SendNotificationToAddedMembers(originalEntity.Id, originalEntity.Name, newlyAddedBoardSecretariesIds, Roles.BoardSecretary);
               
                await SendNotificationToRemovedMembers(originalEntity.Id, originalEntity.Name, deletedFundManagers, Roles.FundManager);
                await SendNotificationToRemovedMembers(originalEntity.Id, originalEntity.Name, deletedSecretaries, Roles.BoardSecretary);

                // Note: Fund completion/edit notifications are now handled by FundStateContext
                // No need for separate SendNotification call as it's integrated in ChangeStatusWithAudit

                return Success<string>(_localizer[SharedResourcesKey.RecordSavedSuccessfully]);


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in EditFundCommand");
                return ServerError<string>(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);
            }
        }
        private async Task SendNotificationToAddedMembers(int fundId,string fundName, List<int> newMembers,Roles role)
        {
            var notifications = new List<Domain.Entities.Notifications.Notification>();

            foreach (var item in newMembers)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    Title = string.Empty, // Will be localized at send time
                    Body = $"{fundName}|{_currentUserService.UserName}|{role}",
                    FundId = fundId,
                    UserId = item,
                    NotificationType = (int)NotificationType.AddedToFund,
                });
            }
            await _repository.Notifications.AddRangeAsync(notifications);
        }
        private async Task SendNotificationToRemovedMembers(int fundId,string fundName, List<int> deletedMembers,Roles role)
        {
            var notifications = new List<Domain.Entities.Notifications.Notification>();

            foreach (var item in deletedMembers)
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    Title = string.Empty, // Will be localized at send time
                    Body = $"{role}|{fundName}|{_currentUserService.UserName}",
                    FundId = fundId,
                    UserId = item,
                    NotificationType = (int)NotificationType.RemoveFromFund,
                });
            }
            await _repository.Notifications.AddRangeAsync(notifications);
        }
        private async Task SendNotification(Fund fund)
        {
            var notifications = new List<Domain.Entities.Notifications.Notification>();

            var fundManagers = fund.FundManagers?.Select(m => m.UserId) ?? Enumerable.Empty<int>();
            var boardSecretary = fund.FundBoardSecretaries?.Select(m => m.UserId) ?? Enumerable.Empty<int>();
            foreach (var item in fundManagers.Concat(boardSecretary))
            {
                notifications.Add(new Domain.Entities.Notifications.Notification
                {
                    Title = string.Empty, // Will be localized at send time
                    Body = $"{fund.Name}|{_currentUserService.UserName}", // Store fund name as parameter for localization
                    FundId = fund.Id,
                    UserId = item,
                    NotificationType = (int)NotificationType.CompeleteFund,
                });
            }
            await _repository.Notifications.AddRangeAsync(notifications);

        }

        #endregion

    }
}
