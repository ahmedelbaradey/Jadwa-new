using Abstraction.Base.Dto;
using System.Globalization;

namespace Application.Common.Dtos
{
    /// <summary>
    /// Base Data Transfer Object for entities requiring Arabic/English localization
    /// Provides automatic culture-based property selection following Jadwa API patterns
    /// Used as base class for multilingual entities
    /// </summary>
    public record LocalizedDto : BaseDto
    {
        /// <summary>
        /// Entity name in Arabic
        /// Required for entities with multilingual support
        /// </summary>
        public string NameAr { get; set; } = string.Empty;

        /// <summary>
        /// Entity name in English
        /// Required for entities with multilingual support
        /// </summary>
        public string NameEn { get; set; } = string.Empty;

       
        /// <summary>
        /// Computed property that returns the appropriate name based on current culture
        /// Returns Arabic name if culture starts with "ar", otherwise returns English name
        /// </summary>
        public string LocalizedName =>
            CultureInfo.CurrentCulture.Name.StartsWith("ar", StringComparison.OrdinalIgnoreCase) 
                ? NameAr 
                : NameEn;

       
    }
}
