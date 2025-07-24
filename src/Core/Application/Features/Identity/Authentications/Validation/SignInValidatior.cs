using FluentValidation;
using Application.Features.Identity.Authentications.Commands.SignIn;
using Microsoft.Extensions.Localization;
using Resources;

namespace Application.Features.Identity.Authentications.Validation
{
    public class SignInValidatior : AbstractValidator<SignInCommand>
    {
        private readonly IStringLocalizer<SharedResources> _localizer;

        public SignInValidatior(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
            ApplyValidationsRules();
        }

        #region Functions
        public void ApplyValidationsRules()
        {

            RuleFor(x => x.UserName)
                 .NotEmpty().WithMessage(_localizer[SharedResourcesKey.LoginUsernameRequired])
                 .NotNull().WithMessage(_localizer[SharedResourcesKey.LoginUsernameRequired]);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(_localizer[SharedResourcesKey.LoginPasswordRequired])
                .NotNull().WithMessage(_localizer[SharedResourcesKey.LoginPasswordRequired]);
        }
        #endregion
    }
}
