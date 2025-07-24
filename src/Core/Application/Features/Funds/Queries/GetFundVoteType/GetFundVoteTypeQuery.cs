using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Funds.Dtos;

namespace Application.Features.Funds.Queries.GetFundVoteType
{
    /// <summary>
    /// Query to get fund vote type by fund ID
    /// Returns the voting methodology configured for a specific fund
    /// </summary>
    public record GetFundVoteTypeQuery : IQuery<BaseResponse<FundVoteTypeDto>>
    {
        /// <summary>
        /// Fund identifier to get vote type for
        /// </summary>
        public int FundId { get; set; }

        public GetFundVoteTypeQuery(int fundId)
        {
            FundId = fundId;
        }
    }
}
