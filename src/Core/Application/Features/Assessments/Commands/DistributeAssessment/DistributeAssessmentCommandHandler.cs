using Application.Features.Assessments.Commands.DistributeAssessment;
using Abstraction.Base.Response;
using Abstraction.Contracts.Repository;
using Domain.Entities.AssessmentManagement;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contract.Service;
using Application.Base.Abstracts;
using Application.Services;

namespace Application.Features.Assessments.Commands.DistributeAssessment
{
    /// <summary>
    /// Handler for DistributeAssessmentCommand
    /// Implements business logic for distributing approved assessments to board members
    /// Based on User Story 3: Distribute Assessment from AssessmentStories.md
    /// Follows CQRS pattern with ICommandHandler interface from Abstract project
    /// </summary>
    public class DistributeAssessmentCommandHandler : BaseResponseHandler, ICommandHandler<DistributeAssessmentCommand, BaseResponse<DistributeAssessmentResponse>>
    {
        private readonly IRepositoryManager _repository;
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly ICurrentUserService _currentUserService;

        public DistributeAssessmentCommandHandler(
            IRepositoryManager repository,
            IStringLocalizer<SharedResources> localizer,
            ICurrentUserService currentUserService)
        {
            _repository = repository;
            _localizer = localizer;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Handles the assessment distribution command
        /// </summary>
        /// <param name="request">The command request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response with distribution result information</returns>
        public async Task<BaseResponse<DistributeAssessmentResponse>> Handle(DistributeAssessmentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                // Get assessment with fund information
                var assessment = await _repository.Assessments.GetByIdAsync(request.AssessmentId);
                if (assessment == null)
                {
                    return NotFound<DistributeAssessmentResponse>(_localizer["Assessment not found"]);
                }

                // Initialize state pattern
                assessment.InitializeState(_localizer, _currentUserService, _repository);

                // Validate current status - must be Approved
                if (assessment.Status != AssessmentStatus.Approved)
                {
                    return BadRequest<DistributeAssessmentResponse>(
                        _localizer["Assessment must be approved before distribution. Current status: {0}", assessment.Status]);
                }

                // Validate user permissions (Fund Manager only)
                var currentUserId = _currentUserService.GetUserId();
                // TODO: Add proper authorization check for Fund Manager role

                // Transition to Active status
                var stateContext = new AssessmentStateContext(assessment, _localizer, _repository, _currentUserService);
                var transitionResult = stateContext.TransitionTo(AssessmentStatus.Active, "Assessment distributed to board members");
                
                if (!transitionResult)
                {
                    return BadRequest<DistributeAssessmentResponse>(_localizer["Cannot distribute assessment in current state"]);
                }

                // Update assessment status and distribution date
                assessment.Status = AssessmentStatus.Active;
                assessment.DistributionDate = DateTime.UtcNow;

                // Create response records for all board members
                var boardMemberCount = await _repository.AssessmentResponses.CreateResponsesForBoardMembersAsync(
                    assessment.Id, assessment.FundId);

                if (boardMemberCount == 0)
                {
                    return BadRequest<DistributeAssessmentResponse>(_localizer["No board members found for this fund"]);
                }

                // Save changes
                await _repository.SaveAsync();

                // Handle state-specific logic
                assessment.Handle();

                // TODO: Send notifications to board members
                // This will be implemented when we integrate with the notification system

                // Prepare response
                var response = new DistributeAssessmentResponse
                {
                    AssessmentId = assessment.Id,
                    Title = assessment.Title,
                    Status = assessment.Status.ToString(),
                    StatusDisplayName = GetLocalizedStatusName(assessment.Status),
                    BoardMemberCount = boardMemberCount,
                    Message = _localizer["Assessment successfully distributed to {0} board members", boardMemberCount],
                    DistributionDate = assessment.DistributionDate ?? DateTime.UtcNow,
                    AvailableActions = assessment.GetAvailableActions()
                };

                return Success(response);
            }
            catch (Exception ex)
            {
                return ServerError<DistributeAssessmentResponse>(_localizer["An error occurred while distributing the assessment: {0}", ex.Message]);
            }
        }

        /// <summary>
        /// Gets localized status name for display
        /// </summary>
        /// <param name="status">Assessment status</param>
        /// <returns>Localized status name</returns>
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
