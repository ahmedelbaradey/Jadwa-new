using Domain.Entities.Users;

namespace Domain.Entities.Base
{
    /// <summary>
    /// Interface for entities that support full audit tracking
    /// Includes creation, modification, and deletion audit information
    /// </summary>
    public interface IFullAuditedEntity
    {
        /// <summary>
        /// Creation timestamp
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// ID of user who created the entity
        /// </summary>
        int? CreatedBy { get; set; }

        /// <summary>
        /// Navigation property to user who created the entity
        /// </summary>
        User? CreatedByUser { get; set; }

        /// <summary>
        /// Last update timestamp
        /// </summary>
        DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// ID of user who last updated the entity
        /// </summary>
        int? UpdatedBy { get; set; }

        /// <summary>
        /// Navigation property to user who last updated the entity
        /// </summary>
        User? UpdatedByUser { get; set; }

        /// <summary>
        /// Deletion timestamp (for soft delete)
        /// </summary>
        DateTime? DeletedAt { get; set; }

        /// <summary>
        /// Soft delete flag
        /// </summary>
        bool? IsDeleted { get; set; }

        /// <summary>
        /// ID of user who deleted the entity
        /// </summary>
        int? DeletedBy { get; set; }

        /// <summary>
        /// Navigation property to user who deleted the entity
        /// </summary>
        User? DeletedByUser { get; set; }
    }
}
