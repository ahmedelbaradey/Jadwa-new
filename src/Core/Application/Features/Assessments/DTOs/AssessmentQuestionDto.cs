using Domain.Entities.AssessmentManagement;

namespace Application.Features.Assessments.DTOs
{
    /// <summary>
    /// Data Transfer Object for AssessmentQuestion entity
    /// Used for displaying and managing assessment questions
    /// Based on user story requirements from AssessmentStories.md
    /// </summary>
    public class AssessmentQuestionDto
    {
        /// <summary>
        /// Question unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Assessment ID this question belongs to
        /// </summary>
        public int AssessmentId { get; set; }

        /// <summary>
        /// Text content of the question
        /// </summary>
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Type of question (SingleChoice or Text)
        /// </summary>
        public QuestionType QuestionType { get; set; }

        /// <summary>
        /// Localized question type name for display
        /// </summary>
        public string QuestionTypeDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Options for single choice questions (JSON array)
        /// </summary>
        public string? Options { get; set; }

        /// <summary>
        /// Parsed options for single choice questions
        /// </summary>
        public List<string> OptionsList { get; set; } = new List<string>();

        /// <summary>
        /// Display order of the question
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Indicates if this question is required
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// Date when question was created
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Date when question was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }

    /// <summary>
    /// Data Transfer Object for adding/editing assessment questions
    /// Used in create and update operations
    /// </summary>
    public class AddAssessmentQuestionDto
    {
        /// <summary>
        /// Question ID (0 for new questions)
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Text content of the question
        /// </summary>
        public string QuestionText { get; set; } = string.Empty;

        /// <summary>
        /// Type of question (SingleChoice or Text)
        /// </summary>
        public QuestionType QuestionType { get; set; }

        /// <summary>
        /// Options for single choice questions
        /// </summary>
        public List<string> Options { get; set; } = new List<string>();

        /// <summary>
        /// Display order of the question
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Indicates if this question is required
        /// </summary>
        public bool IsRequired { get; set; } = true;
    }
}
