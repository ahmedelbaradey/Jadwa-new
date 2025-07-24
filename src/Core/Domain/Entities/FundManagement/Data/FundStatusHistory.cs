using Domain.Entities.Base;
using Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.FundManagement;

/// <summary>
/// Represents a fund history entry for tracking detailed actions performed on funds
/// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
/// Based on requirements in Stories.md and Sprint.md for fund history tracking
/// Follows the same pattern as ResolutionStatusHistory for consistency
/// </summary>
public class FundStatusHistory : FullAuditedEntity
{
    /// <summary>
    /// Fund identifier that this history entry belongs to
    /// Foreign key reference to Fund entity
    /// </summary>
    public int FundId { get; set; }

    /// <summary>
    /// Status identifier from StatusHistory table
    /// Foreign key reference to StatusHistory entity
    /// </summary>
    public int StatusHistoryId { get; set; }

    [ForeignKey("FundId")]
    public Fund Fund { get; set; }

    [ForeignKey("StatusHistoryId")]
    public StatusHistory StatusHistory { get; set; }
}
