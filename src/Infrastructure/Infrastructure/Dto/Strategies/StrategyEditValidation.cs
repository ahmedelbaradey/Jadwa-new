using Abstraction.Contracts.Repository;
using Domain.Entities.Startegies;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;
using Shared.Behaviors;

namespace Infrastructure.Dto.Strategies
{
    public class StrategyEditValidation : AbstractValidator<StrategyEditDto>
    {
        protected IGenericRepository _repository;
        protected IStringLocalizer<SharedResources> _localizer;
        public StrategyEditValidation(IGenericRepository repository, IStringLocalizer<SharedResources> localizer)
        {
            _repository = repository;
            _localizer = localizer;
            ApplyValidationsRules();
        }
        public void ApplyValidationsRules()
        {
            RuleFor(c => c.NameAr).SetValidator(new NotEmptyAndNotNullWithMessageValidator<StrategyEditDto, string>(_localizer))
                .MaximumLength(50).WithMessage(string.Format(_localizer[SharedResourcesKey.MaxLength], 50));

            RuleFor(c => c.NameEn).SetValidator(new NotEmptyAndNotNullWithMessageValidator<StrategyEditDto, string>(_localizer))
                .MaximumLength(50).WithMessage(string.Format(_localizer[SharedResourcesKey.MaxLength], 50));


            RuleFor(c => c)
                .MustAsync(async (c, cancellation) =>
                {
                    var exists = await _repository.AnyAsync<Strategy>(x => x.Id != c.Id &&
                                                                           (x.NameAr == c.NameAr || x.NameEn == c.NameEn));
                    return !exists;
                })
                .WithMessage(_localizer[SharedResourcesKey.Unique]);
        }

    }
}
