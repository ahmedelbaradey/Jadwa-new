using Domain.Entities.Base;
using Domain.Entities.FundManagement;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Represents a junction entity for tracking conflicts of interest between resolution items and board members
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Based on requirements in Sprint.md for conflict of interest management in resolutions
    /// </summary>
    public class ResolutionItemConflict : FullAuditedEntity
    {
        /// <summary>
        /// Resolution item identifier
        /// Foreign key reference to ResolutionItem entity
        /// </summary>
        public int ResolutionItemId { get; set; }

        /// <summary>
        /// Board member identifier who has conflict of interest
        /// Foreign key reference to BoardMember entity
        /// </summary>
        public int BoardMemberId { get; set; }

        /// <summary>
        /// Optional notes about the nature of the conflict
        /// Can be used to document the specific conflict details
        /// </summary>
        public string? ConflictNotes { get; set; }

        /// <summary>
        /// Navigation property to ResolutionItem entity
        /// Provides access to the resolution item that has the conflict
        /// </summary>
        [ForeignKey("ResolutionItemId")]
        public  ResolutionItem ResolutionItem { get; set; } = null!;

        /// <summary>
        /// Navigation property to BoardMember entity
        /// Provides access to the board member who has the conflict of interest
        /// </summary>
        [ForeignKey("BoardMemberId")]
        public  BoardMember BoardMember { get; set; } = null!;
    }
}
