using System.ComponentModel;

namespace Domain.Entities.ResolutionManagement
{
    /// <summary>
    /// Enumeration representing the various actions of a resolution (status changes) and their corresponding status values.
    /// Based on requirements in Sprint.md for resolution management workflow
    /// </summary>
    public enum ResolutionActionEnum
    {
        /// <summary>
        /// Resolution creation
        /// Arabic: إنشاء قرار
        /// </summary>
        [Description("Resolution Creation")]
        ResolutionCreation = 1,

        /// <summary>
        /// Resolution edit
        /// Arabic: تعديل قرار
        /// </summary>
        [Description("Resolution Edit")]
        ResolutionEdit = 2,

        /// <summary>   
        /// Resolution completion
        /// Arabic: استكمال قرار
        /// </summary>
        [Description("Resolution Completion")]
        ResolutionCompletion = 3,        /// <summary>   

        /// <summary>   
        /// Resolution confirmation
        /// Arabic: تأكيد قرار
        /// </summary>
        [Description("Resolution Confirmation")]
        ResolutionConfirmation = 4,

        /// <summary>   
        /// Resolution rejection
        /// Arabic: رفض قرار
        /// </summary>
        [Description("Resolution Rejection")]
        ResolutionRejection = 5,

        /// <summary>   
        /// Resolution cancellation
        /// Arabic: إلغاء قرار
        /// </summary>
        [Description("Resolution Cancellation")]
        ResolutionCancellation = 6,
        /// <summary>   
        /// Resolution Approved
        /// Arabic: موافقة قرار
        /// </summary>
        [Description("Resolution Approved")]
        ResolutionApproved = 7,
        /// <summary>
        /// Resolution UnApproved
        /// Arabic: غير موافقة قرار
        /// </summary>
        [Description("Resolution UnApproved")]
        ResolutionUnApproved = 8,

        /// <summary>
        /// Resolution confirmed by fund manager
        /// Arabic: تأكيد القرار
        /// </summary>
        [Description("Resolution Confirmed")]
        Confirmed = 9,


        /// <summary>
        /// Resolution sent to vote
        /// Arabic: إرسال القرار للتصويت
        /// </summary>
        [Description("Resolution Sent to Vote")]
        SentToVote = 10,

        /// <summary>
        /// Resolution voting suspended for editing (Alternative 1 workflow)
        /// Arabic: تعليق التصويت للتعديل
        /// </summary>
        [Description("Resolution Vote Suspended")]
        ResolutionVoteSuspend = 11,

        /// <summary>
        /// Resolution deletion
        /// Arabic: حذف القرار
        /// </summary>
        [Description("Resolution Deletion")]
        ResolutionDeletion = 12
    }
}
