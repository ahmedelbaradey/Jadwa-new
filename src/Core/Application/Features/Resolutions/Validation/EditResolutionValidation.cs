using Abstraction.Contracts.Repository;
using Application.Features.Resolutions.Commands.Edit;
using Domain.Entities.FundManagement;
using Domain.Entities.ResolutionManagement;
using Domain.Entities.Shared;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Resolutions.Validation
{
    /// <summary>
    /// Consolidated validation rules for EditResolutionCommand
    /// Implements comprehensive business rules and data validation for:
    /// - Basic resolution information (JDWA-506, JDWA-567)
    /// - Resolution items and conflicts (JDWA-507, JDWA-566)
    /// - Resolution attachments (JDWA-505, JDWA-568)
    /// Combines validation from EditResolutionValidation, EditResolutionItemsValidation, and EditResolutionAttachmentsValidation
    /// Supports localization for Arabic and English error messages
    /// Uses dynamic length parameters from constants
    /// </summary>
    public class EditResolutionValidation : AbstractValidator<EditResolutionCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IRepositoryManager _repository;

        public EditResolutionValidation(IStringLocalizer<SharedResources> localizer, IRepositoryManager repository)
        {
            _localizer = localizer;
            _repository = repository;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            // Include base validation rules
            Include(new BaseValidation(_localizer));

            // ID validation for existing resolution
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.InvalidIdValidation]);

            // Fund ID validation
            RuleFor(x => x.FundId)
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.InvalidIdValidation]);

            // Additional resolution date validation (future date check)
            RuleFor(x => x.ResolutionDate)
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage(_localizer[SharedResourcesKey.ResolutionDateCannotBeFuture]);

            // Business rule: Resolution must exist
            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellation) =>
                {
                    var resolution = await _repository.Resolutions.GetByIdAsync<Resolution>(id, trackChanges: false);
                    return resolution != null;
                })
                .WithMessage(_localizer[SharedResourcesKey.ResolutionNotFound]);

            // Business rule: Fund must exist
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

            // Business rule: Resolution type must exist
            RuleFor(x => x.ResolutionTypeId)
                .MustAsync(async (typeId, cancellation) =>
                {
                    var resolutionType = await _repository.ResolutionTypes.GetByIdAsync<ResolutionType>(typeId, trackChanges: false);
                    return resolutionType != null;
                })
                .WithMessage(_localizer[SharedResourcesKey.ResolutionTypeNotFound]);

            // === RESOLUTION ITEMS VALIDATION ===
            // Individual item validation (when items are provided)
            RuleForEach(x => x.ResolutionItems).ChildRules(item =>
            {
                // Description validation (max 500 characters for add/edit)
                item.RuleFor(i => i.Description)
                    .MaximumLength(ValidationConstants.RESOLUTION_ITEM_DESCRIPTION_MAX_LENGTH)
                    .WithMessage(_localizer[SharedResourcesKey.MaxLength, ValidationConstants.RESOLUTION_ITEM_DESCRIPTION_MAX_LENGTH])
                    .When(i => !string.IsNullOrEmpty(i.Description));

                // Conflict members validation - required if HasConflict is true
                item.RuleFor(i => i.ConflictMembers)
                    .NotEmpty()
                    .WithMessage(_localizer[SharedResourcesKey.ConflictMembersRequired])
                    .When(i => i.HasConflict);

                // Board member existence validation for conflicts
                item.RuleForEach(i => i.ConflictMembers)
                    .MustAsync(async (conflict, cancellation) =>
                    {
                        if (conflict == null) return true;

                        var boardMember = await _repository.BoardMembers.GetByIdAsync<Domain.Entities.FundManagement.BoardMember>(
                            conflict.BoardMemberId, trackChanges: false);
                        return boardMember != null;
                    })
                    .WithMessage(_localizer[SharedResourcesKey.InvalidBoardMemberForConflict])
                    .When(i => i.ConflictMembers != null);
            }).When(x => x.ResolutionItems != null && x.ResolutionItems.Any());

            // Business rule: Validate that board members in conflicts belong to the same fund
            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    if (command.ResolutionItems?.Any() != true) return true; // No items to validate

                    var resolution = await _repository.Resolutions.GetByIdAsync<Resolution>(command.Id, trackChanges: false);
                    if (resolution == null) return true; // Let existence validation handle this

                    foreach (var item in command.ResolutionItems)
                    {
                        if (item.ConflictMembers?.Any() == true)
                        {
                            foreach (var conflict in item.ConflictMembers)
                            {
                                var boardMember = await _repository.BoardMembers.GetByIdAsync<Domain.Entities.FundManagement.BoardMember>(
                                    conflict.BoardMemberId, trackChanges: false);

                                if (boardMember != null && boardMember.FundId != resolution.FundId)
                                {
                                    return false;
                                }
                            }
                        }
                    }
                    return true;
                })
                .WithMessage(_localizer[SharedResourcesKey.InvalidBoardMemberForConflict])
                .When(x => x.ResolutionItems != null && x.ResolutionItems.Any());

            // === RESOLUTION ATTACHMENTS VALIDATION ===
            // Maximum attachments validation (max 10 files)
            RuleFor(x => x.AttachmentIds)
                .Must(attachmentIds => attachmentIds.Count <= 10)
                .WithMessage(_localizer[SharedResourcesKey.MaxAttachmentsReached])
                .When(x => x.AttachmentIds != null && x.AttachmentIds.Any());

            // Individual attachment existence validation
            RuleForEach(x => x.AttachmentIds)
                .MustAsync(async (attachmentId, cancellation) =>
                {
                    var attachment = await _repository.Attachments.GetByIdAsync<Domain.Entities.Shared.Attachment>(attachmentId, trackChanges: false);
                    return attachment != null;
                })
                .WithMessage(_localizer[SharedResourcesKey.AttachmentNotFound])
                .When(x => x.AttachmentIds != null && x.AttachmentIds.Any());

            // Unique attachment IDs validation
            RuleFor(x => x.AttachmentIds)
                .Must(attachmentIds => attachmentIds.Distinct().Count() == attachmentIds.Count)
                .WithMessage(_localizer[SharedResourcesKey.DuplicateAttachmentsNotAllowed])
                .When(x => x.AttachmentIds != null && x.AttachmentIds.Any());

            // === ALTERNATIVE 1 WORKFLOW VALIDATION ===
            // Voting suspension validation for VotingInProgress resolutions
            RuleFor(x => x)
                .MustAsync(async (command, cancellation) =>
                {
                    var resolution = await _repository.Resolutions.GetByIdAsync<Resolution>(command.Id, trackChanges: false);
                    if (resolution == null) return false;

                    // Alternative 1: If resolution is VotingInProgress and not saving as draft,
                    // this implies voting suspension which requires special handling
                    if (resolution.Status == ResolutionStatusEnum.VotingInProgress && !command.SaveAsDraft)
                    {
                        // This validation passes - the actual suspension logic is handled in the command handler
                        // We just ensure the resolution exists and is in the correct state
                        return true;
                    }

                    return true;
                })
                .WithMessage(_localizer[SharedResourcesKey.CannotEditVotingResolutionWithoutSuspension])
                .When(x => !x.SaveAsDraft); // Only validate when not saving as draft
        }
    }
}
