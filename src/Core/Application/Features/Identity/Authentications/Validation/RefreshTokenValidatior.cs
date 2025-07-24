using FluentValidation;
using Application.Features.Identity.Authentications.Commands.RefreshToken;

namespace Application.Features.Identity.Authentications.Validation
{
    public class RefreshTokenValidatior : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenValidatior()
        {
            ApplyValidationsRules();
        }

        #region Functions
        public void ApplyValidationsRules()
        {

            RuleFor(x => x.AccessToken)
                 .NotNull().WithMessage("Can't be empty.")
                 .NotEmpty().WithMessage("Can't be empty.");

            RuleFor(x => x.RefreshToken)
                 .NotNull().WithMessage("Can't be empty.")
                 .NotEmpty().WithMessage("Can't be empty.");
        }
        #endregion
    }
}
