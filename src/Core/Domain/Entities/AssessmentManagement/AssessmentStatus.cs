using System.ComponentModel;

namespace Domain.Entities.AssessmentManagement
{
    /// <summary>
    /// Enumeration representing the various statuses of an Assessment
    /// Based on user story requirements and state pattern implementation
    /// </summary>
    public enum AssessmentStatus
    {
        /// <summary>
        /// Draft status - assessment is being created/edited
        /// Arabic: مسودة
        /// </summary>
        [Description("Draft")]
        Draft = 1,

        /// <summary>
        /// Waiting for approval status - submitted for review
        /// Arabic: في انتظار الموافقة
        /// </summary>
        [Description("WaitingForApproval")]
        WaitingForApproval = 2,

        /// <summary>
        /// Approved status - ready for distribution
        /// Arabic: موافق عليه
        /// </summary>
        [Description("Approved")]
        Approved = 3,

        /// <summary>
        /// Rejected status - needs revision
        /// Arabic: مرفوض
        /// </summary>
        [Description("Rejected")]
        Rejected = 4,

        /// <summary>
        /// Active status - distributed to board members
        /// Arabic: نشط
        /// </summary>
        [Description("Active")]
        Active = 5,

        /// <summary>
        /// Completed status - all responses collected
        /// Arabic: مكتمل
        /// </summary>
        [Description("Completed")]
        Completed = 6
    }
}
