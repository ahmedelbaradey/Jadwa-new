using Domain.Entities.Base;
using Domain.Entities.FundManagement;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Represents a resolution item entity for individual agenda items within a resolution
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Based on requirements in Sprint.md for resolution item management (JDWA-506, JDWA-566)
    /// </summary>
    public class ResolutionItem : FullAuditedEntity
    {
        /// <summary>
        /// Resolution identifier that this item belongs to
        /// Foreign key reference to Resolution entity
        /// </summary>
        public int ResolutionId { get; set; }

        /// <summary>
        /// Auto-generated title for the resolution item
        /// Format: Item1, Item2, Item3, etc.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Description of the resolution item
        /// Optional field, maximum 500 characters for add/edit, 200 characters for display
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Indicates if there is a conflict of interest with some board members
        /// When true, specific board members are excluded from voting on this item
        /// </summary>
        public bool HasConflict { get; set; } = false;

        /// <summary>
        /// Display order for sorting resolution items
        /// Used for re-ordering when items are deleted (Item3 becomes Item2, etc.)
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Navigation property to Resolution entity
        /// Provides access to the parent resolution
        /// </summary>
        [ForeignKey("ResolutionId")]
        public  Resolution Resolution { get; set; } = null!;

        /// <summary>
        /// Collection navigation property to ResolutionItemConflict entities
        /// Represents board members who have conflict of interest with this item
        /// Many-to-many relationship through junction table
        /// </summary>
        public  ICollection<ResolutionItemConflict> ConflictMembers { get; set; } = new List<ResolutionItemConflict>();

        /// <summary>
        /// Collection navigation property to ResolutionVote entities
        /// Represents all votes cast on this specific resolution item
        /// </summary>
        public  ICollection<ResolutionVote> Votes { get; set; } = new List<ResolutionVote>();
    }
}
