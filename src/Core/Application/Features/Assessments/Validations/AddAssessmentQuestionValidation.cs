using Application.Features.Assessments.DTOs;
using Domain.Entities.AssessmentManagement;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Assessments.Validations
{
    /// <summary>
    /// Validation rules for AssessmentQuestionDto
    /// Implements business rules for assessment questions
    /// Based on existing validation patterns in the codebase
    /// </summary>
    public class AddAssessmentQuestionValidation : AbstractValidator<AssessmentQuestionDto>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AddAssessmentQuestionValidation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        /// <summary>
        /// Applies all validation rules for assessment questions
        /// </summary>
        private void ApplyValidationRules()
        {
            // Question text validation
            RuleFor(x => x.QuestionText)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.QuestionTextRequired])
                .MaximumLength(1000)
                .WithMessage(_localizer[SharedResourcesKey.QuestionTextMaxLength]);

            // Question type validation
            RuleFor(x => x.QuestionType)
                .IsInEnum()
                .WithMessage(_localizer[SharedResourcesKey.QuestionTypeRequired]);

            // Display order validation
            RuleFor(x => x.DisplayOrder)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.QuestionDisplayOrderRequired]);

            // Options validation for single choice questions
            RuleFor(x => x.Options)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.QuestionOptionsRequired])
                .When(x => x.QuestionType == QuestionType.SingleChoice);

            RuleFor(x => x.Options)
                .Must(HaveAtLeastTwoOptions)
                .WithMessage(_localizer[SharedResourcesKey.QuestionOptionsMinimumTwo])
                .When(x => x.QuestionType == QuestionType.SingleChoice && x.Options.Any());

            // Options should not be provided for text questions
            RuleFor(x => x.Options)
                .Empty()
                .WithMessage(_localizer[SharedResourcesKey.QuestionOptionsNotAllowedForText])
                .When(x => x.QuestionType == QuestionType.Text);
        }

        /// <summary>
        /// Validates that single choice questions have at least 2 options
        /// </summary>
        /// <param name="options">List of options</param>
        /// <returns>True if valid, false otherwise</returns>
        private bool HaveAtLeastTwoOptions(List<string> options)
        {
            return options != null && options.Count >= 2 && options.All(o => !string.IsNullOrWhiteSpace(o));
        }
    }
}
