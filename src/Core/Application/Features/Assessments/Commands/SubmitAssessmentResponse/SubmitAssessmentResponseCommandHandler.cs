using Application.Features.Assessments.Commands.SubmitAssessmentResponse;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Application.Base.Abstracts;

namespace Application.Features.Assessments.Commands.SubmitAssessmentResponse
{
    /// <summary>
    /// Handler for SubmitAssessmentResponseCommand
    /// Implements business logic for board members responding to assessments
    /// Based on User Story 4: Respond to Assessment from AssessmentStories.md
    /// Follows CQRS pattern with ICommandHandler interface from Abstract project
    /// </summary>
    public class SubmitAssessmentResponseCommandHandler : BaseResponseHandler, ICommandHandler<SubmitAssessmentResponseCommand, BaseResponse<SubmitAssessmentResponseResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;

        public SubmitAssessmentResponseCommandHandler(
            IRepositoryManager repository,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Handles the assessment response submission command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response with submission result information</returns>
        public async Task<BaseResponse<SubmitAssessmentResponseResponse>> Handle(SubmitAssessmentResponseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get assessment with questions
                var assessment = await _repository.Assessments.GetAssessmentWithQuestionsAsync(request.AssessmentId);
                if (assessment == null)
                {
                    return NotFound<SubmitAssessmentResponseResponse>(_localizer["Assessment not found"]);
                }

                // Validate assessment is active
                if (assessment.Status != AssessmentStatus.Active)
                {
                    return BadRequest<SubmitAssessmentResponseResponse>(
                        _localizer["Assessment is not active. Current status: {0}", assessment.Status]);
                }

                var currentUserId = _currentUserService.GetUserId();

                // Get or create response record
                var existingResponse = await _repository.AssessmentResponses.GetResponseByAssessmentAndUserAsync(
                    request.AssessmentId, currentUserId);

                bool isUpdate = existingResponse != null;
                AssessmentResponse response;

                if (existingResponse != null)
                {
                    response = existingResponse;
                    // Clear existing answers for update
                    var existingAnswers = await _repository.Answers.GetAnswersByResponseIdAsync(response.Id);
                    foreach (var answer in existingAnswers)
                    {
                        await _repository.Answers.DeleteAsync(answer);
                    }
                }
                else
                {
                    response = new AssessmentResponse
                    {
                        AssessmentId = request.AssessmentId,
                        UserId = currentUserId,
                        Status = ResponseStatus.Pending
                    };
                    await _repository.AssessmentResponses.AddAsync(response);
                }

                // Validate and process answers based on assessment type
                if (assessment.Type == AssessmentType.Questionnaire)
                {
                    var validationResult = await ValidateQuestionnaireAnswers(request.Answers, assessment.Questions);
                    if (!validationResult.IsValid)
                    {
                        return BadRequest<SubmitAssessmentResponseResponse>(validationResult.ErrorMessage);
                    }

                    // Create answer records
                    foreach (var answerDto in request.Answers)
                    {
                        var answer = new Answer
                        {
                            ResponseId = response.Id,
                            QuestionId = answerDto.QuestionId,
                            AnswerText = answerDto.AnswerText.Trim()
                        };
                        await _repository.Answers.AddAsync(answer);
                    }
                }
                else if (assessment.Type == AssessmentType.Attachment)
                {
                    // For attachment type, create a single answer with comments
                    if (!string.IsNullOrWhiteSpace(request.Comments))
                    {
                        var answer = new Answer
                        {
                            ResponseId = response.Id,
                            QuestionId = null, // No specific question for attachment type
                            AnswerText = request.Comments.Trim()
                        };
                        await _repository.Answers.AddAsync(answer);
                    }
                }

                // Update response status and submission date
                response.Status = ResponseStatus.Completed;
                response.SubmissionDate = DateTime.UtcNow;

                // Save changes
                await _repository.SaveAsync();

                // TODO: Send notification to Fund Manager about response submission
                // This will be implemented when we integrate with the notification system

                // Prepare response
                var responseDto = new SubmitAssessmentResponseResponse
                {
                    AssessmentId = assessment.Id,
                    AssessmentTitle = assessment.Title,
                    ResponseId = response.Id,
                    Status = response.Status,
                    StatusDisplayName = GetLocalizedResponseStatusName(response.Status),
                    AnswerCount = request.Answers.Count,
                    SubmissionDate = response.SubmissionDate ?? DateTime.UtcNow,
                    Message = isUpdate 
                        ? _localizer["Response updated successfully"] 
                        : _localizer["Response submitted successfully"],
                    IsUpdate = isUpdate
                };

                return Success(responseDto);
            }
            catch (Exception ex)
            {
                return ServerError<SubmitAssessmentResponseResponse>(_localizer["An error occurred while submitting the response: {0}", ex.Message]);
            }
        }

        /// <summary>
        /// Validates questionnaire answers
        /// </summary>
        private async Task<(bool IsValid, string ErrorMessage)> ValidateQuestionnaireAnswers(
            List<SubmitAnswerDto> answers, ICollection<AssessmentQuestion> questions)
        {
            // Check if all required questions are answered
            var requiredQuestions = questions.Where(q => q.IsRequired).ToList();
            var answeredQuestionIds = answers.Select(a => a.QuestionId).ToHashSet();

            foreach (var requiredQuestion in requiredQuestions)
            {
                if (!answeredQuestionIds.Contains(requiredQuestion.Id))
                {
                    return (false, _localizer["Required question '{0}' must be answered", requiredQuestion.QuestionText]);
                }
            }

            // Validate answer format for each question type
            foreach (var answer in answers)
            {
                var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (question == null)
                {
                    return (false, _localizer["Invalid question ID: {0}", answer.QuestionId]);
                }

                if (string.IsNullOrWhiteSpace(answer.AnswerText))
                {
                    return (false, _localizer["Answer cannot be empty for question: {0}", question.QuestionText]);
                }

                // Additional validation for single choice questions
                if (question.QuestionType == QuestionType.SingleChoice && !string.IsNullOrEmpty(question.Options))
                {
                    // TODO: Validate that the answer is one of the valid options
                    // This would require deserializing the options JSON and checking
                }
            }

            return (true, string.Empty);
        }

        /// <summary>
        /// Gets localized response status name for display
        /// </summary>
        private string GetLocalizedResponseStatusName(ResponseStatus status)
        {
            return status switch
            {
                ResponseStatus.Pending => _localizer["Pending"],
                ResponseStatus.Completed => _localizer["Completed"],
                _ => status.ToString()
            };
        }
    }
}
