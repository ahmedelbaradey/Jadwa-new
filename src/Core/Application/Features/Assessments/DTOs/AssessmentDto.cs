using Domain.Entities.AssessmentManagement;

namespace Application.Features.Assessments.DTOs
{
    /// <summary>
    /// Data Transfer Object for Assessment entity
    /// Used for displaying assessment information in responses
    /// Based on user story requirements from AssessmentStories.md
    /// </summary>
    public class AssessmentDto
    {
        /// <summary>
        /// Assessment unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Fund ID this assessment belongs to
        /// </summary>
        public int FundId { get; set; }

        /// <summary>
        /// Fund name for display purposes
        /// </summary>
        public string FundName { get; set; } = string.Empty;

        /// <summary>
        /// Assessment title
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Type of assessment (Questionnaire or Attachment)
        /// </summary>
        public AssessmentType Type { get; set; }

        /// <summary>
        /// Localized type name for display
        /// </summary>
        public string TypeDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Current status of the assessment
        /// </summary>
        public AssessmentStatus Status { get; set; }

        /// <summary>
        /// Localized status name for display
        /// </summary>
        public string StatusDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Attachment ID reference (for attachment type assessments)
        /// </summary>
        public int? AttachmentId { get; set; }

        /// <summary>
        /// Attachment file URL (computed from AttachmentId using IPreviewUrlHelper)
        /// </summary>
        public string? AttachmentURL { get; set; }

        /// <summary>
        /// Comments from reviewer (for rejected assessments)
        /// </summary>
        public string? ReviewerComments { get; set; }

        /// <summary>
        /// Name of the reviewer who approved/rejected
        /// </summary>
        public string? ReviewerName { get; set; }

        /// <summary>
        /// Date when assessment was reviewed
        /// </summary>
        public DateTime? ReviewedDate { get; set; }

        /// <summary>
        /// Name of the user who created the assessment
        /// </summary>
        public string CreatedByName { get; set; } = string.Empty;

        /// <summary>
        /// Date when assessment was created
        /// </summary>
        public DateTime? CreatedAt { get; set; }

        /// <summary>
        /// Date when assessment was last updated
        /// </summary>
        public DateTime? UpdatedAt { get; set; }

        /// <summary>
        /// Number of questions in the assessment (for questionnaire type)
        /// </summary>
        public int QuestionCount { get; set; }

        /// <summary>
        /// Number of responses received
        /// </summary>
        public int ResponseCount { get; set; }

        /// <summary>
        /// Number of completed responses
        /// </summary>
        public int CompletedResponseCount { get; set; }

        /// <summary>
        /// Available actions for current user and assessment state
        /// </summary>
        public List<string> AvailableActions { get; set; } = new List<string>();

        /// <summary>
        /// Allowed status transitions from current state
        /// </summary>
        public List<AssessmentStatus> AllowedTransitions { get; set; } = new List<AssessmentStatus>();
    }
}
