using Abstraction.Base.Dto;
using Domain.Entities.FundManagement;

namespace Application.Features.BoardMembers.Dtos
{
    /// <summary>
    /// Base Data Transfer Object for BoardMember entity
    /// Contains core board member properties following Clean Architecture principles
    /// Based on requirements in Sprint.md for board member management (JDWA-596, JDWA-595)
    /// Follows Clean DTOs Implementation Reference Template patterns
    /// </summary>
    public record BoardMemberDto : BaseDto
    {
        /// <summary>
        /// Fund identifier that this board member belongs to
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// User identifier for the board member
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Type of board member (Independent or Not Independent)
        /// </summary>
        public BoardMemberType MemberType { get; set; }

        /// <summary>
        /// Indicates if this board member is the chairman
        /// </summary>
        public bool IsChairman { get; set; }

        /// <summary>
        /// Status of the board member (Active/Inactive)
        /// </summary>
        public bool? IsActive { get; set; } = true;

        /// <summary>
        /// Member full name from User entity
        /// Display property for ListQuery operations
        /// </summary>
        public string MemberName { get; set; } = string.Empty;

        /// <summary>
        /// Member last update date
        /// Shows when the board member record was last modified
        /// </summary>
        public DateTime? LastUpdateDate { get; set; }

        /// <summary>
        /// Display text for board member type
        /// Computed property for UI display
        /// </summary>
        public string MemberTypeDisplay { get; set; } = string.Empty;

        /// <summary>
        /// Display text for member status
        /// Computed property for UI display (Active/Inactive)
        /// </summary>
        public string StatusDisplay { get; set; } = string.Empty;

        /// <summary>
        /// Display text for member role
        /// Shows "Chairman" or "Member" based on IsChairman flag
        /// </summary>
        public string RoleDisplay { get; set; } = string.Empty;
    }
}
