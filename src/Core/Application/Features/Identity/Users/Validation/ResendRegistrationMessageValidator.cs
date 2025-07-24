using Application.Features.Identity.Users.Commands.ResendRegistrationMessage;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Identity.Users.Validation
{
    /// <summary>
    /// Validator for ResendRegistrationMessageCommand
    /// Implements Sprint 3 validation rules with localization
    /// </summary>
    public class ResendRegistrationMessageValidator : AbstractValidator<ResendRegistrationMessageCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public ResendRegistrationMessageValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            // User ID validation
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.ProfileRequiredField]);
        }
    }
}
