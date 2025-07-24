using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Application.Features.Resolutions.Dtos;

namespace Application.Features.Resolutions.Queries.GetStatuses
{
    /// <summary>
    /// Query to get all available resolution statuses
    /// Used for populating dropdowns and filters in the frontend
    /// </summary>
    public record GetResolutionStatusesQuery : IQuery<BaseResponse<List<ResolutionStatusDto>>>
    {
        public int? FundId { get; set; }
    }
}
