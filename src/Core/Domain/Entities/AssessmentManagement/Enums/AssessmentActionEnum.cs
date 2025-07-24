using System.ComponentModel;

namespace Domain.Entities.AssessmentManagement
{
    /// <summary>
    /// Enumeration representing the various actions of an assessment (status changes) and their corresponding action values.
    /// Based on requirements for assessment management workflow
    /// Follows the same pattern as ResolutionActionEnum for consistency
    /// </summary>
    public enum AssessmentActionEnum
    {
        /// <summary>
        /// Assessment creation
        /// Arabic: إنشاء تقييم
        /// </summary>
        [Description("Assessment Creation")]
        Creation = 1,

        /// <summary>
        /// Assessment edit
        /// Arabic: تعديل تقييم
        /// </summary>
        [Description("Assessment Edit")]
        Edit = 2,

        /// <summary>
        /// Assessment submission for approval
        /// Arabic: إرسال التقييم للموافقة
        /// </summary>
        [Description("Assessment Submission")]
        Submission = 3,

        /// <summary>
        /// Assessment approval
        /// Arabic: موافقة التقييم
        /// </summary>
        [Description("Assessment Approval")]
        Approval = 4,

        /// <summary>
        /// Assessment rejection
        /// Arabic: رفض التقييم
        /// </summary>
        [Description("Assessment Rejection")]
        Rejection = 5,

        /// <summary>
        /// Assessment distribution to board members
        /// Arabic: توزيع التقييم
        /// </summary>
        [Description("Assessment Distribution")]
        Distribution = 6,

        /// <summary>
        /// Assessment response submission
        /// Arabic: إرسال إجابة التقييم
        /// </summary>
        [Description("Assessment Response Submission")]
        ResponseSubmission = 7,

        /// <summary>
        /// Assessment completion
        /// Arabic: إكمال التقييم
        /// </summary>
        [Description("Assessment Completion")]
        Completion = 8,

        /// <summary>
        /// Assessment status change
        /// Arabic: تغيير حالة التقييم
        /// </summary>
        [Description("Assessment Status Change")]
        StatusChange = 9,

        /// <summary>
        /// Assessment deletion
        /// Arabic: حذف التقييم
        /// </summary>
        [Description("Assessment Deletion")]
        Delete = 10,

        /// <summary>
        /// Assessment archiving
        /// Arabic: أرشفة التقييم
        /// </summary>
        [Description("Assessment Archive")]
        Archive = 11,

        /// <summary>
        /// Assessment view details
        /// Arabic: عرض تفاصيل التقييم
        /// </summary>
        [Description("Assessment View Details")]
        ViewDetails = 12,

        /// <summary>
        /// Assessment save
        /// Arabic: حفظ التقييم
        /// </summary>
        [Description("Assessment Save")]
        Save = 13,

        /// <summary>
        /// Assessment respond
        /// Arabic: الرد على التقييم
        /// </summary>
        [Description("Assessment Respond")]
        Respond = 14,

        /// <summary>
        /// Assessment view rejection reason
        /// Arabic: عرض سبب الرفض
        /// </summary>
        [Description("Assessment View Rejection Reason")]
        ViewRejectionReason = 15,

        /// <summary>
        /// Assessment resubmit
        /// Arabic: إعادة إرسال التقييم
        /// </summary>
        [Description("Assessment Resubmit")]
        Resubmit = 16,

        /// <summary>
        /// Assessment view responses
        /// Arabic: عرض الإجابات
        /// </summary>
        [Description("Assessment View Responses")]
        ViewResponses = 17,

        /// <summary>
        /// Assessment view results
        /// Arabic: عرض النتائج
        /// </summary>
        [Description("Assessment View Results")]
        ViewResults = 18,

        /// <summary>
        /// Assessment complete assessment
        /// Arabic: إغلاق التقييم
        /// </summary>
        [Description("Assessment Complete Assessment")]
        CompleteAssessment = 19,

        /// <summary>
        /// Assessment export results
        /// Arabic: تصدير النتائج
        /// </summary>
        [Description("Assessment Export Results")]
        ExportResults = 20,

        /// <summary>
        /// Assessment export data
        /// Arabic: تصدير البيانات
        /// </summary>
        [Description("Assessment Export Data")]
        ExportData = 21
    }
}
