using Abstraction.Contracts.Repository;
using Domain.Entities.Startegies;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;
using Shared.Behaviors;

namespace Infrastructure.Dto.Strategies
{
    public class StrategyValidation : AbstractValidator<StrategyDto>
    {
        protected IGenericRepository _repository;
        protected IStringLocalizer<SharedResources> _localizer;
        public StrategyValidation(IGenericRepository repository,IStringLocalizer<SharedResources> localizer)
        {
            _repository = repository;
            _localizer = localizer;
            ApplyValidationsRules();
        }
        public void ApplyValidationsRules()
        {
          
            RuleFor(c => c.NameAr).SetValidator(new NotEmptyAndNotNullWithMessageValidator<StrategyDto, string>(_localizer))
                .MaximumLength(50).WithMessage(string.Format(_localizer[SharedResourcesKey.MaxLength], 50))
                .MustAsync(async (nameAr, cancellation) =>
                {
                    var exists = await _repository.AnyAsync<Strategy>(s => s.NameAr == nameAr);
                    return !exists;
                })
                .WithMessage(_localizer[SharedResourcesKey.Unique]);

            RuleFor(c => c.NameEn).SetValidator(new NotEmptyAndNotNullWithMessageValidator<StrategyDto, string>(_localizer))
                .MaximumLength(50).WithMessage(string.Format(_localizer[SharedResourcesKey.MaxLength], 50))
                .MustAsync(async (nameEn, cancellation) =>
                {
                    var exists = await _repository.AnyAsync<Strategy>(s => s.NameEn == nameEn);
                    return !exists;
                })
                .WithMessage(_localizer[SharedResourcesKey.Unique]);
        } 

    }
}
