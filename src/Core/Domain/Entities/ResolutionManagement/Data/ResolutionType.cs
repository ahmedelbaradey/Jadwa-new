using Domain.Entities.Base;

namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Represents a resolution type entity for categorizing different types of resolutions
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Based on requirements in Sprint.md for resolution type management
    /// </summary>
    public class ResolutionType : FullAuditedEntity
    {
        /// <summary>
        /// Arabic name of the resolution type
        /// Required field for Arabic localization
        /// Examples: استحواذ، تخارج، بيع، توزيع أرباح، تمديد مدة الصندوق، etc.
        /// </summary>
        public string NameAr { get; set; } = string.Empty;

        /// <summary>
        /// English name of the resolution type
        /// Required field for English localization
        /// Examples: Acquisition, Exit, Sale, Profit Distribution, Fund Extension, etc.
        /// </summary>
        public string NameEn { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if this resolution type is active and available for selection
        /// Default is true for new resolution types
        /// </summary>
        public bool IsActive { get; set; } = true;

        public bool IsOther { get; set; }

        /// <summary>
        /// Display order for sorting resolution types in UI
        /// Lower numbers appear first
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Collection navigation property to Resolution entities
        /// Represents all resolutions that use this resolution type
        /// </summary>
        public  ICollection<Resolution> Resolutions { get; set; } = new List<Resolution>();
    }
}
