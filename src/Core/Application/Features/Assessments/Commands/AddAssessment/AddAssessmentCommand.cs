using Application.Features.Assessments.DTOs;
using Abstraction.Base.Response;
using Application.Base.Abstracts;

namespace Application.Features.Assessments.Commands.AddAssessment
{
    /// <summary>
    /// Command for creating a new assessment
    /// Implements User Story 1: Create New Assessment from AssessmentStories.md
    /// Follows CQRS pattern with ICommand interface from Abstract project
    /// </summary>
    public record AddAssessmentCommand : AddAssessmentDto, ICommand<BaseResponse<AddAssessmentResponse>>
    {
        // Command inherits all properties from AddAssessmentDto
        // No additional properties needed unless specific to command execution
    }
}
