using Abstraction.Base.Dto;
using Abstraction.Common.Wappers;
using Application.Base.Abstracts;
using Application.Features.Resolutions.Dtos;
using Domain.Entities.ResolutionManagement;

namespace Application.Features.Resolutions.Queries.List
{
    /// <summary>
    /// Query for listing resolutions with filtering and pagination
    /// Based on Sprint.md requirements (JDWA-582)
    /// Supports role-based filtering and comprehensive search criteria
    /// </summary>
    public record ListQuery : BaseListDto, IQuery<PaginatedResult<SingleResolutionResponse>>
    {
        /// <summary>
        /// Filter by fund ID (optional)
        /// </summary>
        public int? FundId { get; set; }

        /// <summary>
        /// Filter by resolution status (optional)
        /// </summary>
        public ResolutionStatusEnum? Status { get; set; }

        /// <summary>
        /// Filter by resolution type ID (optional)
        /// </summary>
        public int? ResolutionTypeId { get; set; }

        /// <summary>
        /// Filter by date range - start date (optional)
        /// </summary>
        public DateTime? FromDate { get; set; }

        /// <summary>
        /// Filter by date range - end date (optional)
        /// </summary>
        public DateTime? ToDate { get; set; }

        /// <summary>
        /// Filter by created by user ID (optional)
        /// </summary>
        public int? CreatedBy { get; set; }
    }
}
