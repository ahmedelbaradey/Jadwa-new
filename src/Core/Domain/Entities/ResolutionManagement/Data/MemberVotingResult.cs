using System.ComponentModel;

namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Enumeration representing how member voting results are calculated
    /// Based on requirements in Sprint.md for resolution voting methodology
    /// </summary>
    public enum MemberVotingResult
    {
        /// <summary>
        /// All items must be approved
        /// Arabic: جميع البنود
        /// </summary>
        [Description("All Items")]
        AllItems = 1,

        /// <summary>
        /// Majority of items must be approved (default selection)
        /// Arabic: أغلبية البنود
        /// </summary>
        [Description("Majority of Items")]
        MajorityOfItems = 2
    }
}
