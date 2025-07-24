using System.ComponentModel;

namespace Domain.Entities.FundManagement
{
    /// <summary>
    /// Enumeration representing fund action types for audit trail tracking
    /// Based on requirements in Stories.md and Sprint.md for fund lifecycle management
    /// Used in FundStatusHistory for comprehensive audit logging
    /// </summary>
    public enum FundActionEnum
    {
        /// <summary>
        /// Fund creation action
        /// Triggered when a new fund is created by fund manager or legal council
        /// Maps to user stories JDWA-187, JDWA-186
        /// </summary>
        [Description("Fund Creation")]
        FundCreation = 1,

        /// <summary>
        /// Fund data completion action
        /// Triggered when legal council completes fund data from under construction
        /// Maps to user story JDWA-188
        /// </summary>
        [Description("Fund Data Completion")]
        FundDataCompletion = 2,

        /// <summary>
        /// Fund data edit action
        /// Triggered when legal council edits fund data
        /// Maps to user story JDWA-185
        /// </summary>
        [Description("Fund Data Edit")]
        FundDataEdit = 3,

        /// <summary>
        /// Fund activation action
        /// Triggered when fund is activated due to having 2+ independent board members
        /// Maps to Sprint.md MSG008 notification requirements
        /// </summary>
        [Description("Fund Activation")]
        FundActivation = 4,

        /// <summary>
        /// Exit date edit action
        /// Triggered when legal council edits fund exit date
        /// Maps to user story JDWA-276
        /// </summary>
        [Description("Exit Date Edit")]
        ExitDateEdit = 5,

        /// <summary>
        /// Board member addition action
        /// Triggered when board member is added to fund
        /// Maps to Sprint.md board member management requirements
        /// </summary>
        [Description("Board Member Addition")]
        BoardMemberAddition = 6,

        /// <summary>
        /// Status change action
        /// Generic action for status transitions not covered by specific actions
        /// </summary>
        [Description("Status Change")]
        StatusChange = 7
    }
}
