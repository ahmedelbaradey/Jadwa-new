using Abstraction.Base.Dto;
using Domain.Entities.ResolutionManagement;

namespace Application.Features.Resolutions.Dtos
{
    public record class ListResolutionDto : BaseDto
    {
        /// <summary>
        /// Auto-generated resolution code (fund code/resolution year/resolution no.)
        /// </summary>
        public string Code { get; set; } = string.Empty;
        /// <summary>
        /// Date of the resolution
        /// </summary>
        public DateTime ResolutionDate { get; set; }
        /// <summary>
        /// Description of the resolution (optional, max 500 characters)
        /// </summary>
        public string? Description { get; set; }
        /// <summary>
        /// Resolution type identifier
        /// </summary>
        public int ResolutionTypeId { get; set; }

        /// <summary>
        /// Localized resolution type name for display
        /// </summary>
        public string ResolutionTypeName { get; set; } = string.Empty;

        /// <summary>
        /// Current status of the resolution
        /// </summary>
        public ResolutionStatusEnum Status { get; set; }

        /// <summary>
        /// Localized display text for resolution status
        /// </summary>
        public string StatusDisplay { get; set; } = string.Empty;

        public string ResolutionTypeNameEn { get; set; } = string.Empty;
        public string ResolutionTypeNameAr { get; set; } = string.Empty;
    }
}
