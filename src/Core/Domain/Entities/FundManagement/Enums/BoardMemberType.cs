using System.ComponentModel;

namespace Domain.Entities.FundManagement
{
    /// <summary>
    /// Enumeration representing the types of board members
    /// Based on requirements in Sprint.md for board member management
    /// </summary>
    public enum BoardMemberType
    {
        /// <summary>
        /// Independent board member
        /// Arabic: عضو مستقل
        /// Maximum of 14 independent members allowed per fund
        /// </summary>
        [Description("Independent")]
        Independent = 1,

        /// <summary>
        /// Non-independent board member
        /// Arabic: عضو غير مستقل
        /// </summary>
        [Description("Not Independent")]
        NotIndependent = 2
    }
}
