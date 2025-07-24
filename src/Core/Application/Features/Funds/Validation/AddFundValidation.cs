using Abstraction.Contracts.Repository;
using Application.Features.Funds.Commands.Add;
using Domain.Entities.FundManagement;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Funds.Validation
{
    public class AddFundValidation : AbstractValidator<AddFundCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public AddFundValidation(IGenericRepository repository, IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            Include(new BaseValidation(_localizer));
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKey.RequiredField])
               .NotNull().WithMessage(_localizer[SharedResourcesKey.RequiredField])
               // don't accept numbers or decimal points in the name
               .Must(name =>
               {
                   if (string.IsNullOrWhiteSpace(name))
                       return true; // Skip check if empty, already handled by NotEmpty
                   // Reject if contains any decimal point
                   return !name.Any(c => char.IsNumber(c) || c == '.');
               })
               .WithMessage(_localizer[SharedResourcesKey.InvalidFundName])
               .MustAsync(async (name, cancellation) =>
               {
                   if (string.IsNullOrWhiteSpace(name))
                       return true; // Skip check if empty, already handled by NotEmpty

                   return !await repository.AnyAsync<Fund>(s => s.Name == name);
               })
               .WithMessage(_localizer[SharedResourcesKey.FundAlreadyExist]);

            RuleFor(x => x.AttachmentId)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKey.RequiredField])
               .NotNull().WithMessage(_localizer[SharedResourcesKey.RequiredField]);

            RuleFor(x => x.PropertiesNumber)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKey.RequiredField])
               .NotNull().WithMessage(_localizer[SharedResourcesKey.RequiredField])
               .Must(value => value >= 1).WithMessage(_localizer[SharedResourcesKey.PropertiesNumberValidator]);

            RuleFor(x => x.VotingTypeId)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKey.RequiredField])
               .NotNull().WithMessage(_localizer[SharedResourcesKey.RequiredField])
               .Must(value => value >= 1 && value <= 2).WithMessage(_localizer[SharedResourcesKey.VotingTypeRangeValidator]);


            RuleFor(x => x.StrategyId)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKey.RequiredField])
               .NotNull().WithMessage(_localizer[SharedResourcesKey.RequiredField]);

            RuleFor(x => x.FundManagers)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKey.RequiredField])
               .NotNull().WithMessage(_localizer[SharedResourcesKey.RequiredField])
               .Must(x => x.Count >= 1 && x.Count <= 3).WithMessage(_localizer[SharedResourcesKey.FundManagersListValidation]);

            When(x => x.FundBoardSecretaries != null && x.FundBoardSecretaries.Count() > 0, () =>
            {
                RuleFor(x => x.FundBoardSecretaries)
                    .Must(list => list!.Count >= 1 && list.Count <= 4)
                    .WithMessage(_localizer[SharedResourcesKey.FundBoardSecretariesListValidation]);
            });

            RuleFor(x => x.LegalCouncilId)
               .NotEmpty().WithMessage(_localizer[SharedResourcesKey.RequiredField])
               .NotNull().WithMessage(_localizer[SharedResourcesKey.RequiredField]);

            RuleFor(x => x.OldCode)
            .Must(oldCode =>
            {
                if (string.IsNullOrWhiteSpace(oldCode))
                    return true; // Skip check if empty, already handled by NotEmpty
                                 // Reject if contains any decimal point
                return !oldCode.Any(c => c == '.');
            })
            .WithMessage(_localizer[SharedResourcesKey.InvalidFund])
            .MustAsync(async (oldCode, cancellation) =>
            {
                if (string.IsNullOrWhiteSpace(oldCode))
                    return true; // Skip check if empty, already handled by NotEmpty

                return !await repository.AnyAsync<Fund>(s => s.OldCode == oldCode);
            })
            .WithMessage(_localizer[SharedResourcesKey.FundAlreadyExist]);

        }
    }
}
