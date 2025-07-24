using Abstraction.Contracts.Repository;
using Application.Features.Resolutions.Commands.Add;
using Domain.Entities.FundManagement;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.Shared;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Resolutions.Validation
{
    /// <summary>
    /// Validation rules for AddResolutionCommand
    /// Implements FluentValidation for comprehensive input validation
    /// Supports localization for Arabic and English error messages
    /// Based on Sprint.md requirements (JDWA-511)
    /// Uses dynamic length parameters from constants
    /// </summary>
    public class AddResolutionValidation : AbstractValidator<AddResolutionCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IRepositoryManager _repository;

        public AddResolutionValidation(IStringLocalizer<SharedResources> localizer, IRepositoryManager repository)
        {
            _localizer = localizer;
            _repository = repository;
          
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            // Include base validation rules (handles common field validations)
            Include(new BaseValidation(_localizer));

            // === ADD-SPECIFIC VALIDATIONS (not in BaseValidation) ===

            // Fund ID validation (specific to Add operation)
            RuleFor(x => x.FundId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.InvalidIdValidation]);

            // Additional resolution date validation (future date check)
            RuleFor(x => x.ResolutionDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage(_localizer[SharedResourcesKey.ResolutionDateCannotBeFuture]);

            // Business rule: Fund must exist and be active
            RuleFor(x => x.FundId)
                .MustAsync(async (fundId, cancellation) =>
                {
                    var fund = await _repository.Funds.GetByIdAsync<Fund>(fundId, trackChanges: false);
                    return fund != null;
                })
                .WithMessage(_localizer[SharedResourcesKey.FundNotFound]);

            // Business rule: Resolution date must be after fund initiation date
            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    var fund = await _repository.Funds.GetByIdAsync<Fund>(command.FundId, trackChanges: false);
                    if (fund == null) return true; // Let the fund existence validation handle this

                    return command.ResolutionDate.Date >= fund.InitiationDate.Date;
                })
                .WithMessage(_localizer[SharedResourcesKey.ResolutionDateMustBeAfterFundInitiation]);

            // Business rule: Attachment must exist if provided
            RuleFor(x => x.AttachmentId)
                .MustAsync(async (attachmentId, cancellation) =>
                {
                    if (attachmentId <= 0) return true; // Optional attachment

                    var attachment = await _repository.Attachments.GetByIdAsync<Attachment>(attachmentId, trackChanges: false);
                    return attachment != null;
                })
                .WithMessage(_localizer[SharedResourcesKey.AttachmentNotFound])
                .When(x => x.AttachmentId > 0);

            // Business rule: Attachment must be PDF if provided
            RuleFor(x => x.AttachmentId)
                .MustAsync(async (attachmentId, cancellation) =>
                {
                    if (attachmentId <= 0) return true; // Optional attachment

                    var attachment = await _repository.Attachments.GetByIdAsync<Attachment>(attachmentId, trackChanges: false);
                    if (attachment == null) return true; // Let attachment existence validation handle this

                    return attachment.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase);
                })
                .WithMessage(_localizer[SharedResourcesKey.InvalidFileType])
                .When(x => x.AttachmentId > 0);

            // Business rule: Attachment file size must not exceed 10 MB (Sprint.md requirement)
            RuleFor(x => x.AttachmentId)
                .MustAsync(async (attachmentId, cancellation) =>
                {
                    if (attachmentId <= 0) return true; // Optional attachment

                    var attachment = await _repository.Attachments.GetByIdAsync<Attachment>(attachmentId, trackChanges: false);
                    if (attachment == null) return true; // Let attachment existence validation handle this

                    // Check file size - 10 MB = 10 * 1024 * 1024 bytes
                    const long maxFileSizeBytes = 10 * 1024 * 1024;
                    return attachment.FileSize <= maxFileSizeBytes;
                })
                .WithMessage(_localizer[SharedResourcesKey.FileSizeExceedsLimit])
                .When(x => x.AttachmentId > 0);
        }
    }
}
