using Application.Features.Assessments.DTOs;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using AutoMapper;
using Domain.Entities.AssessmentManagement;
using Domain.States.AssessmentStates;
using Microsoft.Extensions.Localization;
using Resources;
using System.Text.Json;
using Abstraction.Contract.Service;
using Application.Base.Abstracts;
using Application.Services;

namespace Application.Features.Assessments.Commands.AddAssessment
{
    /// <summary>
    /// Handler for AddAssessmentCommand
    /// Implements business logic for creating new assessments
    /// Based on User Story 1: Create New Assessment from AssessmentStories.md
    /// Follows CQRS pattern with ICommandHandler interface from Abstract project
    /// </summary>
    public class AddAssessmentCommandHandler : BaseResponseHandler, ICommandHandler<AddAssessmentCommand, BaseResponse<AddAssessmentResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;

        public AddAssessmentCommandHandler(
            IRepositoryManager repository,
            IMapper mapper,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Handles the assessment creation command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response with created assessment information</returns>
        public async Task<BaseResponse<AddAssessmentResponse>> Handle(AddAssessmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate fund exists and user has access
                var fund = await _repository.Funds.GetByIdAsync(request.FundId);
                if (fund == null)
                {
                    return BadRequest<AddAssessmentResponse>(_localizer[SharedResourcesKey.InvalidFund]);
                }

                // Check if assessment title already exists for this fund
                var titleExists = await _repository.Assessments.IsTitleExistsAsync(
                    request.FundId,
                    request.Title);

                if (titleExists)
                {
                    return BadRequest<AddAssessmentResponse>(_localizer["Assessment title already exists for this fund"]);
                }

                // Create assessment entity
                var assessment = new Assessment
                {
                    FundId = request.FundId,
                    Title = request.Title.Trim(),
                    Type = request.Type,
                    AttachmentId = request.AttachmentId,
                    Status = request.SaveAsDraft ? AssessmentStatus.Draft : AssessmentStatus.WaitingForApproval
                };

                // Initialize state pattern
                assessment.InitializeState();

                // Validate state transition and business rules using localized context
                var stateContext = new AssessmentStateContext(assessment, _localizer, _repository, _currentUserService);
                var (isValid, validationMessages) = stateContext.ValidateCurrentState();
                if (!isValid)
                {
                    return BadRequest<AddAssessmentResponse>(string.Join(", ", validationMessages));
                }

                // Add assessment to repository
                await _repository.Assessments.AddAsync(assessment);

                // Create questions for questionnaire type
                if (request.Type == AssessmentType.Questionnaire && request.Questions.Any())
                {
                    var questions = new List<AssessmentQuestion>();

                    foreach (var questionDto in request.Questions.OrderBy(q => q.DisplayOrder))
                    {
                        var question = new AssessmentQuestion
                        {
                            AssessmentId = assessment.Id,
                            QuestionText = questionDto.QuestionText.Trim(),
                            QuestionType = questionDto.QuestionType,
                            DisplayOrder = questionDto.DisplayOrder,
                            IsRequired = questionDto.IsRequired
                        };

                        // Serialize options for single choice questions
                        if (questionDto.QuestionType == QuestionType.SingleChoice && questionDto.Options.Any())
                        {
                            question.Options = JsonSerializer.Serialize(questionDto.Options);
                        }

                        questions.Add(question);
                        await _repository.AssessmentQuestions.AddAsync(question);
                    }
                }

                // Save changes
                await _repository.SaveAsync();

                // Handle state-specific logic
                assessment.Handle();

                // Send notifications if submitted for approval
                if (!request.SaveAsDraft)
                {
                    // TODO: Send notification to Legal Council and Board Secretary
                    // This will be implemented when we integrate with the notification system
                }

                // Prepare response
                var response = new AddAssessmentResponse
                {
                    AssessmentId = assessment.Id,
                    Title = assessment.Title,
                    Status = assessment.Status,
                    StatusDisplayName = GetLocalizedStatusName(assessment.Status),
                    Message = GetSuccessMessage(assessment.Status),
                    QuestionCount = request.Questions.Count,
                    AvailableActions = assessment.GetAvailableActions()
                };

                return Success(response);
            }
            catch (Exception ex)
            {
                return ServerError<AddAssessmentResponse>($"An error occurred while creating the assessment: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets localized status name based on current culture
        /// </summary>
        /// <param name="status">Assessment status</param>
        /// <returns>Localized status name</returns>
        private string GetLocalizedStatusName(AssessmentStatus status)
        {
            return status switch
            {
                AssessmentStatus.Draft => _localizer[SharedResourcesKey.AssessmentStatusDraft],
                AssessmentStatus.WaitingForApproval => _localizer[SharedResourcesKey.AssessmentStatusWaitingForApproval],
                AssessmentStatus.Approved => _localizer[SharedResourcesKey.AssessmentStatusApproved],
                AssessmentStatus.Rejected => _localizer[SharedResourcesKey.AssessmentStatusRejected],
                AssessmentStatus.Active => _localizer[SharedResourcesKey.AssessmentStatusActive],
                AssessmentStatus.Completed => _localizer[SharedResourcesKey.AssessmentStatusCompleted],
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Gets success message based on assessment status
        /// </summary>
        /// <param name="status">Assessment status</param>
        /// <returns>Localized success message</returns>
        private string GetSuccessMessage(AssessmentStatus status)
        {
            return status switch
            {
                AssessmentStatus.Draft => _localizer[SharedResourcesKey.AssessmentSavedAsDraft],
                AssessmentStatus.WaitingForApproval => _localizer[SharedResourcesKey.AssessmentSubmittedForApproval],
                _ => _localizer[SharedResourcesKey.RecordSavedSuccessfully]
            };
        }
    }
}
