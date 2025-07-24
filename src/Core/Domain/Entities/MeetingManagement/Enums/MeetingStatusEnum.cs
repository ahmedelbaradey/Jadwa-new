using System.ComponentModel;

namespace Domain.Entities.MeetingManagement
{
    /// <summary>
    /// Enumeration representing the various statuses of a meeting
    /// Based on requirements in Meetings.md for meeting management workflow
    /// Follows the same pattern as ResolutionStatusEnum
    /// </summary>
    public enum MeetingStatusEnum
    {
        /// <summary>
        /// Scheduled status - meeting is scheduled and waiting to start
        /// Arabic: مجدول
        /// </summary>
        [Description("Scheduled")]
        Scheduled = 1,

        /// <summary>
        /// In Progress status - meeting is currently active
        /// Arabic: جاري
        /// </summary>
        [Description("In Progress")]
        InProgress = 2,

        /// <summary>
        /// Finished status - meeting has been completed
        /// Arabic: منتهي
        /// </summary>
        [Description("Finished")]
        Finished = 3,

        /// <summary>
        /// Cancelled status - meeting has been cancelled
        /// Arabic: ملغي
        /// </summary>
        [Description("Cancelled")]
        Cancelled = 4,

        /// <summary>
        /// Postponed status - meeting has been postponed to another time
        /// Arabic: مؤجل
        /// </summary>
        [Description("Postponed")]
        Postponed = 5
    }
}
