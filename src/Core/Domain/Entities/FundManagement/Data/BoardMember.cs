using Domain.Entities.Base;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.FundManagement
{
    /// <summary>
    /// Represents a board member entity for fund board management
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Based on requirements in Sprint.md for board member management (JDWA-596, JDWA-595)
    /// </summary>
    public class BoardMember : FullAuditedEntity
    {
        /// <summary>
        /// Fund identifier that this board member belongs to
        /// Foreign key reference to Fund entity
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// User identifier for the board member
        /// Foreign key reference to User entity
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Type of board member (Independent or Not Independent)
        /// Business rule: Maximum of 14 independent board members per fund
        /// When 2 independent members are added, fund status changes to "Active"
        /// </summary>
        public BoardMemberType MemberType { get; set; }

        /// <summary>
        /// Indicates if this board member is the chairman
        /// Business rule: Each board can have maximum 1 chairman
        /// </summary>
        public bool IsChairman { get; set; } = false;

        /// <summary>
        /// Status of the board member (Active/Inactive)
        /// Default is Active when member is added
        /// </summary>
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Navigation property to Fund entity
        /// Provides access to the fund this board member belongs to
        /// </summary>
        [ForeignKey("FundId")]
        public Fund Fund { get; set; } = null!;

        /// <summary>
        /// Navigation property to User entity
        /// Provides access to the user information for this board member
        /// </summary>
        [ForeignKey("UserId")]
        public User User { get; set; } = null!;

        /// <summary>
        /// Collection navigation property to ResolutionVote entities
        /// Represents all votes cast by this board member
        /// </summary>
        public virtual ICollection<ResolutionVote> Votes { get; set; } = new List<ResolutionVote>();

        /// <summary>
        /// Collection navigation property to ResolutionItemConflict entities
        /// Represents all resolution items where this board member has conflicts of interest
        /// </summary>
        public virtual ICollection<ResolutionItemConflict> ConflictItems { get; set; } = new List<ResolutionItemConflict>();
    }
}
