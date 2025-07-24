using System.ComponentModel;

namespace Domain.Entities.MeetingManagement
{
    /// <summary>
    /// Enumeration representing the various actions of a meeting (status changes) and their corresponding status values.
    /// Based on requirements in Meetings.md for meeting management workflow
    /// Follows the same pattern as ResolutionActionEnum
    /// </summary>
    public enum MeetingActionEnum
    {
        /// <summary>
        /// Meeting creation
        /// Arabic: إنشاء اجتماع
        /// </summary>
        [Description("Meeting Creation")]
        MeetingCreation = 1,

        /// <summary>
        /// Meeting edit/modification
        /// Arabic: تعديل اجتماع
        /// </summary>
        [Description("Meeting Edit")]
        MeetingEdit = 2,

        /// <summary>
        /// Meeting start
        /// Arabic: بدء اجتماع
        /// </summary>
        [Description("Meeting Start")]
        MeetingStart = 3,

        /// <summary>
        /// Meeting end/completion
        /// Arabic: إنهاء اجتماع
        /// </summary>
        [Description("Meeting End")]
        MeetingEnd = 4,

        /// <summary>
        /// Meeting cancellation
        /// Arabic: إلغاء اجتماع
        /// </summary>
        [Description("Meeting Cancellation")]
        MeetingCancellation = 5,

        /// <summary>
        /// Meeting postponement
        /// Arabic: تأجيل اجتماع
        /// </summary>
        [Description("Meeting Postponement")]
        MeetingPostponement = 6,

        /// <summary>
        /// Meeting time proposal creation
        /// Arabic: إنشاء مقترح موعد اجتماع
        /// </summary>
        [Description("Meeting Time Proposal Creation")]
        MeetingTimeProposalCreation = 7,

        /// <summary>
        /// Meeting time vote submission
        /// Arabic: إرسال تصويت على موعد اجتماع
        /// </summary>
        [Description("Meeting Time Vote Submission")]
        MeetingTimeVoteSubmission = 8,

        /// <summary>
        /// Meeting minutes creation
        /// Arabic: إنشاء محضر اجتماع
        /// </summary>
        [Description("Meeting Minutes Creation")]
        MeetingMinutesCreation = 9,

        /// <summary>
        /// Meeting minutes signature
        /// Arabic: توقيع محضر اجتماع
        /// </summary>
        [Description("Meeting Minutes Signature")]
        MeetingMinutesSignature = 10
    }
}
