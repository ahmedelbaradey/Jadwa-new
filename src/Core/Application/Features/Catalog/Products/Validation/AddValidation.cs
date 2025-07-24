using Application.Features.Catalog.Products.Commands.Add;
using FluentValidation;

namespace Application.Features.Catalog.Products.Validation
{
    public class AddValidation : AbstractValidator<AddProductCommand>
    {
        public AddValidation()
        {
            Include(new BaseValidation());
            RuleFor(x => x.CategoryId)
                   .NotEmpty().WithMessage("Category Id can't be empty.")
                   .NotNull().WithMessage("Category Id can't be blank.");
        }
    }
}
