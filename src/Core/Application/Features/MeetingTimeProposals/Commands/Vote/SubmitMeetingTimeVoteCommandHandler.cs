using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using AutoMapper;
using Domain.Entities.MeetingManagement;
using Domain.Entities.Notifications;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using System.Net;

namespace Application.Features.MeetingTimeProposals.Commands.Vote
{
    /// <summary>
    /// Handler for SubmitMeetingTimeVoteCommand
    /// Implements business logic for voting on meeting time proposals
    /// Based on User Story 2 requirements from Meetings.md
    /// </summary>
    public class SubmitMeetingTimeVoteCommandHandler : BaseResponseHandler, ICommandHandler<SubmitMeetingTimeVoteCommand, BaseResponse<string>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        #endregion

        #region Constructor
        public SubmitMeetingTimeVoteCommandHandler(
            ILoggerManager logger,
            IRepositoryManager repository,
            IMapper mapper,
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

        #region Handle Method
        public async Task<BaseResponse<string>> Handle(SubmitMeetingTimeVoteCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Submitting vote for proposal {request.ProposalId} by user {_currentUserService.UserId}");

                // Get the proposal with details
                var proposal = await _repository.MeetingTimeProposals.GetProposalWithDetailsAsync(request.ProposalId);
                if (proposal == null)
                {
                    return NotFound<string>(_localizer[SharedResourcesKey.NotFound]);
                }

                // Check if proposal is still under voting
                if (proposal.Status != "Under Voting")
                {
                    return BadRequest<string>(_localizer[SharedResourcesKey.VotingForProposalComplete]);
                }

                // Check if user has already voted
                var hasVoted = await _repository.MeetingTimeProposals.HasUserVotedAsync(request.ProposalId, _currentUserService.UserId);
                if (hasVoted)
                {
                    return BadRequest<string>(_localizer[SharedResourcesKey.AlreadyVotedOnProposal]);
                }

                // Create votes for each selected proposed date
                var votes = new List<MeetingTimeVote>();
                foreach (var proposedDateId in request.SelectedProposedDateIds)
                {
                    votes.Add(new MeetingTimeVote
                    {
                        ProposalId = request.ProposalId,
                        UserId = _currentUserService.UserId,
                        ProposedDateId = proposedDateId,
                        VoteTimestamp = DateTime.UtcNow
                    });
                }

                // Save votes
                await _repository.MeetingTimeVotes.AddRangeAsync(votes);
                await _repository.SaveAsync();

                _logger.LogInfo($"Vote submitted successfully for proposal {request.ProposalId}");

                // Check if all board members have voted
                await CheckVotingCompletionAsync(proposal);

                return Success(_localizer[SharedResourcesKey.VoteSubmittedSuccessfully]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error submitting vote for proposal {request.ProposalId}");
                return ServerError<string>(_localizer[SharedResourcesKey.InternalServerError]);
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Checks if all board members have voted and completes the voting if so
        /// </summary>
        private async Task CheckVotingCompletionAsync(MeetingTimeProposal proposal)
        {
            try
            {
                var totalBoardMembers = await _repository.MeetingTimeProposals.GetBoardMemberCountAsync(proposal.FundId);
                var totalVotes = await _repository.MeetingTimeProposals.GetVoteCountAsync(proposal.Id);

                if (totalVotes >= totalBoardMembers)
                {
                    // Update proposal status to completed
                    proposal.Status = "Completed";
                    _repository.MeetingTimeProposals.Update(proposal);
                    await _repository.SaveAsync();

                    // Send completion notification to proposal creator
                    await SendCompletionNotificationAsync(proposal);

                    _logger.LogInfo($"Voting completed for proposal {proposal.Id}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error checking voting completion for proposal {proposal.Id}");
                // Don't fail the main operation
            }
        }

        /// <summary>
        /// Sends notification to proposal creator when voting is complete
        /// </summary>
        private async Task SendCompletionNotificationAsync(MeetingTimeProposal proposal)
        {
            try
            {
                var notification = new Notification
                {
                    Title = _localizer[SharedResourcesKey.VotingCompletedNotificationTitle],
                    Body = string.Format(_localizer[SharedResourcesKey.VotingCompletedNotificationBody], proposal.Subject),
                    FundId = proposal.FundId,
                    UserId = proposal.CreatedByUserId,
                    NotificationType = (int)NotificationType.MeetingTimeVotingCompleted,
                    SentDate = DateTime.UtcNow
                };

                await _repository.Notifications.AddAsync(notification);
                await _repository.SaveAsync();

                _logger.LogInfo($"Sent completion notification for proposal {proposal.Id}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending completion notification for proposal {proposal.Id}");
                // Don't fail the main operation
            }
        }
        #endregion
    }
}
