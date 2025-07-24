using Application.Features.Catalog.Products.Dtos;
using FluentValidation;

namespace Application.Features.Catalog.Products.Validation
{
    public class BaseValidation : AbstractValidator<ProductDto>
    {
        public BaseValidation()
        {
            ApplyValidationsRules();
        }
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Can't be empty.")
                .NotNull().WithMessage("Can't be blank.");

            RuleFor(x => x.Price)
                .NotEmpty().WithMessage("Can't be empty.")
                .NotNull().WithMessage("Can't be blank.");
        }
    }
}
