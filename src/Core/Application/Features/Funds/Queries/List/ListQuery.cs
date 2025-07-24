using Abstraction.Base.Dto;
using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Features.Funds.Dtos;

namespace Application.Features.Funds.Queries.List
{
    public record ListQuery : BaseListDto, IQuery<PaginatedResult<FundGroupDto>>
    {
        public DateTime? CreationDateFrom { get; set; }
        public DateTime? CreationDateTo { get; set; }
        public int? StrategyId { get; set; }
    }
}
