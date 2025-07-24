using FluentValidation;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using Abstraction.Base.Response;
using Abstraction.Contracts.Logger;

namespace Application.Common.MiddleWare
{
    public class ErrorHandlerMiddleWare
    {
        private readonly ILoggerManager _logger;
        private readonly RequestDelegate _next;

        public ErrorHandlerMiddleWare(RequestDelegate next, ILoggerManager logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                await HandleExceptionAsync(context, e);
            }
        }

        public async Task HandleExceptionAsync(HttpContext context, Exception error)
        {

            var response = context.Response;
            response.ContentType = "application/json";
            var responseModel = new BaseResponse<string>() { Successed = false, Message = error?.Message };

            //TODO:: cover all validation errors
            switch (error)
            {
                case UnauthorizedAccessException e:
                    // custom application error
                    responseModel.Message = error.Message;
                    responseModel.StatusCode = HttpStatusCode.Unauthorized;
                    response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    break;

                case ValidationException e:
                    // custom validation error
                    responseModel.Message = "Validation Failed";
                    responseModel.StatusCode = HttpStatusCode.UnprocessableEntity;
                    response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    responseModel.Errors =  GetErrors(error);

                    break;
                case KeyNotFoundException e:
                    // not found error
                    responseModel.Message = error.Message; ;
                    responseModel.StatusCode = HttpStatusCode.NotFound;
                    response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                case Exception e:
                    if (e.GetType().ToString() == "ApiException")
                    {
                        responseModel.Message += e.Message;
                        responseModel.Message += e.InnerException == null ? "" : "\n" + e.InnerException.Message;
                        responseModel.StatusCode = HttpStatusCode.BadRequest;
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                    }
                    responseModel.Message = error.Message;
                    responseModel.Message += e.InnerException == null ? "" : "\n" + e.InnerException.Message;

                    responseModel.StatusCode = HttpStatusCode.InternalServerError;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;

                default:
                    // unhandled error
                    responseModel.Message = error?.Message;
                    responseModel.StatusCode = HttpStatusCode.InternalServerError;
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonSerializer.Serialize(responseModel);
            await response.WriteAsync(result);

        }
        private static IReadOnlyList<object> GetErrors(Exception exception)
        {
            if (exception is ValidationException validationException)
            {
                var errors = 
                  validationException.Errors
                    .Select(e => new
                    {
                        e.PropertyName,
                        e.ErrorMessage
                       
                    })
                    .ToList();
                return errors;
            }
            return null;
        }
        //private static IReadOnlyList<string> GetErrors(Exception exception)
        //{
        //    IReadOnlyList<string> errors = null;
        //    if (exception is ValidationException validationException)
        //    {
        //        errors = validationException.Errors.DistinctBy(C=>C.ErrorMessage).Select(C => C.ErrorMessage).ToList();
        //    }
        //    return errors;
        //}
    }
}
