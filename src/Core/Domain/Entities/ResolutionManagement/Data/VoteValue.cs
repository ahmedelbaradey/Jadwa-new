using System.ComponentModel;

namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Enumeration representing the possible vote values for resolution voting
    /// Based on business requirements for resolution voting system
    /// </summary>
    public enum VoteValue
    {
        /// <summary>
        /// Vote in favor of the resolution/item
        /// Arabic: موافق
        /// </summary>
        [Description("Approve")]
        Approve = 1,

        /// <summary>
        /// Vote against the resolution/item
        /// Arabic: غير موافق
        /// </summary>
        [Description("Reject")]
        Reject = 2,

        /// <summary>
        /// Abstain from voting (neutral position)
        /// Arabic: امتناع عن التصويت
        /// </summary>
        [Description("Abstain")]
        Abstain = 3
    }
}
