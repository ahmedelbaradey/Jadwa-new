using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Domain.Entities.AssessmentManagement;

namespace Application.Features.Assessments.Commands.SubmitAssessmentResponse
{
    /// <summary>
    /// Command for submitting a response to an assessment
    /// Implements User Story 4: Respond to Assessment from AssessmentStories.md
    /// Follows CQRS pattern with ICommand interface from Abstract project
    /// </summary>
    public class SubmitAssessmentResponseCommand : ICommand<BaseResponse<SubmitAssessmentResponseResponse>>
    {
        /// <summary>
        /// Assessment ID to respond to
        /// </summary>
        public int AssessmentId { get; set; }

        /// <summary>
        /// List of answers for questionnaire type assessments
        /// </summary>
        public List<SubmitAnswerDto> Answers { get; set; } = new List<SubmitAnswerDto>();

        /// <summary>
        /// Comments for attachment type assessments
        /// </summary>
        public string? Comments { get; set; }
    }

    /// <summary>
    /// DTO for individual answer submission
    /// </summary>
    public class SubmitAnswerDto
    {
        /// <summary>
        /// Question ID being answered
        /// </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Answer text for text questions or selected option for single choice
        /// </summary>
        public string AnswerText { get; set; } = string.Empty;
    }

    /// <summary>
    /// Response DTO for assessment response submission
    /// Contains the submission result information
    /// </summary>
    public class SubmitAssessmentResponseResponse
    {
        /// <summary>
        /// Assessment ID that was responded to
        /// </summary>
        public int AssessmentId { get; set; }

        /// <summary>
        /// Assessment title
        /// </summary>
        public string AssessmentTitle { get; set; } = string.Empty;

        /// <summary>
        /// Response ID that was created/updated
        /// </summary>
        public int ResponseId { get; set; }

        /// <summary>
        /// Response status after submission
        /// </summary>
        public ResponseStatus Status { get; set; }

        /// <summary>
        /// Localized status name for display
        /// </summary>
        public string StatusDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Number of answers submitted
        /// </summary>
        public int AnswerCount { get; set; }

        /// <summary>
        /// Submission date
        /// </summary>
        public DateTime SubmissionDate { get; set; }

        /// <summary>
        /// Success message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Whether this was a new response or an update
        /// </summary>
        public bool IsUpdate { get; set; }
    }
}
