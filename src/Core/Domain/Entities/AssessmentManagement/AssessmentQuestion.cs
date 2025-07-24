using Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.AssessmentManagement
{
    /// <summary>
    /// Represents a question within an assessment
    /// Inherits from FullAuditedEntity to provide comprehensive audit trail functionality
    /// Properties are defined based on requirements in AssessmentStories.md
    /// </summary>
    public class AssessmentQuestion : FullAuditedEntity
    {
        /// <summary>
        /// Foreign key reference to the Assessment this question belongs to
        /// Required field as specified in user stories
        /// </summary>
        public int AssessmentId { get; set; }

        /// <summary>
        /// Text content of the question
        /// Required field with maximum 1000 characters as specified in user stories
        /// </summary>
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Type of question (SingleChoice or Text)
        /// Required field as specified in user stories
        /// </summary>
        public QuestionType QuestionType { get; set; }

        /// <summary>
        /// JSON string containing options for single choice questions
        /// Optional field, required only when QuestionType is SingleChoice
        /// Stores array of options like ["Option 1", "Option 2", "Option 3"]
        /// </summary>
        public string? Options { get; set; }

        /// <summary>
        /// Display order of the question within the assessment
        /// Used for maintaining question sequence
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Indicates if this question is required to be answered
        /// Default is true for mandatory questions
        /// </summary>
        public bool IsRequired { get; set; } = true;

        /// <summary>
        /// Navigation property to the Assessment this question belongs to
        /// Provides access to assessment information
        /// </summary>
        [ForeignKey("AssessmentId")]
        public virtual Assessment Assessment { get; set; } = null!;

        /// <summary>
        /// Collection navigation property to Answer entities
        /// Represents all answers submitted for this question
        /// </summary>
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
    }
}
