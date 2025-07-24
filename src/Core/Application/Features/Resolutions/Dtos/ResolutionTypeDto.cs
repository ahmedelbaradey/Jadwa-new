using Application.Common.Dtos;

namespace Application.Features.Resolutions.Dtos
{
    /// <summary>
    /// Data Transfer Object for ResolutionType entity with localization support
    /// Inherits from LocalizedDto following Clean DTOs Implementation Reference Template
    /// Used for resolution type selection and display
    /// </summary>
    public record ResolutionTypeDto : LocalizedDto
    {
        /// <summary>
        /// Indicates if this resolution type is active and available for selection
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Indicates if this is the "Other" resolution type that allows custom input
        /// </summary>
        public bool IsOther { get; set; }

        /// <summary>
        /// Display order for sorting resolution types in UI
        /// Lower numbers appear first
        /// </summary>
        public int DisplayOrder { get; set; } = 0;
    }
}
