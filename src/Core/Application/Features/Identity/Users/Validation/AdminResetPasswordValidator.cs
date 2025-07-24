using Application.Features.Identity.Users.Commands.AdminResetPassword;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Identity.Users.Validation
{
    /// <summary>
    /// Validator for AdminResetPasswordCommand
    /// Implements Sprint 3 validation rules with localization
    /// </summary>
    public class AdminResetPasswordValidator : AbstractValidator<AdminResetPasswordCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public AdminResetPasswordValidator(IStringLocalizer<SharedResources> localizer)
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
