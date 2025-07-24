using System.ComponentModel;

namespace Domain.Entities.AssessmentManagement
{
    /// <summary>
    /// Enumeration representing the types of assessments
    /// Based on user story requirements from AssessmentStories.md
    /// </summary>
    public enum AssessmentType
    {
        /// <summary>
        /// Questionnaire-based assessment with questions
        /// Arabic: استبيان
        /// </summary>
        [Description("Questionnaire")]
        Questionnaire = 1,

        /// <summary>
        /// Attachment-based assessment for document review
        /// Arabic: مرفق
        /// </summary>
        [Description("Attachment")]
        Attachment = 2
    }
}
