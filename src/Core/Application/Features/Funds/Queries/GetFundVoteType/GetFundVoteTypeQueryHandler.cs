using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using Application.Features.Funds.Dtos;
using AutoMapper;
using Domain.Entities.FundManagement;
using Abstraction.Contracts.Logger;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Funds.Queries.GetFundVoteType
{
    /// <summary>
    /// Handler for GetFundVoteTypeQuery
    /// Retrieves the voting type configuration for a specific fund
    /// </summary>
    public class GetFundVoteTypeQueryHandler : BaseResponseHandler, IQueryHandler<GetFundVoteTypeQuery, BaseResponse<FundVoteTypeDto>>
    {
        private readonly ILoggerManager _logger;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public GetFundVoteTypeQueryHandler(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger, IStringLocalizer<SharedResources> localizer)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _logger = logger;
            _localizer = localizer;
        }

        /// <summary>
        /// Handles the GetFundVoteTypeQuery
        /// </summary>
        /// <param name="request">The query request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Fund vote type information</returns>
        public async Task<BaseResponse<FundVoteTypeDto>> Handle(GetFundVoteTypeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get fund by ID
                var fund = await _repositoryManager.Funds.GetByIdAsync<Domain.Entities.FundManagement.Fund>(request.FundId, trackChanges: false);

                if (fund == null)
                {
                    return BaseResponse<FundVoteTypeDto>.Failure($"Fund with ID {request.FundId} not found");
                }

                // Create DTO with fund vote type information
                var fundVoteTypeDto = new FundVoteTypeDto
                {
                    FundId = fund.Id,
                    FundName = fund.Name,
                    VotingType = (VotingType)fund.VotingTypeId,
                    VotingTypeId = fund.VotingTypeId,
                    VotingTypeNameAr = GetVotingTypeNameAr((VotingType)fund.VotingTypeId),
                    VotingTypeNameEn = GetVotingTypeNameEn((VotingType)fund.VotingTypeId)
                };
                return Success(fundVoteTypeDto);          
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: in GetFundVoteTypeQueryHandler");
                return ServerError<FundVoteTypeDto>(ex.Message);
            }
        }

        /// <summary>
        /// Gets Arabic name for voting type
        /// </summary>
        private static string GetVotingTypeNameAr(VotingType votingType)
        {
            return votingType switch
            {
                VotingType.AllMembers => "جميع الأعضاء",
                VotingType.Majority => "الأغلبية",
                _ => "غير محدد"
            };
        }

        /// <summary>
        /// Gets English name for voting type
        /// </summary>
        private static string GetVotingTypeNameEn(VotingType votingType)
        {
            return votingType switch
            {
                VotingType.AllMembers => "All Members",
                VotingType.Majority => "Majority",
                _ => "Unknown"
            };
        }
    }
}
