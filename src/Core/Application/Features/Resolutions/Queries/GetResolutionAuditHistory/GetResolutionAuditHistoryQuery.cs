using Abstraction.Base.Dto;
using Abstraction.Base.Response;
using Abstraction.Common.Wappers;
using Application.Base.Abstracts;
using Application.Features.Resolutions.Dtos;

namespace Application.Features.Resolutions.Queries.GetResolutionAuditHistory
{
    /// <summary>
    /// Query to retrieve localized audit history for a resolution
    /// Demonstrates usage of IAuditLocalizationService for multilingual audit trail display
    /// </summary>
    public record GetResolutionAuditHistoryQuery : BaseListDto, IQuery<BaseResponse<List<ResolutionStatusHistoryDto>>>
    {
        /// <summary>
        /// Resolution ID to retrieve audit history for
        /// </summary>
        public int ResolutionId { get; set; }
    }


}