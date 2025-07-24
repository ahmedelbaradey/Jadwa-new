using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Identity.Users.Commands.UpdateUserProfile
{
    /// <summary>
    /// Validator for UpdateUserProfileCommand
    /// Implements Sprint 3 validation rules with localization
    /// </summary>
    public class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public UpdateUserProfileCommandValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            // Email validation
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.ProfileRequiredField])
                .EmailAddress()
                .WithMessage(_localizer[SharedResourcesKey.ProfileInvalidEmailFormat])
                .MaximumLength(255)
                .WithMessage(_localizer[SharedResourcesKey.MaxLength, 255]);

          

            // IBAN validation (optional)
            RuleFor(x => x.IBAN)
                .MaximumLength(34)
                .WithMessage(_localizer[SharedResourcesKey.MaxLength, 34])
                .Must(BeValidIBAN)
                .WithMessage(_localizer[SharedResourcesKey.InvalidIBANFormat])
                .When(x => !string.IsNullOrWhiteSpace(x.IBAN));

            // Nationality validation
            RuleFor(x => x.Nationality)
                .MaximumLength(100)
                .WithMessage(_localizer[SharedResourcesKey.MaxLength, 100])
                .When(x => !string.IsNullOrWhiteSpace(x.Nationality));

            // Passport number validation
            RuleFor(x => x.PassportNo)
                .MaximumLength(20)
                .WithMessage(_localizer[SharedResourcesKey.MaxLength, 20])
                .Matches(@"^[A-Za-z0-9]+$")
                .WithMessage(_localizer[SharedResourcesKey.PassportNumberAlphanumeric])
                .When(x => !string.IsNullOrWhiteSpace(x.PassportNo));

            // CV file validation
            RuleFor(x => x.CVFile)
                .Must(BeValidCVFile)
                .WithMessage(_localizer[SharedResourcesKey.ProfileInvalidCVFile])
                .When(x => x.CVFile != null);

            // Personal photo validation
            RuleFor(x => x.PersonalPhoto)
                .Must(BeValidPhotoFile)
                .WithMessage(_localizer[SharedResourcesKey.ProfileInvalidPhotoFile])
                .When(x => x.PersonalPhoto != null);
                
         
        }

        private bool BeValidIBAN(string? iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return true;

            // Basic IBAN validation (simplified)
            iban = iban.Replace(" ", "").ToUpperInvariant();
            return iban.Length >= 15 && iban.Length <= 34 && iban.All(c => char.IsLetterOrDigit(c));
        }

        private bool BeValidCVFile(IFormFile? file)
        {
            if (file == null)
                return true;

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var maxSizeInBytes = 10 * 1024 * 1024; // 10MB

            return allowedExtensions.Contains(extension) && file.Length <= maxSizeInBytes;
        }

        private bool BeValidPhotoFile(IFormFile? file)
        {
            if (file == null)
                return true;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var maxSizeInBytes = 2 * 1024 * 1024; // 2MB

            return allowedExtensions.Contains(extension) && file.Length <= maxSizeInBytes;
        }
    }
}
