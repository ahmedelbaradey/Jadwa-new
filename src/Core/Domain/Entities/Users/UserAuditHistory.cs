using Domain.Entities.Base;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Users
{
    /// <summary>
    /// User audit history entity following the established audit pattern
    /// Tracks all user management operations with comprehensive audit trail
    /// Follows the 8-field audit logging pattern established in ResolutionStatusHistory
    /// </summary>
    public class UserAuditHistory : CreationAuditedEntity
    {
        /// <summary>
        /// Foreign key to the User being audited
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Navigation property to the User
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        /// <summary>
        /// Detailed description of the action performed
        /// Field 1 of 8 required audit fields
        /// </summary>
        public string ActionDetails { get; set; } = string.Empty;

        /// <summary>
        /// Localization keys for retrieval-time translation
        /// Field 2 of 8 required audit fields
        /// Stores localization keys that will be translated when displaying audit history
        /// </summary>
        public string Notes { get; set; } = string.Empty;

        /// <summary>
        /// New status after the action (if applicable)
        /// Field 3 of 8 required audit fields
        /// For user management: Active/Inactive status or role status
        /// </summary>
        public string? NewStatus { get; set; }

        /// <summary>
        /// Previous status before the action (if applicable)
        /// Field 4 of 8 required audit fields
        /// For user management: Previous Active/Inactive status or role status
        /// </summary>
        public string? PreviousStatus { get; set; }

        /// <summary>
        /// Timestamp of the action
        /// Field 5 of 8 required audit fields
        /// Inherited from CreationAuditedEntity.CreatedAt
        /// </summary>
        // CreatedAt is inherited from CreationAuditedEntity

        /// <summary>
        /// Role of the user performing the action
        /// Field 6 of 8 required audit fields
        /// </summary>
        public string UserRole { get; set; } = string.Empty;

        /// <summary>
        /// ID of the user performing the action
        /// Field 7 of 8 required audit fields
        /// Inherited from CreationAuditedEntity.CreatedBy
        /// </summary>
        // CreatedBy is inherited from CreationAuditedEntity

        /// <summary>
        /// Type of action performed
        /// Field 8 of 8 required audit fields
        /// </summary>
        public string Action { get; set; } = string.Empty;

        /// <summary>
        /// Additional context data (JSON format for complex data)
        /// Optional field for storing additional audit context
        /// </summary>
        public string? AdditionalData { get; set; }
    }
}
