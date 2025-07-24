using Application.Features.Shared.FileManagment.Commands.MinIOPreview;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Shared.FileManagment.Validation
{
    /// <summary>
    /// Validation rules for MinIO file preview operations
    /// </summary>
    public class MinIOPreviewValidation : AbstractValidator<MinIOPreviewCommand>
    {
        public MinIOPreviewValidation(IStringLocalizer<SharedResources> localizer)
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage(localizer[SharedResourcesKey.MinIOFileIdRequired])
                .GreaterThan(0).WithMessage(localizer[SharedResourcesKey.MinIOFileIdMustBeGreaterThanZero]);

            RuleFor(x => x.ExpiryInMinutes)
                .GreaterThan(0).WithMessage(localizer[SharedResourcesKey.MinIOExpiryTimeInvalid]);

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
