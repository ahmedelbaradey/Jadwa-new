using Application.Features.Resolutions.Commands.Confirm;
using Abstraction.Contracts.Repository;
using Domain.Entities.ResolutionManagement;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Resolutions.Validation
{
    /// <summary>
    /// Validation rules for ConfirmResolutionCommand
    /// Implements comprehensive business rules and data validation
    /// Based on Sprint.md requirements (JDWA-570)
    /// </summary>
    public class ConfirmResolutionValidation : AbstractValidator<ConfirmResolutionCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        private readonly IRepositoryManager _repository;

        public ConfirmResolutionValidation(IStringLocalizer<SharedResources> localizer, IRepositoryManager repository)
        {
            _localizer = localizer;
            _repository = repository;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            // Resolution ID validation
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage(_localizer[SharedResourcesKey.EmptyIdValidation])
                .GreaterThan(0)
                .WithMessage(_localizer[SharedResourcesKey.EmptyIdValidation]);

            // Resolution existence validation
            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellation) =>
                {
                    var resolution = await _repository.Resolutions.GetByIdAsync<Resolution>(id, trackChanges: false);
                    return resolution != null;
                })
                .WithMessage(_localizer[SharedResourcesKey.ResolutionNotFound]);

            // Resolution status validation - must be WaitingForConfirmation
            RuleFor(x => x.Id)
                .MustAsync(async (id, cancellation) =>
                {
                    var resolution = await _repository.Resolutions.GetByIdAsync<Resolution>(id, trackChanges: false);
                    if (resolution == null) return true; // Let existence validation handle this

                    return resolution.Status == ResolutionStatusEnum.WaitingForConfirmation;
                })
                .WithMessage(_localizer[SharedResourcesKey.InvalidResolutionStatusForConfirmation]);
        }
    }
}
