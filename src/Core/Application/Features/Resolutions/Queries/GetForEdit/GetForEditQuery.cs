using Application.Base.Abstracts;
using Abstraction.Base.Response;
using Application.Features.Resolutions.Dtos;

namespace Application.Features.Resolutions.Queries.GetForEdit
{
    /// <summary>
    /// Query for retrieving resolution data optimized for edit screen
    /// Returns minimal data needed for editing a resolution
    /// </summary>
    public record GetForEditQuery : IQuery<BaseResponse<ResolutionForEditDto>>
    {
        /// <summary>
        /// Resolution identifier
        /// </summary>
        public int Id { get; set; }
    }
}
