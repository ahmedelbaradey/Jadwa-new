using Application.Features.Assessments.DTOs;
using Domain.Entities.AssessmentManagement;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Assessments.Validations
{
    /// <summary>
    /// Validation rules for AddAssessmentDto
    /// Implements business rules from User Story 1: Create New Assessment
    /// Based on existing validation patterns in the codebase
    /// </summary>
    public class AddAssessmentValidation : AbstractValidator<AddAssessmentDto>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AddAssessmentValidation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        /// <summary>
        /// Applies all validation rules for assessment creation
        /// </summary>
        private void ApplyValidationRules()
        {
            // Fund ID validation
            RuleFor(x => x.FundId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.EmptyIdValidation]);

            // Title validation
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.AssessmentTitleRequired])
                .MaximumLength(255)
                .WithMessage(_localizer[SharedResourcesKey.AssessmentTitleMaxLength]);

            // Type validation
            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage(_localizer[SharedResourcesKey.AssessmentTypeRequired]);

            // Attachment ID validation for Attachment type
            RuleFor(x => x.AttachmentId)
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKey.AttachmentRequired])
                .When(x => x.Type == AssessmentType.Attachment);

            RuleFor(x => x.AttachmentId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.InvalidAttachmentId])
                .When(x => x.AttachmentId.HasValue);

            // Questions validation for Questionnaire type
            RuleFor(x => x.Questions)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.AtLeastOneQuestionRequired])
                .When(x => x.Type == AssessmentType.Questionnaire);

            // Individual question validation
            RuleForEach(x => x.Questions)
                .SetValidator(new AddAssessmentQuestionValidation(_localizer))
                .When(x => x.Type == AssessmentType.Questionnaire);

            // Business rule: Questions should not be provided for Attachment type
            RuleFor(x => x.Questions)
                .Empty()
                .WithMessage(_localizer[SharedResourcesKey.QuestionsNotAllowedForAttachment])
                .When(x => x.Type == AssessmentType.Attachment);

            // Display order validation for questions
            RuleFor(x => x.Questions)
                .Must(HaveUniqueDisplayOrders)
                .WithMessage(_localizer[SharedResourcesKey.QuestionDisplayOrdersUnique])
                .When(x => x.Type == AssessmentType.Questionnaire && x.Questions.Any());
        }

        /// <summary>
        /// Validates that question display orders are unique
        /// </summary>
        /// <param name="questions">List of questions to validate</param>
        /// <returns>True if all display orders are unique, false otherwise</returns>
        private bool HaveUniqueDisplayOrders(List<AddAssessmentQuestionDto> questions)
        {
            if (questions == null || !questions.Any())
                return true;

            var displayOrders = questions.Select(q => q.DisplayOrder).ToList();
            return displayOrders.Count == displayOrders.Distinct().Count();
        }
    }

    /// <summary>
    /// Validation rules for AddAssessmentQuestionDto
    /// Implements business rules for individual questions
    /// </summary>
    public class AddAssessmentQuestionValidation : AbstractValidator<AddAssessmentQuestionDto>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AddAssessmentQuestionValidation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        /// <summary>
        /// Applies all validation rules for question creation
        /// </summary>
        private void ApplyValidationRules()
        {
            // Question text validation
            RuleFor(x => x.QuestionText)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.QuestionTextRequired])
                .MaximumLength(1000)
                .WithMessage("Question text must be less than 1000 characters");

            // Question type validation
            RuleFor(x => x.QuestionType)
                .IsInEnum()
                .WithMessage("Invalid question type");

            // Options validation for SingleChoice questions
            RuleFor(x => x.Options)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.QuestionOptionsRequired])
                .When(x => x.QuestionType == QuestionType.SingleChoice);

            RuleFor(x => x.Options)
                .Must(HaveAtLeastTwoOptions)
                .WithMessage("Single choice questions must have at least 2 options")
                .When(x => x.QuestionType == QuestionType.SingleChoice);

            // Display order validation
            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Display order must be greater than or equal to 0");

            // Business rule: Options should not be provided for Text type questions
            RuleFor(x => x.Options)
                .Empty()
                .WithMessage("Options should not be provided for text type questions")
                .When(x => x.QuestionType == QuestionType.Text);
        }

        /// <summary>
        /// Validates that single choice questions have at least 2 options
        /// </summary>
        /// <param name="options">List of options to validate</param>
        /// <returns>True if at least 2 options are provided, false otherwise</returns>
        private bool HaveAtLeastTwoOptions(List<string> options)
        {
            return options != null && options.Count >= 2 && options.All(o => !string.IsNullOrWhiteSpace(o));
        }
    }
}
