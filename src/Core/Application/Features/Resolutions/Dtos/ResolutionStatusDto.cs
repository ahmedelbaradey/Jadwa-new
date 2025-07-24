using Application.Common.Dtos;
using Domain.Entities.ResolutionManagement;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// DTO representing a resolution status for API responses
    /// Inherits from LocalizedDto to provide automatic Arabic/English localization
    /// Used for populating dropdowns and filters in the frontend
    /// </summary>
    public record ResolutionStatusDto : LocalizedDto
    {
        /// <summary>
        /// Status enum value
        /// </summary>
        public ResolutionStatusEnum Value { get; set; }

        /// <summary>
        /// Description of the status from the enum Description attribute
        /// </summary>
        public string Description { get; set; } = string.Empty;

        // Note: Id, NameAr, NameEn, and LocalizedName are inherited from LocalizedDto
        // LocalizedName automatically returns the appropriate name based on current culture
    }
}
