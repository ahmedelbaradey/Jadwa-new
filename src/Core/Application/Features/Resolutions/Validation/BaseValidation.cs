using Application.Features.Resolutions.Dtos;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Resolutions.Validation
{
    /// <summary>
    /// Base validation rules for ResolutionDto
    /// Contains common validation rules shared between Add and Edit operations
    /// Based on Sprint.md field validation requirements
    /// Uses localized messages with dynamic length parameters
    /// </summary>
    public class BaseValidation : AbstractValidator<ResolutionDto>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public BaseValidation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }

        public void ApplyValidationsRules()
        {
            // Common validation rules that apply to both Add and Edit operations
            // These are basic field-level validations without business logic

            // Resolution date is required and cannot be empty
            RuleFor(x => x.ResolutionDate)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.Required]);

            // Resolution type ID must be greater than 0
            RuleFor(x => x.ResolutionTypeId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.Required]);

            // Description maximum length validation with dynamic length parameter
            RuleFor(x => x.Description)
                .MaximumLength(ValidationConstants.DESCRIPTION_MAX_LENGTH)
                .WithMessage(_localizer[SharedResourcesKey.MaxLength, ValidationConstants.DESCRIPTION_MAX_LENGTH])
                .When(x => !string.IsNullOrEmpty(x.Description));

            // NewType validation when ResolutionTypeId is "Other" (10)
            RuleFor(x => x.NewType)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.NewTypeRequiredForOtherResolutionType])
                .MaximumLength(ValidationConstants.NEW_TYPE_MAX_LENGTH)
                .WithMessage(_localizer[SharedResourcesKey.MaxLength, ValidationConstants.NEW_TYPE_MAX_LENGTH])
                .When(x => x.ResolutionTypeId == 10);

            // Voting type validation
            RuleFor(x => x.VotingType)
                .IsInEnum()
                .WithMessage(_localizer[SharedResourcesKey.InvalidVotingMethodology]);

            // Member voting result validation
            RuleFor(x => x.MemberVotingResult)
                .IsInEnum()
                .WithMessage(_localizer[SharedResourcesKey.InvalidVotingMethodology]);
        }
    }
}
