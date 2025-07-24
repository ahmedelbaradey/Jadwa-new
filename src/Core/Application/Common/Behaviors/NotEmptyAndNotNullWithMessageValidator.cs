using FluentValidation.Validators;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Resources;

namespace Shared.Behaviors
{
    public class NotEmptyAndNotNullWithMessageValidator<T, TProperty> : PropertyValidator<T, TProperty>
    {
        protected IStringLocalizer<SharedResources> _localizer;
        public NotEmptyAndNotNullWithMessageValidator(IStringLocalizer<SharedResources> localizer)
        {
            _localizer = localizer;
        }
        public override string Name => "NotEmptyAndNotNullWithMessageValidator";
        public override bool IsValid(ValidationContext<T> context, TProperty value)
        {
            if (value is string stringValue)
            {
                return !string.IsNullOrEmpty(stringValue);
            }
            else if (value is int intValue)
            {
                return intValue != 0;
            }
            else if (value is double doubleValue)
            {
                return doubleValue != 0.0;
            }
            return value != null;
        }

        protected override string GetDefaultMessageTemplate(string errorCode)
          => _localizer[SharedResourcesKey.Required];
    }
}
