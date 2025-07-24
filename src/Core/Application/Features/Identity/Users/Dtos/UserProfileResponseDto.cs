using Abstraction.Base.Dto;

namespace Application.Features.Identity.Users.Dtos
{
    /// <summary>
    /// Response DTO for user profile information
    /// Contains all user profile data for display and editing
    /// </summary>
    public record UserProfileResponseDto : BaseUserDto  
    {
 
        /// <summary>
        /// CV file path/URL
        /// </summary>
        public string? CVFilePath { get; set; }

        /// <summary>
        /// Personal photo file path/URL
        /// </summary>
        public string? PersonalPhotoPath { get; set; }

        public string? CVFileName { get; set; }
        public string? CountryCode { get; set; }

        /// <summary>
        /// MinIO preview URL for personal photo
        /// </summary>
        public string? PreviewUrl { get; set; }
 
        /// <summary>
        /// User status (read-only)
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// User roles (read-only)
        /// </summary>
        public List<string> Roles { get; set; } = new();

        /// <summary>
        /// Registration completion status (read-only)
        /// </summary>
        public bool RegistrationIsCompleted { get; set; }

        /// <summary>
        /// True if there is an active user assigned to the Legal Counsel role
        /// </summary>
        public bool LegalCouncilHasActiveUser { get; set; }

        /// <summary>
        /// True if there is an active user assigned to the Finance Controller role
        /// </summary>
        public bool FinanceControllerHasActiveUser { get; set; }

        /// <summary>
        /// True if there is an active user assigned to the Compliance Legal Managing Director role
        /// </summary>
        public bool ComplianceLegalManagingDirectorHasActiveUser { get; set; }

        /// <summary>
        /// True if there is an active user assigned to the Head of Real Estate role
        /// </summary>
        public bool HeadOfRealEstateHasActiveUser { get; set; }

    }
}
