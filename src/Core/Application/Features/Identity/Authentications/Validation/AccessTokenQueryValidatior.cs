using FluentValidation;
using Application.Features.Identity.Authentications.Queries.ValidateAccessToken;

namespace Application.Features.Identity.Authentications.Validation
{
    public class AccessTokenQueryValidatior : AbstractValidator<AccessTokenQuery>
    {
        public AccessTokenQueryValidatior()
        {
            ApplyValidationsRules();
        }

        #region Functions
        public void ApplyValidationsRules()
        {

            RuleFor(x => x.Accesstoken)
                 .NotNull().WithMessage("Can't be empty.")
                 .NotEmpty().WithMessage("Can't be empty.");

        }
        #endregion
    }
}
