using System.Net;
using FluentValidation.Results;



namespace Abstraction.Base.Response
{
    public class BaseResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public bool Successed { get; set; }
        public string? Message { get; set; }
        public T Data { get; set; }
        public IReadOnlyList<object> Errors { get; set; }
        //public object meta

        public BaseResponse()
        {

        }

        public BaseResponse(T data, string message = null)
        {
            Successed = true;
            Data = data;
            Message = message;
            StatusCode = HttpStatusCode.OK;
        }
        public BaseResponse(string message)
        {
            Successed = false;
            Message = message;
        }

        public BaseResponse(string message, bool successed)
        {
            Message = message;
            Successed = successed;
        }

        /// <summary>
        /// Creates a successful response with data and optional message
        /// </summary>
        public static BaseResponse<T> Success(T data, string message = "Operation completed successfully")
        {
            return new BaseResponse<T>
            {
                StatusCode = HttpStatusCode.OK,
                Successed = true,
                Data = data,
                Message = message,
                Errors = new List<object>()
            };
        }

        /// <summary>
        /// Creates a failure response with error message
        /// </summary>
        public static BaseResponse<T> Failure(string message)
        {
            return new BaseResponse<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Successed = false,
                Message = message,
                Errors = new List<object> { message }
            };
        }
    }
}
