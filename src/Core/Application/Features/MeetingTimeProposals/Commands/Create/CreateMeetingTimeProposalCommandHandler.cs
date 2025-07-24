using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using AutoMapper;
using Domain.Entities.MeetingManagement;
using Domain.Entities.FundManagement;
using Domain.Entities.Notifications;
using Domain.Entities.MeetingManagement.State;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using System.Net;

namespace Application.Features.MeetingTimeProposals.Commands.Create
{
    /// <summary>
    /// Handler for CreateMeetingTimeProposalCommand
    /// Implements business logic for creating meeting time proposals with notifications
    /// Based on User Story 1 requirements from Meetings.md
    /// </summary>
    public class CreateMeetingTimeProposalCommandHandler : BaseResponseHandler, ICommandHandler<CreateMeetingTimeProposalCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly MeetingStateFactory _stateFactory;
        #endregion

        #region Constructor
        public CreateMeetingTimeProposalCommandHandler(
            ILoggerManager logger,
            IRepositoryManager repository,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService,
            MeetingStateFactory stateFactory)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _stateFactory = stateFactory;
        }
        #endregion

        #region Handle Method
        public async Task<BaseResponse<string>> Handle(CreateMeetingTimeProposalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Creating meeting time proposal for fund {request.FundId} by user {_currentUserService.UserId}");

                // Validate fund exists and user has access
                var fund = await _repository.Funds.GetByIdAsync(request.FundId);
                if (fund == null)
                {
                    return BadRequest<string>(_localizer[SharedResourcesKey.NotFound]);
                }

                // Create the proposal entity
                var proposal = new MeetingTimeProposal
                {
                    FundId = request.FundId,
                    Subject = request.Subject,
                    Description = request.Description,
                    Status = "Under Voting",
                    CreatedByUserId = _currentUserService.UserId,
                    AttachmentId = request.AttachmentId
                };

                // Add proposed dates
                foreach (var proposedDateDto in request.ProposedDates)
                {
                    proposal.ProposedDates.Add(new ProposedDate
                    {
                        ProposedDateTime = proposedDateDto.ProposedDateTime
                    });
                }

                // Save to database
                await _repository.MeetingTimeProposals.AddAsync(proposal);
                await _repository.SaveAsync();

                _logger.LogInfo($"Meeting time proposal created successfully with ID {proposal.Id}");

                // Send notifications to all board members
                await SendNotificationsAsync(proposal, fund);

                return Success(_localizer[SharedResourcesKey.MeetingTimeProposalCreatedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error creating meeting time proposal for fund {request.FundId}");
                return ServerError<string>(_localizer[SharedResourcesKey.InternalServerError]);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Sends notifications to all board members about the new proposal
        /// </summary>
        private async Task SendNotificationsAsync(MeetingTimeProposal proposal, Fund fund)
        {
            try
            {
                // Get all board members for the fund
                var boardMemberUserIds = await _repository.MeetingTimeProposals.GetBoardMemberUserIdsAsync(proposal.FundId);

                var notifications = new List<Notification>();

                foreach (var userId in boardMemberUserIds)
                {
                    notifications.Add(new Notification
                    {
                        Title = _localizer[SharedResourcesKey.NewVoteStartedNotificationTitle],
                        Body = string.Format(_localizer[SharedResourcesKey.NewVoteStartedNotificationBody], proposal.Subject),
                        FundId = proposal.FundId,
                        UserId = userId,
                        NotificationType = (int)NotificationType.MeetingTimeProposalCreated,
                        SentDate = DateTime.UtcNow
                    });
                }

                if (notifications.Any())
                {
                    await _repository.Notifications.AddRangeAsync(notifications);
                    await _repository.SaveAsync();
                    _logger.LogInfo($"Sent {notifications.Count} notifications for meeting time proposal {proposal.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending notifications for meeting time proposal {proposal.Id}");
                // Don't fail the main operation if notifications fail
            }
        }

        /// <summary>
        /// Gets the current user's role in the fund context
        /// Replicates the logic from AddResolutionCommandHandler.GetUserFundRole() method
        /// </summary>
        /// <param name="fundId">Fund identifier</param>
        /// <returns>User's role in the fund context</returns>
        private async Task<Roles> GetUserFundRole(int fundId)
        {
            var userId = _currentUserService.UserId;

            // Get all role collections for the fund
            var boardMembers = await _repository.BoardMembers.GetBoardMembersByFundIdAsync(fundId);
            var fundManagers = await _repository.FundManagers.GetFundManagersByFundIdAsync(fundId);
            var legalCounsels = await _repository.LegalCounsels.GetLegalCounselsByFundIdAsync(fundId);
            var boardSecretaries = await _repository.BoardSecretaries.GetBoardSecretariesByFundIdAsync(fundId);

            // Use state context to determine user role
            var stateContext = _stateFactory.CreateContext(MeetingStatusEnum.Scheduled);
            return stateContext.GetCurrentUserRole(userId, fundId, boardMembers, fundManagers, legalCounsels, boardSecretaries);
        }
        #endregion
    }
}
