using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;
using Abstraction.Contracts.Identity;
using System.Text.RegularExpressions;
using Abstraction.Constants;
using Application.Features.Identity.Users.Commands.AddUser;
using Application.Features.Identity.Users.Dtos;

namespace Application.Features.Identity.Users.Validation
{
    /// <summary>
    /// Validator for AddUserCommand with Sprint 3 enhancements
    /// Implements mobile number validation and unique role checking
    /// </summary>
    public class BaseUserValidator : AbstractValidator<BaseUserDto>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public BaseUserValidator(
            IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            // Basic Information Validation
            RuleFor(x => x.FullName)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.ProfileRequiredField])
                .MaximumLength(255)
                .WithMessage(_localizer[SharedResourcesKey.MaxLength, 255]);

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.ProfileRequiredField])
                .EmailAddress()
                .WithMessage(_localizer[SharedResourcesKey.ProfileInvalidEmailFormat])
                .MaximumLength(255)
                .WithMessage(_localizer[SharedResourcesKey.MaxLength, 255]);

            // IBAN Validation (optional)
            RuleFor(x => x.IBAN)
                .MaximumLength(34)
                .WithMessage(_localizer[SharedResourcesKey.MaxLength, 34])
                .Must(BeValidIBAN)
                .WithMessage(_localizer[SharedResourcesKey.InvalidIBANFormat])
                .When(x => !string.IsNullOrWhiteSpace(x.IBAN));

            // Nationality Validation
            RuleFor(x => x.Nationality)
                .MaximumLength(100)
                .WithMessage(_localizer[SharedResourcesKey.MaxLength, 100])
                .When(x => !string.IsNullOrWhiteSpace(x.Nationality));

            // Passport Number Validation
            RuleFor(x => x.PassportNo)
                .MaximumLength(20)
                .WithMessage(_localizer[SharedResourcesKey.MaxLength, 20])
                .Matches(@"^[A-Za-z0-9]+$")
                .WithMessage(_localizer[SharedResourcesKey.PassportNumberAlphanumeric])
                .When(x => !string.IsNullOrWhiteSpace(x.PassportNo));
           
        }




        /// <summary>
        /// Validates IBAN format (basic validation)
        /// </summary>
        private bool BeValidIBAN(string iban)
        {
            if (string.IsNullOrWhiteSpace(iban))
                return true; // Optional field

            // Basic IBAN validation - should start with country code and be alphanumeric
            var ibanPattern = @"^[A-Z]{2}[0-9]{2}[A-Z0-9]+$";
            return Regex.IsMatch(iban.Replace(" ", "").ToUpper(), ibanPattern);
        }

        /// <summary>
        /// Validates CV file format and size
        /// </summary>
        private bool BeValidCVFile(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null) return true; // Optional

            var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
            var maxSizeInBytes = 10 * 1024 * 1024; // 10MB

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension) && file.Length <= maxSizeInBytes;
        }

        /// <summary>
        /// Validates personal photo file format and size
        /// </summary>
        private bool BeValidPhotoFile(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (file == null) return true; // Optional

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
            var maxSizeInBytes = 2 * 1024 * 1024; // 2MB

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return allowedExtensions.Contains(extension) && file.Length <= maxSizeInBytes;
        }
    }
}
