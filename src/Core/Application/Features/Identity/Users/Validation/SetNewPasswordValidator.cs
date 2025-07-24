using FluentValidation;
using Application.Features.Identity.Users.Commands.ChangePassword;
using Application.Features.Identity.Users.Commands.SetNewPassword;
using Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Identity.Users.Validation
{
    /// <summary>
    /// Validator for ChangePasswordCommand
    /// Enhanced for Sprint 3 with conditional validation rules
    /// </summary>
    public class SetNewPasswordValidator : AbstractValidator<SetNewPasswordCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly UserManager<User> _userManager;

        public SetNewPasswordValidator(IStringLocalizer<SharedResources> localizer,UserManager<User> userManager)
        {
            _localizer = localizer;
            _userManager = userManager;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {

            

            // New password validation
            RuleFor(x => x.NewPassword)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.ProfileRequiredField])
                .MinimumLength(8)
                .WithMessage(_localizer[SharedResourcesKey.PasswordMinimumLength])
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
                .WithMessage(_localizer[SharedResourcesKey.PasswordComplexityError]);


            // Confirm password validation
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.ProfileRequiredField])
                .Equal(x => x.NewPassword)
                .WithMessage(_localizer[SharedResourcesKey.PasswordMismatch]);

            // Custom validation: New password should not be same as current
            RuleFor(x => x)
                .MustAsync(async (command, cancellationToken) => await NewPasswordDifferentFromCurrent(command))
                .WithMessage(_localizer[SharedResourcesKey.PasswordSameAsCurrent])
                .When(x => !string.IsNullOrEmpty(x.NewPassword) && !string.IsNullOrEmpty(x.NewPassword));
        }

         

        private async Task<bool> NewPasswordDifferentFromCurrent(SetNewPasswordCommand command)
        {
            try
            {
                if (string.IsNullOrEmpty(command.NewPassword) || string.IsNullOrEmpty(command.NewPassword))
                    return true;

                var user = await _userManager.FindByIdAsync(command.UserId.ToString());
                if (user == null)
                    return true;

                // Check if the new password is the same as current password
                var passwordVerificationResult = _userManager.PasswordHasher.VerifyHashedPassword(
                    user, user.PasswordHash!, command.NewPassword);

                return passwordVerificationResult == PasswordVerificationResult.Failed;
            }
            catch
            {
                return true; // Allow if we can't verify
            }
        }
    }
}
