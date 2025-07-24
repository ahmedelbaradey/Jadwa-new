
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Main.Extensions
{
     
     
    public class ValidationMiddleware : IAsyncActionFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationMiddleware(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check basic model state validation (from data annotations)
            if (!context.ModelState.IsValid)
            {
                var _errors = context.ModelState
                .Where(p => p.Value.Errors.Count > 0)
                .SelectMany(kvp => kvp.Value.Errors.Select(e => new ValidationFailure(kvp.Key, e.ErrorMessage)))
                .ToList();

                throw new ValidationException(_errors);

                //var errorsInModelState = context.ModelState.Where(p => p.Value.Errors.Count > 0)
                //    .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value.Errors
                //       .Select(p => p.ErrorMessage)).ToArray();
                //var list = errorsInModelState.SelectMany(error => error.Value).ToList();
                //if (list.Count != 0)
                //{
                //    var errorLines = list.Select(s => s).Distinct().Aggregate((current, next) => current + "\n" + next);
                //    throw new ValidationException(errorLines);
                //}
            }

            // Apply FluentValidation for action arguments
            var actionArguments = context.ActionArguments;
            var validationErrors = new List<string>();
            var errors = new List<ValidationFailure>();
            foreach (var argument in actionArguments)
            {
                var argumentValue = argument.Value;
                if (argumentValue == null)
                    continue;

                var argumentType = argumentValue.GetType();
                var validatorType = typeof(IValidator<>).MakeGenericType(argumentType);

                // Try to get validator for this type from DI
                var validator = _serviceProvider.GetService(validatorType) as IValidator;
                if (validator != null)
                {
                    var validationContext = new ValidationContext<object>(argumentValue);
                    var validationResult = await validator.ValidateAsync(validationContext);

                    if (!validationResult.IsValid)
                    {
                        foreach (var error in validationResult.Errors)
                        {
                            validationErrors.Add(error.ErrorMessage);
                            errors.Add(error);
                        }
                    }
                }
            }

            // If we have validation errors, throw exception
            if (validationErrors.Any())
            {
                var errorLines = validationErrors.Distinct().Aggregate((current, next) => current + "\n" + next);
                throw new ValidationException(errors);
            }
            //pipeline
            await next();
        }

    }
}
