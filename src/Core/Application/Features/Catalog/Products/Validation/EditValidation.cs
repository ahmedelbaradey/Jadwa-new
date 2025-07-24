using Application.Common.Configurations;
using Application.Features.Catalog.Products.Commands.Edit;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Application.Features.Catalog.Products.Validation
{
    public class EditValidation : AbstractValidator<EditProductCommand>
    {
        private readonly IOptionsMonitor<AttachmentConfiguration> _configuration;
        private readonly AttachmentConfiguration _pdfConfiguration;
        public EditValidation(IOptionsMonitor<AttachmentConfiguration> configuration)
        {
            _configuration = configuration;
            _pdfConfiguration = _configuration.CurrentValue;
            Include(new BaseValidation());
            RuleFor(x => x.CategoryId)
                   .NotEmpty().WithMessage("Category Id can't be empty.")
                   .NotNull().WithMessage("Category Id can't be blank.");
        }
    }
}
