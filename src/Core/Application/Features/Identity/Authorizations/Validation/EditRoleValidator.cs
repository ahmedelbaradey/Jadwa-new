using FluentValidation;
using Application.Features.Identity.Authorizations.Commands.EditRole;

namespace Application.Features.Identity.Authorizations.Validation
{
    public class EditRoleValidator : AbstractValidator<EditRoleCommand>
    {
        #region Constructors
        public EditRoleValidator()
        {
            ApplyValidationsRules();
        }
        #endregion

        #region Handle Fnctions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.Id)
                .NotNull().WithMessage("Can't be balnk.");

            RuleFor(x => x.roleName)
                .NotNull().WithMessage("Can't be balnk.");
        }
        #endregion
    }
}
