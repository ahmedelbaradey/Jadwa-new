using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Users
{
    public class User : IdentityUser<int>
    {
        // Existing Properties
        public string FullName { get; set; }
        public string PreferredLanguage { get; set; } = "ar-EG"; // Default to Arabic

        /// <summary>
        /// Country code for mobile number (default Saudi Arabia)
        /// </summary>
        public string CountryCode { get; set; } = "+20"; // Default to Saudi Arabia

        /// <summary>
        /// International Bank Account Number
        /// </summary>
        public string? IBAN { get; set; }

        /// <summary>
        /// User's nationality
        /// </summary>
        public string? Nationality { get; set; }

        /// <summary>
        /// File path for user's CV document
        /// </summary>
        public string? CVFilePath { get; set; }

        /// <summary>
        /// User's passport number
        /// </summary>
        public string? PassportNo { get; set; }

        /// <summary>
        /// File path for user's personal photo
        /// </summary>
        public string? PersonalPhotoPath { get; set; }

        /// <summary>
        /// Flag indicating if registration message has been sent
        /// </summary>
        public bool RegistrationMessageIsSent { get; set; } = false;

        /// <summary>
        /// Flag indicating if user has completed registration (password reset)
        /// </summary>
        public bool RegistrationIsCompleted { get; set; } = false;

        /// <summary>
        /// Flag indicating if user has completed registration (password reset)
        /// </summary>
        public bool IsActive { get; set; } = true;
        /// <summary>
        /// Timestamp of last failed login attempt
        /// </summary>
        public DateTime? LastFailedLoginAttempt { get; set; }

        // IFullAuditedEntity Implementation
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; } = DateTime.Now;
        public int? UpdatedBy { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool? IsDeleted { get; set; } = false;
        public int? DeletedBy { get; set; }

        // Navigation Properties for Audit
        [ForeignKey("CreatedBy")]
        public User? CreatedByUser { get; set; }

        [ForeignKey("UpdatedBy")]
        public User? UpdatedByUser { get; set; }

        [ForeignKey("DeletedBy")]
        public User? DeletedByUser { get; set; }

        // Navigation Properties for Role Management
        /// <summary>
        /// Navigation property for the many-to-many relationship with Roles through IdentityUserRole
        /// Used for eager loading user roles to optimize performance and eliminate N+1 queries
        /// </summary>
        public virtual ICollection<Microsoft.AspNetCore.Identity.IdentityUserRole<int>> UserRoles { get; set; } = new List<Microsoft.AspNetCore.Identity.IdentityUserRole<int>>();

        /// <summary>
        /// Navigation property for direct access to user's roles
        /// Configured through Entity Framework to use the IdentityUserRole junction table
        /// Enables efficient querying of user roles with Include() operations
        /// </summary>
        public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    }
}
