using System.ComponentModel;

namespace Domain.Entities.FundManagement
{
    /// <summary>
    /// Enumeration representing fund status values that map to StatusHistory table
    /// Based on StatusHistoryConfig seed data and Sprint.md requirements
    /// Used for fund lifecycle management and state transitions
    /// </summary>
    public enum FundRoleEnum
    {
        [Description("None")]
        None = 0,
        
        [Description("Fund Manager")]
        FundManager = 1,

        [Description("Board Secretary")]
        BoardSecretary = 2,

        [Description("Legal Council")]
        LegalCouncil = 3,

        [Description("Board Member")]
        BoardMember = 4
    }
}
