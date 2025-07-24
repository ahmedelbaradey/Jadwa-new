using FluentValidation;
using Application.Features.Identity.Authorizations.Commands.AddRole;

namespace Application.Features.Identity.Authorizations.Validation
{
    public class AddRoleValidator : AbstractValidator<AddRoleCommand>
    {

        #region Constructors
        public AddRoleValidator()
        {
            ApplyValidationsRules();
        }
        #endregion

        #region Handle Fnctions
        public void ApplyValidationsRules()
        {
            RuleFor(x => x.roleName)
                .NotNull().WithMessage("Can't be balnk.");
        }
        #endregion
    }
}
