using Application.Features.Funds.Dtos;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;
using Shared.Behaviors;

namespace Application.Features.Funds.Validation
{
    public class BaseValidation : AbstractValidator<BaseFundDto>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public BaseValidation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }
        public void ApplyValidationsRules()
        {

            RuleFor(c => c.Name).SetValidator(new NotEmptyAndNotNullWithMessageValidator<BaseFundDto, string>(_localizer));
            RuleFor(x => x.InitiationDate)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKey.RequiredField])
                .NotNull().WithMessage(_localizer[SharedResourcesKey.RequiredField])
                .Must(date => date.Date >= new DateTime(2010, 1, 1).Date && date.Date <= DateTime.Today.Date)
                .WithMessage(_localizer[SharedResourcesKey.InitiationDateRangeValidator]);
        }

    }
}
