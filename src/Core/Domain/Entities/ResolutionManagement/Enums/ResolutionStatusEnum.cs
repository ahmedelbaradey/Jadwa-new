using System.ComponentModel;

namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Enumeration representing the various statuses of a resolution
    /// Based on requirements in Sprint.md for resolution management workflow
    /// </summary>
    public enum ResolutionStatusEnum
    {
        /// <summary>
        /// Draft status - resolution is being created/edited
        /// Arabic: مسودة
        /// </summary>
        [Description("Draft")]
        Draft = 1,

        /// <summary>
        /// Pending status - resolution sent for review
        /// Arabic: معلق
        /// </summary>
        [Description("Pending")]
        Pending = 2,

        /// <summary>
        /// Completing data status - resolution data is being completed
        /// Arabic: استكمال البيانات
        /// </summary>
        [Description("Completing Data")]
        CompletingData = 3,

        /// <summary>
        /// Waiting for confirmation status
        /// Arabic: في انتظار التأكيد
        /// </summary>
        [Description("Waiting for Confirmation")]
        WaitingForConfirmation = 4,

        /// <summary>
        /// Confirmed status
        /// Arabic: مؤكد
        /// </summary>
        [Description("Confirmed")]
        Confirmed = 5,

        /// <summary>
        /// Rejected status
        /// Arabic: مرفوض
        /// </summary>
        [Description("Rejected")]
        Rejected = 6,

        /// <summary>
        /// Voting in progress status
        /// Arabic:التصويت قيد التقدم
        /// </summary>
        [Description("Voting in Progress")]
        VotingInProgress = 7,

        /// <summary>
        /// Approved status - resolution has been approved
        /// Arabic: معتمد
        /// </summary>
        [Description("Approved")]
        Approved = 8,

        /// <summary>
        /// Not approved status - resolution has been rejected after voting
        /// Arabic: غير معتمد
        /// </summary>
        [Description("Not Approved")]
        NotApproved = 9,

        /// <summary>
        /// Cancelled status - resolution has been cancelled
        /// Arabic: ملغي
        /// </summary>
        [Description("Cancelled")]
        Cancelled = 10,

    }
}
