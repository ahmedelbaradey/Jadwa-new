using System;
using Application.Features.Catalog.Categories.Dtos;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Catalog.Categories.Validation
{
    public class BaseValidation : AbstractValidator<CategoryDto>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;
        public BaseValidation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Id)
               .NotNull().WithMessage(_localizer[SharedResourcesKey.EmptyIdValidation]);
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKey.EmptyNameValidation])
                .NotNull().WithMessage(_localizer[SharedResourcesKey.EmptyNameValidation]);
        }

    }
}
