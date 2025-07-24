using Microsoft.AspNetCore.Identity;

namespace Domain.Entities.Users
{
    /// <summary>
    /// Role entity extending ASP.NET Core Identity IdentityRole
    /// Enhanced with navigation properties for optimized user-role relationship queries
    /// </summary>
    public class Role : IdentityRole<int>
    {
        /// <summary>
        /// Navigation property for the many-to-many relationship with Users through IdentityUserRole
        /// Used for eager loading role users to optimize performance and eliminate N+1 queries
        /// </summary>
        public virtual ICollection<Microsoft.AspNetCore.Identity.IdentityUserRole<int>> UserRoles { get; set; } = new List<Microsoft.AspNetCore.Identity.IdentityUserRole<int>>();

        /// <summary>
        /// Navigation property for direct access to users with this role
        /// Configured through Entity Framework to use the IdentityUserRole junction table
        /// Enables efficient querying of role users with Include() operations
        /// </summary>
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
