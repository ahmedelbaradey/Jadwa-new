using Application.Features.Identity.Users.Dtos;

namespace Application.Features.Identity.Users.Queries.Responses
{
    /// <summary>
    /// Enhanced response DTO for user list with Sprint 3 fields
    /// Includes additional fields for advanced filtering and display
    /// </summary>
    public record GetUserListResponse 
    {
         public int Id { get; set; }
        // Basic Information
        public string FullName { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string CountryCode { get; set; } = null!;
        /// <summary>
        /// Email address (must be unique)
        /// </summary>
        public string Email { get; set; } = null!;

        /// <summary>
        /// User status (active/inactive)
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Registration completion status
        /// </summary>
        public bool RegistrationIsCompleted { get; set; }

        /// <summary>
        /// Registration message sent status
        /// </summary>
        public bool RegistrationMessageIsSent { get; set; }
        /// <summary>
        /// User roles (for display)
        /// </summary>
        public List<string> Roles { get; set; } = new();

        /// <summary>
        /// Primary role for display (first role)
        /// </summary>
        public string? PrimaryRole { get; set; }

        /// <summary>
        /// Last update date
        /// </summary>
        public DateTime LastUpdateDate { get; set; }


    }
}
