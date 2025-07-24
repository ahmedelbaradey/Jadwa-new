using Application.Features.Catalog.Categories.Commands.Add;


using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Catalog.Categories.Validation
{
    public class AddValidation : AbstractValidator<AddCategoryCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;  
        public AddValidation(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            Include(new BaseValidation(_localizer));
        }
    }
}
