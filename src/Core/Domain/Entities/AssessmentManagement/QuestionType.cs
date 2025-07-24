using System.ComponentModel;

namespace Domain.Entities.AssessmentManagement
{
    /// <summary>
    /// Enumeration representing the types of questions in an assessment
    /// Based on user story requirements from AssessmentStories.md
    /// </summary>
    public enum QuestionType
    {
        /// <summary>
        /// Single choice question with predefined options
        /// Arabic: اختيار واحد
        /// </summary>
        [Description("SingleChoice")]
        SingleChoice = 1,

        /// <summary>
        /// Text-based question for free-form answers
        /// Arabic: نص
        /// </summary>
        [Description("Text")]
        Text = 2
    }
}
