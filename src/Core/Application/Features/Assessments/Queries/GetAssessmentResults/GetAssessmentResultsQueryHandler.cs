using Application.Features.Assessments.Queries.GetAssessmentResults;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Application.Base.Abstracts;
using System.Text.Json;

namespace Application.Features.Assessments.Queries.GetAssessmentResults
{
    /// <summary>
    /// Handler for GetAssessmentResultsQuery
    /// Implements business logic for viewing compiled assessment results
    /// Based on User Story 5: View Compiled Assessment Results from AssessmentStories.md
    /// Follows CQRS pattern with IQueryHandler interface from Abstract project
    /// </summary>
    public class GetAssessmentResultsQueryHandler : BaseResponseHandler, IQueryHandler<GetAssessmentResultsQuery, BaseResponse<GetAssessmentResultsResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;

        public GetAssessmentResultsQueryHandler(
            IRepositoryManager repository,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Handles the assessment results query
        /// </summary>
        /// <param name="request">The query request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response with compiled assessment results</returns>
        public async Task<BaseResponse<GetAssessmentResultsResponse>> Handle(GetAssessmentResultsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get assessment with fund and questions
                var assessment = await _repository.Assessments.GetAssessmentWithDetailsAsync(request.AssessmentId);
                if (assessment == null)
                {
                    return NotFound<GetAssessmentResultsResponse>(_localizer["Assessment not found"]);
                }

                // Validate assessment status - must be Active or Completed
                if (assessment.Status != AssessmentStatus.Active && assessment.Status != AssessmentStatus.Completed)
                {
                    return BadRequest<GetAssessmentResultsResponse>(
                        _localizer["Assessment results are only available for Active or Completed assessments. Current status: {0}", assessment.Status]);
                }

                // TODO: Add proper authorization check for management roles
                var currentUserId = _currentUserService.GetUserId();

                // Get all responses for this assessment
                var responses = await _repository.AssessmentResponses.GetResponsesByAssessmentIdAsync(request.AssessmentId);

                // Get completion statistics
                var statistics = await _repository.AssessmentResponses.GetCompletionStatisticsAsync(request.AssessmentId);

                // Build response
                var response = new GetAssessmentResultsResponse
                {
                    Assessment = new AssessmentResultsDto
                    {
                        Id = assessment.Id,
                        Title = assessment.Title,
                        Type = assessment.Type,
                        TypeDisplayName = GetLocalizedAssessmentTypeName(assessment.Type),
                        Status = assessment.Status,
                        StatusDisplayName = GetLocalizedStatusName(assessment.Status),
                        DistributionDate = assessment.DistributionDate,
                        FundName = assessment.Fund?.Name ?? string.Empty,
                        AttachmentUrl = assessment.Attachment?.FilePath
                    },
                    Statistics = new CompletionStatisticsDto
                    {
                        TotalBoardMembers = statistics.Total,
                        CompletedResponses = statistics.Completed,
                        PendingResponses = statistics.Pending,
                        CompletionRate = statistics.Total > 0 ? (decimal)statistics.Completed / statistics.Total * 100 : 0,
                        LastResponseDate = responses.Where(r => r.SubmissionDate.HasValue)
                            .OrderByDescending(r => r.SubmissionDate)
                            .FirstOrDefault()?.SubmissionDate
                    }
                };

                // Process results based on assessment type
                if (assessment.Type == AssessmentType.Questionnaire)
                {
                    response.QuestionResults = await ProcessQuestionnaireResults(assessment.Questions, responses);
                }
                else if (assessment.Type == AssessmentType.Attachment)
                {
                    response.TextResponses = await ProcessAttachmentResults(responses);
                }

                return Success(response);
            }
            catch (Exception ex)
            {
                return ServerError<GetAssessmentResultsResponse>(_localizer["An error occurred while retrieving assessment results: {0}", ex.Message]);
            }
        }

        /// <summary>
        /// Processes questionnaire results by question
        /// </summary>
        private async Task<List<QuestionResultDto>> ProcessQuestionnaireResults(
            ICollection<AssessmentQuestion> questions, IEnumerable<AssessmentResponse> responses)
        {
            var questionResults = new List<QuestionResultDto>();
            var completedResponses = responses.Where(r => r.Status == ResponseStatus.Completed).ToList();

            foreach (var question in questions.OrderBy(q => q.DisplayOrder))
            {
                var questionResult = new QuestionResultDto
                {
                    QuestionId = question.Id,
                    QuestionText = question.QuestionText,
                    QuestionType = question.QuestionType,
                    QuestionTypeDisplayName = GetLocalizedQuestionTypeName(question.QuestionType),
                    DisplayOrder = question.DisplayOrder,
                    IsRequired = question.IsRequired
                };

                // Get answers for this question
                var answers = new List<Answer>();
                foreach (var response in completedResponses)
                {
                    var responseAnswers = await _repository.Answers.GetAnswersByResponseIdAsync(response.Id);
                    answers.AddRange(responseAnswers.Where(a => a.QuestionId == question.Id));
                }

                questionResult.ResponseCount = answers.Count;

                if (question.QuestionType == QuestionType.SingleChoice)
                {
                    questionResult.ChoiceResults = ProcessSingleChoiceResults(question, answers);
                }
                else if (question.QuestionType == QuestionType.Text)
                {
                    questionResult.TextAnswers = ProcessTextAnswers(answers, completedResponses);
                }

                questionResults.Add(questionResult);
            }

            return questionResults;
        }

        /// <summary>
        /// Processes single choice question results
        /// </summary>
        private List<ChoiceResultDto> ProcessSingleChoiceResults(AssessmentQuestion question, List<Answer> answers)
        {
            var choiceResults = new List<ChoiceResultDto>();

            if (string.IsNullOrEmpty(question.Options) || !answers.Any())
                return choiceResults;

            try
            {
                var options = JsonSerializer.Deserialize<List<string>>(question.Options) ?? new List<string>();
                var answerCounts = answers.GroupBy(a => a.AnswerText)
                    .ToDictionary(g => g.Key, g => g.Count());

                var totalAnswers = answers.Count;

                foreach (var option in options)
                {
                    var count = answerCounts.GetValueOrDefault(option, 0);
                    choiceResults.Add(new ChoiceResultDto
                    {
                        OptionText = option,
                        Count = count,
                        Percentage = totalAnswers > 0 ? (decimal)count / totalAnswers * 100 : 0
                    });
                }
            }
            catch
            {
                // If JSON deserialization fails, return empty results
            }

            return choiceResults;
        }

        /// <summary>
        /// Processes text answers
        /// </summary>
        private List<TextAnswerDto> ProcessTextAnswers(List<Answer> answers, List<AssessmentResponse> responses)
        {
            var textAnswers = new List<TextAnswerDto>();

            foreach (var answer in answers)
            {
                var response = responses.FirstOrDefault(r => r.Id == answer.ResponseId);
                if (response != null)
                {
                    textAnswers.Add(new TextAnswerDto
                    {
                        AnswerText = answer.AnswerText,
                        RespondentName = response.User?.FullName ?? "Unknown",
                        SubmissionDate = response.SubmissionDate ?? DateTime.MinValue
                    });
                }
            }

            return textAnswers.OrderBy(ta => ta.SubmissionDate).ToList();
        }

        /// <summary>
        /// Processes attachment type assessment results
        /// </summary>
        private async Task<List<TextResponseDto>> ProcessAttachmentResults(IEnumerable<AssessmentResponse> responses)
        {
            var textResponses = new List<TextResponseDto>();
            var completedResponses = responses.Where(r => r.Status == ResponseStatus.Completed);

            foreach (var response in completedResponses)
            {
                var answers = await _repository.Answers.GetAnswersByResponseIdAsync(response.Id);
                var comments = answers.FirstOrDefault()?.AnswerText ?? string.Empty;

                if (!string.IsNullOrWhiteSpace(comments))
                {
                    textResponses.Add(new TextResponseDto
                    {
                        Comments = comments,
                        RespondentName = response.User?.FullName ?? "Unknown",
                        SubmissionDate = response.SubmissionDate ?? DateTime.MinValue
                    });
                }
            }

            return textResponses.OrderBy(tr => tr.SubmissionDate).ToList();
        }

        /// <summary>
        /// Gets localized assessment type name
        /// </summary>
        private string GetLocalizedAssessmentTypeName(AssessmentType type)
        {
            return type switch
            {
                AssessmentType.Questionnaire => _localizer["Questionnaire"],
                AssessmentType.Attachment => _localizer["Attachment"],
                _ => type.ToString()
            };
        }

        /// <summary>
        /// Gets localized question type name
        /// </summary>
        private string GetLocalizedQuestionTypeName(QuestionType type)
        {
            return type switch
            {
                QuestionType.Text => _localizer["Text"],
                QuestionType.SingleChoice => _localizer["Single Choice"],
                _ => type.ToString()
            };
        }

        /// <summary>
        /// Gets localized status name
        /// </summary>
        private string GetLocalizedStatusName(AssessmentStatus status)
        {
            return status switch
            {
                AssessmentStatus.Draft => _localizer["Draft"],
                AssessmentStatus.WaitingForApproval => _localizer["Waiting for Approval"],
                AssessmentStatus.Approved => _localizer["Approved"],
                AssessmentStatus.Active => _localizer["Active"],
                AssessmentStatus.Completed => _localizer["Completed"],
                AssessmentStatus.Rejected => _localizer["Rejected"],
                _ => status.ToString()
            };
        }
    }
}
