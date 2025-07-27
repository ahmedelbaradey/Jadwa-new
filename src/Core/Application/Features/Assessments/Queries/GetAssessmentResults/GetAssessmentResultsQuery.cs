using Abstraction.Base.Response;
using Application.Base.Abstracts;
using Domain.Entities.AssessmentManagement;

namespace Application.Features.Assessments.Queries.GetAssessmentResults
{
    /// <summary>
    /// Query for getting compiled assessment results
    /// Implements User Story 5: View Compiled Assessment Results from AssessmentStories.md
    /// Follows CQRS pattern with IQuery interface from Abstract project
    /// </summary>
    public class GetAssessmentResultsQuery : IQuery<BaseResponse<GetAssessmentResultsResponse>>
    {
        /// <summary>
        /// Assessment ID to get results for
        /// </summary>
        public int AssessmentId { get; set; }
    }

    /// <summary>
    /// Response DTO for assessment results
    /// Contains compiled results and statistics
    /// </summary>
    public class GetAssessmentResultsResponse
    {
        /// <summary>
        /// Assessment information
        /// </summary>
        public AssessmentResultsDto Assessment { get; set; } = new AssessmentResultsDto();

        /// <summary>
        /// Completion statistics
        /// </summary>
        public CompletionStatisticsDto Statistics { get; set; } = new CompletionStatisticsDto();

        /// <summary>
        /// Question results for questionnaire type
        /// </summary>
        public List<QuestionResultDto> QuestionResults { get; set; } = new List<QuestionResultDto>();

        /// <summary>
        /// Text responses for attachment type or text questions
        /// </summary>
        public List<TextResponseDto> TextResponses { get; set; } = new List<TextResponseDto>();
    }

    /// <summary>
    /// Assessment basic information for results view
    /// </summary>
    public class AssessmentResultsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public AssessmentType Type { get; set; }
        public string TypeDisplayName { get; set; } = string.Empty;
        public AssessmentStatus Status { get; set; }
        public string StatusDisplayName { get; set; } = string.Empty;
        public DateTime? DistributionDate { get; set; }
        public string FundName { get; set; } = string.Empty;
        public string? AttachmentUrl { get; set; }
    }

    /// <summary>
    /// Completion statistics
    /// </summary>
    public class CompletionStatisticsDto
    {
        public int TotalBoardMembers { get; set; }
        public int CompletedResponses { get; set; }
        public int PendingResponses { get; set; }
        public decimal CompletionRate { get; set; }
        public DateTime? LastResponseDate { get; set; }
    }

    /// <summary>
    /// Results for individual questions (questionnaire type)
    /// </summary>
    public class QuestionResultDto
    {
        public int QuestionId { get; set; }
        public string QuestionText { get; set; } = string.Empty;
        public QuestionType QuestionType { get; set; }
        public string QuestionTypeDisplayName { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsRequired { get; set; }
        public int ResponseCount { get; set; }

        // For single choice questions
        public List<ChoiceResultDto> ChoiceResults { get; set; } = new List<ChoiceResultDto>();

        // For text questions
        public List<TextAnswerDto> TextAnswers { get; set; } = new List<TextAnswerDto>();
    }

    /// <summary>
    /// Results for single choice options
    /// </summary>
    public class ChoiceResultDto
    {
        public string OptionText { get; set; } = string.Empty;
        public int Count { get; set; }
        public decimal Percentage { get; set; }
    }

    /// <summary>
    /// Text answer with respondent information
    /// </summary>
    public class TextAnswerDto
    {
        public string AnswerText { get; set; } = string.Empty;
        public string RespondentName { get; set; } = string.Empty;
        public DateTime SubmissionDate { get; set; }
    }

    /// <summary>
    /// Text responses for attachment type assessments
    /// </summary>
    public class TextResponseDto
    {
        public string Comments { get; set; } = string.Empty;
        public string RespondentName { get; set; } = string.Empty;
        public DateTime SubmissionDate { get; set; }
    }
}
