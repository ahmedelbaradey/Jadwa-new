using Application.Features.Shared.FileManagment.Commands.MinIOUpload;
using Application.Common.Configurations;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Resources;

namespace Application.Features.Shared.FileManagment.Validation
{
    /// <summary>
    /// Validation rules for MinIO file upload operations
    /// </summary>
    public class MinIOUploadValidation : AbstractValidator<MinIOUploadCommand>
    {
        public MinIOUploadValidation(IStringLocalizer<SharedResources> localizer, IOptionsMonitor<MinIOConfiguration> configuration)
        {
            RuleFor(x => x.FileName)
                .NotEmpty().WithMessage(localizer[SharedResourcesKey.EmptyNameValidation])
                .NotNull().WithMessage(localizer[SharedResourcesKey.EmptyNameValidation]);

            RuleFor(x => x.File)
            .NotNull().WithMessage(localizer[SharedResourcesKey.Required])
            .Must(file => file.Length <= configuration.CurrentValue.MaxFileSize)
            .WithMessage(localizer[SharedResourcesKey.MaxFileSize, 10]);

            RuleFor(x => x.ModuleId)
                .GreaterThan(0).WithMessage(localizer[SharedResourcesKey.MinIOModuleIdMustBeGreaterThanZero]);

            RuleFor(x => x.BucketName)
                .NotEmpty()
                .WithMessage(localizer[SharedResourcesKey.MinIOInvalidBucketName]);
        }

        /// <summary>
        /// Validates MinIO bucket name according to AWS S3 naming conventions
        /// </summary>
        private static bool IsValidBucketName(string bucketName)
        {
            if (string.IsNullOrEmpty(bucketName) || bucketName.Length < 3 || bucketName.Length > 63)
                return false;

            // Must start and end with lowercase letter or number
            if (!char.IsLetterOrDigit(bucketName[0]) || !char.IsLetterOrDigit(bucketName[^1]))
                return false;

            // Can only contain lowercase letters, numbers, and hyphens
            foreach (char c in bucketName)
            {
                if (!char.IsLower(c) && !char.IsDigit(c) && c != '-')
                    return false;
            }

            // Cannot contain consecutive hyphens
            if (bucketName.Contains("--"))
                return false;

            return true;
        }
    }
}
