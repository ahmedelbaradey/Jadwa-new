using Application.Features.MeetingTimeProposals.Commands.Vote;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.MeetingTimeProposals.Validation
{
    /// <summary>
    /// Validator for SubmitMeetingTimeVoteCommand
    /// Implements validation rules based on User Story 2 requirements
    /// Ensures voting business rules are enforced with localized error messages
    /// </summary>
    public class SubmitMeetingTimeVoteValidator : AbstractValidator<SubmitMeetingTimeVoteCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public SubmitMeetingTimeVoteValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            // Proposal ID validation
            RuleFor(x => x.ProposalId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.RequiredField]);

            // Selected proposed date IDs validation - at least one selection required
            RuleFor(x => x.SelectedProposedDateIds)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.SelectAtLeastOneOption])
                .Must(ids => ids.Count > 0)
                .WithMessage(_localizer[SharedResourcesKey.SelectAtLeastOneOption]);

            // Individual proposed date ID validation
            RuleForEach(x => x.SelectedProposedDateIds)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.InvalidValue]);

            // Ensure no duplicate selections
            RuleFor(x => x.SelectedProposedDateIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage(_localizer[SharedResourcesKey.DuplicateSelection])
                .When(x => x.SelectedProposedDateIds.Any());
        }
    }
}
