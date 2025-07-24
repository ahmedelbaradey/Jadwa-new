using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Funds.Dtos;

namespace Application.Features.Funds.Queries.GetFundBasicInfo
{
    /// <summary>
    /// Query to get fund basic information by fund ID
    /// Returns the initiation date and voting type information for a specific fund
    /// </summary>
    public record GetFundBasicInfoQuery : IQuery<BaseResponse<FundBasicInfoDto>>
    {
        /// <summary>
        /// Fund identifier to get basic information for
        /// </summary>
        public int FundId { get; set; }

       
    }
}
