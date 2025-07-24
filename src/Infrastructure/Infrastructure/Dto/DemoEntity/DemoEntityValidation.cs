using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Catalog.Categories.Dtos;
using FluentValidation;

namespace Infrastructure.Dto.DemoEntity
{
    public class DemoEntityValidation : AbstractValidator<DemoEntityDto>
    {
        public DemoEntityValidation()
        {
            ApplyValidationsRules();
        }
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Can't be empty.")
                .NotNull().WithMessage("Can't be blank.");
        }

    }
}
