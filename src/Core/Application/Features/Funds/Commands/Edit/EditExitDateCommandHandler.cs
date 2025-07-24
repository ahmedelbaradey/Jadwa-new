using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Constants;
using Abstraction.Contracts.Repository;
using Application.Services;
using Domain.Entities.FundManagement;
using Domain.Entities.Notifications;
using Abstraction.Contract.Service;
using Resources;
using Microsoft.Extensions.Localization;
using Domain.Entities.FundManagement.State;

namespace Application.Features.Funds.Commands.Edit
{
    public class EditExitDateCommandHandler : BaseResponseHandler, ICommandHandler<EditExitDateCommand, BaseResponse<string>>
    {

        #region Fileds
        private readonly ILoggerManager _logger;
        protected IRepositoryManager _repository;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
   
        #endregion

        #region Constructors
        public EditExitDateCommandHandler(ILoggerManager logger,
                                              IRepositoryManager repository,
                                              IStringLocalizer<SharedResources> localizer,
                                              ICurrentUserService currentUserService)
        {
            _logger = logger;
            _repository = repository;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }
        #endregion

        #region Handle Functions
        public async Task<BaseResponse<string>> Handle(EditExitDateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.EmptyRequestValidation]);

                // RBAC validation - Only Legal Council can edit exit date
                if (!_currentUserService.Roles.Contains(Roles.LegalCouncil.ToString().ToLower()))
                {
                    return BadRequest<string>(_localizer[SharedResourcesKey.UnauthorizedAccess]);
                }

                var fund = await _repository.Funds.EditFundById(request.Id, true);
                if (fund == null)
                    return BadRequest<string>(_localizer[SharedResourcesKey.AnErrorIsOccurredWhileSavingData]);

                // Initialize FundStateContext for enhanced audit logging and notifications
                var fundStateContext = new FundStateContext(
                    fund,
                    _localizer,
                    _repository,
                    _currentUserService
                );

                // Store original exit date for audit trail
                var originalExitDate = fund.ExitDate;
                fund.ExitDate = request.ExitDate;

                // Add comprehensive audit entry for exit date edit
                var originalDateStr = originalExitDate?.ToString("yyyy-MM-dd") ?? "null";
                var newDateStr = request.ExitDate.ToString("yyyy-MM-dd");
                var exitDateChangeDetails = $"Exit date changed from {originalDateStr} to {newDateStr}";
                //fundStateContext.AddAuditEntry(
                //    FundActionEnum.ExitDateEdit,
                //    exitDateChangeDetails,
                //    SharedResourcesKey.FundExitDateEditAction,null
                //);
                var exitState = new ExitedFund();
                var transitionSuccess = fundStateContext.ChangeStatusWithAudit(
                    exitState,
                    FundActionEnum.ExitDateEdit,
                    _localizer[SharedResourcesKey.FundExitDateEditAction],
                    SharedResourcesKey.FundExitDateEditAction,
                    sendNotifications: false
                );
                await _repository.Funds.UpdateAsync(fund);

                // Send enhanced notifications using FundStateContext pattern
                await SendExitDateChangeNotifications(fund);

                return Success<string>(_localizer[SharedResourcesKey.RecordSavedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in EditExitDateCommand");
                return ServerError<string>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
            }
        }
        /// <summary>
        /// Sends enhanced exit date change notifications following the established patterns
        /// Uses MSG004 notification type as specified in user story JDWA-276
        /// Follows the same notification pattern as FundStateContext for consistency
        /// </summary>
        /// <param name="fund">Fund entity with updated exit date</param>
        private async Task SendExitDateChangeNotifications(Fund fund)
        {
            try
            {
                var notifications = new List<Domain.Entities.Notifications.Notification>();

                // Get stakeholder user IDs following the established pattern
                var stakeholderUserIds = new List<int>();

                // Add Fund Managers
                if (fund.FundManagers != null)
                {
                    stakeholderUserIds.AddRange(fund.FundManagers.Where(fm => fm.IsDeleted != true).Select(fm => fm.UserId));
                }

                // Add Board Secretaries
                if (fund.FundBoardSecretaries != null)
                {
                    stakeholderUserIds.AddRange(fund.FundBoardSecretaries.Where(bs => bs.IsDeleted != true).Select(bs => bs.UserId));
                }

                // Add Legal Council
                stakeholderUserIds.Add(fund.LegalCouncilId);

                // Remove duplicates
                stakeholderUserIds = stakeholderUserIds.Distinct().ToList();

                // Create notifications for all stakeholders
                foreach (var userId in stakeholderUserIds)
                {
                    notifications.Add(new Domain.Entities.Notifications.Notification
                    {
                        Title = string.Empty, // Will be localized at send time
                        Body = $"{fund.Name}|{fund.ExitDate}|{_currentUserService.UserName}", // Store parameters for localization
                        FundId = fund.Id,
                        UserId = userId,
                        NotificationType = (int)NotificationType.ChangeExitDate // MSG004
                    });
                }

                if (notifications.Count > 0)
                {
                    await _repository.Notifications.AddRangeAsync(notifications);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending exit date change notifications for fund {fund.Id}");
                // Don't throw to avoid breaking the main operation
            }
        }

        #endregion

    }
}
