using Application.Features.MeetingTimeProposals.Commands.Create;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.MeetingTimeProposals.Validation
{
    /// <summary>
    /// Validator for CreateMeetingTimeProposalCommand
    /// Implements validation rules based on User Story 1 requirements
    /// Ensures all business rules are enforced with localized error messages
    /// </summary>
    public class CreateMeetingTimeProposalValidator : AbstractValidator<CreateMeetingTimeProposalCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public CreateMeetingTimeProposalValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            // Fund ID validation
            RuleFor(x => x.FundId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.RequiredField]);

            // Subject validation - required field, max 255 characters
            RuleFor(x => x.Subject)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.MeetingSubjectRequired])
                .NotNull()
                .WithMessage(_localizer[SharedResourcesKey.MeetingSubjectRequired])
                .MaximumLength(255)
                .WithMessage(_localizer[SharedResourcesKey.MaximumLength, 255]);

            // Description validation - optional, max 1000 characters
            RuleFor(x => x.Description)
                .MaximumLength(1000)
                .WithMessage(_localizer[SharedResourcesKey.MaximumLength, 1000])
                .When(x => !string.IsNullOrEmpty(x.Description));

            // Proposed dates validation - at least 1, maximum 4
            RuleFor(x => x.ProposedDates)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.AtLeastOneProposedTimeRequired])
                .Must(dates => dates.Count >= 1 && dates.Count <= 4)
                .WithMessage(_localizer[SharedResourcesKey.AtLeastOneProposedTimeRequired]);

            // Individual proposed date validation
            RuleForEach(x => x.ProposedDates)
                .Must(date => date.ProposedDateTime > DateTime.Now)
                .WithMessage(_localizer[SharedResourcesKey.FutureDateRequired]);

            // Attachment ID validation - optional, but if provided must be positive
            RuleFor(x => x.AttachmentId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.InvalidValue])
                .When(x => x.AttachmentId.HasValue);
        }
    }
}
