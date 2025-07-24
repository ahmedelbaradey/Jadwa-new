using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Resolutions.Dtos;

namespace Application.Features.Resolutions.Queries.GetResolutionTypes
{
    /// <summary>
    /// Query to get all active resolution types
    /// Returns list of resolution types for dropdown/selection purposes
    /// </summary>
    public record GetResolutionTypesQuery : IQuery<BaseResponse<IEnumerable<ResolutionTypeDto>>>
    {
        /// <summary>
        /// Optional parameter to include inactive types (default: false)
        /// </summary>
        public bool IncludeInactive { get; set; } = false;
    }
}
