using Domain.Entities.AssessmentManagement;

namespace Application.Features.Assessments.DTOs
{
    /// <summary>
    /// Data Transfer Object for creating new assessments
    /// Based on User Story 1: Create New Assessment from AssessmentStories.md
    /// </summary>
    public class AddAssessmentDto
    {
        /// <summary>
        /// Fund ID this assessment belongs to
        /// Required field as specified in user stories
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Assessment title
        /// Required field with maximum 255 characters as specified in user stories
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Type of assessment (Questionnaire or Attachment)
        /// Required field as specified in user stories
        /// </summary>
        public AssessmentType Type { get; set; }

        /// <summary>
        /// Attachment ID reference (required for Attachment type assessments)
        /// Optional field, required only when Type is Attachment
        /// </summary>
        public int? AttachmentId { get; set; }

        /// <summary>
        /// List of questions for questionnaire type assessments
        /// Required for Questionnaire type, must have at least one question
        /// </summary>
        public List<AddAssessmentQuestionDto> Questions { get; set; } = new List<AddAssessmentQuestionDto>();

        /// <summary>
        /// Indicates whether to save as draft or submit for approval
        /// True = save as draft, False = submit for approval
        /// </summary>
        public bool SaveAsDraft { get; set; } = true;
    }

    /// <summary>
    /// Response DTO for assessment creation operations
    /// Contains the created assessment information and status
    /// </summary>
    public class AddAssessmentResponse
    {
        /// <summary>
        /// Created assessment ID
        /// </summary>
        public int AssessmentId { get; set; }

        /// <summary>
        /// Assessment title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Current status after creation
        /// </summary>
        public AssessmentStatus Status { get; set; }

        /// <summary>
        /// Localized status name for display
        /// </summary>
        public string StatusDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Success message based on action taken
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Number of questions created (for questionnaire type)
        /// </summary>
        public int QuestionCount { get; set; }

        /// <summary>
        /// Available actions for the created assessment
        /// </summary>
        public List<string> AvailableActions { get; set; } = new List<string>();
    }
}
