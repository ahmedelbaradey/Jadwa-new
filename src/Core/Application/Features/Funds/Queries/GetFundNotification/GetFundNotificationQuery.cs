using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Funds.Dtos;
using Abstraction.Base.Dto;
using System.Linq.Dynamic.Core;
using Abstraction.Common.Wappers;

namespace Application.Features.Funds.Queries.GetFundBasicInfo
{
    /// <summary>
    /// Query to get fund basic information by fund ID
    /// Returns the initiation date and voting type information for a specific fund
    /// </summary>
    public record GetFundNotificationQuery : BaseListDto,  IQuery<PaginatedResult<FundNotificationDto>>
    {
        /// <summary>
        /// Fund identifier to get basic information for
        /// </summary>
        public int FundId { get; set; }

       
    }
}
