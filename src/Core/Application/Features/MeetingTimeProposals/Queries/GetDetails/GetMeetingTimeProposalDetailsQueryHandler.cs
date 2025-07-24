using Abstraction.Contracts.Logger;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using AutoMapper;
using Application.Features.MeetingTimeProposals.Dtos;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Abstraction.Contract.Service.Storage;

namespace Application.Features.MeetingTimeProposals.Queries.GetDetails
{
    /// <summary>
    /// Handler for GetMeetingTimeProposalDetailsQuery
    /// Retrieves detailed information about a meeting time proposal
    /// Includes voting status and attachment information
    /// </summary>
    public class GetMeetingTimeProposalDetailsQueryHandler : BaseResponseHandler, IQueryHandler<GetMeetingTimeProposalDetailsQuery, BaseResponse<MeetingTimeProposalResponseDto>>
    {
        #region Fields
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;
        private readonly IPreviewUrlHelper _previewUrlHelper;
        #endregion

        #region Constructor
        public GetMeetingTimeProposalDetailsQueryHandler(
            ILoggerManager logger,
            IRepositoryManager repository,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService,
            IPreviewUrlHelper previewUrlHelper)
        {
            _logger = logger;
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
            _previewUrlHelper = previewUrlHelper;
        }
        #endregion

        #region Handle Method
        public async Task<BaseResponse<MeetingTimeProposalResponseDto>> Handle(GetMeetingTimeProposalDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Getting meeting time proposal details for ID {request.ProposalId}");

                // Get proposal with all related data
                var proposal = await _repository.MeetingTimeProposals.GetProposalWithDetailsAsync(request.ProposalId);
                if (proposal == null)
                {
                    return NotFound<MeetingTimeProposalResponseDto>(_localizer[SharedResourcesKey.NotFound]);
                }

                // Get vote counts for each proposed date
                var voteCounts = await _repository.MeetingTimeVotes.GetVoteCountsByProposedDateAsync(request.ProposalId);

                // Check if current user has voted
                var hasCurrentUserVoted = await _repository.MeetingTimeProposals.HasUserVotedAsync(request.ProposalId, _currentUserService.UserId);

                // Get total board members count
                var totalBoardMembers = await _repository.MeetingTimeProposals.GetBoardMemberCountAsync(proposal.FundId);

                // Map to response DTO
                var response = new MeetingTimeProposalResponseDto
                {
                    Id = proposal.Id,
                    FundId = proposal.FundId,
                    FundName = proposal.Fund?.Name ?? string.Empty,
                    Subject = proposal.Subject,
                    Description = proposal.Description,
                    Status = proposal.Status,
                    CreatedByUserName = proposal.CreatedByUser?.FullName ?? string.Empty,
                    CreationDate = proposal.CreationTime,
                    TotalVotes = proposal.Votes.Select(v => v.UserId).Distinct().Count(),
                    TotalBoardMembers = totalBoardMembers,
                    HasCurrentUserVoted = hasCurrentUserVoted
                };

                // Map proposed dates with vote counts
                response.ProposedDates = proposal.ProposedDates.Select(pd => new ProposedDateResponseDto
                {
                    Id = pd.Id,
                    ProposedDateTime = pd.ProposedDateTime,
                    VoteCount = voteCounts.GetValueOrDefault(pd.Id, 0)
                }).ToList();

                // Map attachment if exists
                if (proposal.Attachment != null)
                {
                    var fileUrl = await _previewUrlHelper.GeneratePreviewUrlAsync(
                        proposal.Attachment.FilePath, 
                        proposal.Attachment.ModuleId, 
                        cancellationToken);

                    response.Attachment = new AttachmentResponseDto
                    {
                        Id = proposal.Attachment.Id,
                        FileName = proposal.Attachment.FileName,
                        FileUrl = fileUrl
                    };
                }

                _logger.LogInfo($"Successfully retrieved meeting time proposal details for ID {request.ProposalId}");
                return Success(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting meeting time proposal details for ID {request.ProposalId}");
                return ServerError<MeetingTimeProposalResponseDto>(_localizer[SharedResourcesKey.InternalServerError]);
            }
        }
        #endregion
    }
}
