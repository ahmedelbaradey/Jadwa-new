using Abstraction.Base.Response;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Main.Extensions
{
    public static class ValidationErrorResponseFactory
    {
        public static IActionResult CreateResponse(ActionContext context)
        {
            var errors = context.ModelState
                .Where(p => p.Value.Errors.Count > 0)
                .SelectMany(kvp => kvp.Value.Errors.Select(e => new ValidationFailure(kvp.Key, e.ErrorMessage)))
                .ToList();

           // var errors = context.ModelState.Where(x => x.Value.Errors.Count > 0).ToDictionary(kvp => kvp.Key,kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray());
            var response = new BaseResponse<object>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Successed = false,
                Message = "Validation Failed",
                Data = errors
            };

            // Logging if needed
            var logger = context.HttpContext.RequestServices.GetService<ILoggerFactory>()?.CreateLogger("ValidationError");

            logger?.LogWarning("Validation Failed: {@Errors}", errors);

            return new UnprocessableEntityObjectResult(response);
        }

         
    }
}
