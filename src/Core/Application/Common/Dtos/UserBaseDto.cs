using Abstraction.Base.Dto;

namespace Application.Common.Dtos
{
    /// <summary>
    /// Base Data Transfer Object for user-related entities
    /// Contains common user properties shared across different user operations
    /// Used as base class for entities that have user associations
    /// </summary>
    public record UserBaseDto : BaseDto
    {
        /// <summary>
        /// User identifier
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// User name for login
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Full name of the user
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// Email address of the user
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Indicates if the user is active
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Date when the user joined
        /// </summary>
        public DateTime JoinedDate { get; set; }

        /// <summary>
        /// Date when the user was deactivated (if applicable)
        /// </summary>
        public DateTime? DeactivatedDate { get; set; }

        /// <summary>
        /// Reason for deactivation (if applicable)
        /// </summary>
        public string? DeactivationReason { get; set; }
    }
}
