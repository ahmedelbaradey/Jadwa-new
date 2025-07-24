using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using Application.Features.Funds.Dtos;
using AutoMapper;
using Domain.Entities.FundManagement;
using Abstraction.Contracts.Logger;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Funds.Queries.GetFundBasicInfo
{
    /// <summary>
    /// Handler for GetFundBasicInfoQuery
    /// Retrieves the initiation date and voting type information for a specific fund
    /// </summary>
    public class GetFundBasicInfoQueryHandler : BaseResponseHandler, IQueryHandler<GetFundBasicInfoQuery, BaseResponse<FundBasicInfoDto>>
    {
        private readonly ILoggerManager _logger;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public GetFundBasicInfoQueryHandler(IRepositoryManager repositoryManager, IMapper mapper, ILoggerManager logger, IStringLocalizer<SharedResources> localizer)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _logger = logger;
            _localizer = localizer;
        }

        /// <summary>
        /// Handles the GetFundBasicInfoQuery
        /// </summary>
        /// <param name="request">The query request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Fund basic information including initiation date and voting type</returns>
        public async Task<BaseResponse<FundBasicInfoDto>> Handle(GetFundBasicInfoQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInfo($"Getting basic information for Fund ID: {request.FundId}");

                // Get fund by ID
                var fund = await _repositoryManager.Funds.GetByIdAsync<Domain.Entities.FundManagement.Fund>(request.FundId, trackChanges: false);

                if (fund == null)
                {
                    _logger.LogWarn($"Fund with ID {request.FundId} not found");
                    return NotFound<FundBasicInfoDto>($"Fund with ID {request.FundId} not found");
                }

                // Create DTO with fund basic information (initiation date and voting type)
                var fundBasicInfoDto = new FundBasicInfoDto
                {
                    FundId = fund.Id,
                    FundName = fund.Name,
                    InitiationDate = fund.InitiationDate, //new FundInitiationDateInfo
                   // {
                    //    Date = fund.InitiationDate
                    //},
                    VotingType = new FundVotingTypeInfo
                    {
                        Type = (VotingType)fund.VotingTypeId,
                        TypeId = fund.VotingTypeId,
                        NameAr = GetVotingTypeNameAr((VotingType)fund.VotingTypeId),
                        NameEn = GetVotingTypeNameEn((VotingType)fund.VotingTypeId)
                    }
                };

                _logger.LogInfo($"Successfully retrieved basic information for Fund ID: {request.FundId}");
                return Success(fundBasicInfoDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting basic information for Fund ID: {request.FundId}");
                return ServerError<FundBasicInfoDto>(_localizer[SharedResourcesKey.SystemErrorSavingData]);
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
