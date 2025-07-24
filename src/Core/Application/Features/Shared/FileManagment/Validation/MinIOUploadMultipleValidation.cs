using Application.Features.Shared.FileManagment.Commands.MinIOUploadMultiple;
using Application.Common.Configurations;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Resources;

namespace Application.Features.Shared.FileManagment.Validation
{
    /// <summary>
    /// Validation rules for MinIO multiple file upload operations
    /// </summary>
    public class MinIOUploadMultipleValidation : AbstractValidator<MinIOUploadMultipleCommand>
    {
        public MinIOUploadMultipleValidation(IStringLocalizer<SharedResources> localizer, IOptionsMonitor<MinIOConfiguration> configuration)
        {
            RuleFor(x => x.Files)
                .NotNull().WithMessage(localizer[SharedResourcesKey.Required])
                .NotEmpty().WithMessage(localizer[SharedResourcesKey.MinIONoFilesProvided])
                .Must(files => files.Count <= 50).WithMessage(localizer[SharedResourcesKey.MinIOMaxFilesExceeded, 50]);

            RuleFor(x => x.ModuleId)
                .GreaterThan(0).WithMessage(localizer[SharedResourcesKey.MinIOModuleIdMustBeGreaterThanZero]);

            RuleFor(x => x.MaxFileCount)
                .GreaterThan(0).WithMessage(localizer[SharedResourcesKey.MinIOModuleIdMustBeGreaterThanZero])
                .LessThanOrEqualTo(100).WithMessage(localizer[SharedResourcesKey.MinIOMaxFilesExceeded, 100]);
          
            RuleFor(x => x.FileNames)
                .Must((command, fileNames) => fileNames == null || fileNames.Count == command.Files?.Count)
                .WithMessage(localizer[SharedResourcesKey.MinIOFileNameCountMismatch]);

            RuleForEach(x => x.Files)
                .Must(file => file != null && file.Length > 0)
                .WithMessage(localizer[SharedResourcesKey.MinIOFileNullOrEmpty])
                .Must(file => file == null || file.Length <= configuration.CurrentValue.MaxFileSize)
                .WithMessage(localizer[SharedResourcesKey.MinIOFileSizeExceedsLimit, configuration.CurrentValue.MaxFileSize]);

            When(x => x.FileNames != null, () => {
                RuleForEach(x => x.FileNames)
                    .NotEmpty().WithMessage("File name cannot be empty")
                    .MaximumLength(255).WithMessage("File name cannot exceed 255 characters");
            });
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
