using System.Net;
using Abstraction.Base.Response;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Abstraction.Common.Wappers
{
    public class PaginatedResult<T> : BaseResponse<List<T>>
    {
        #region Pagination Properties
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }

        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        #endregion

        #region Constructors

        public PaginatedResult()
        {
            Successed = true;
        }
      
        public PaginatedResult(List<T> data, int totalCount, int page, int pageSize, string message = null) : base(data, message)
        {
            TotalCount = totalCount;
            CurrentPage = page;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            Successed = true;
            StatusCode = System.Net.HttpStatusCode.OK;
        }

        public PaginatedResult(string message, bool succeeded = false): base(message, succeeded)
        {
            Data = new List<T>();
        }

        #endregion

        #region Static Factory
        public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize, string message = null)
        {
            return new PaginatedResult<T>(data, count, page, pageSize, message);
        }
        public static PaginatedResult<T> ServerError(string? message = null)
        {
            return new PaginatedResult<T>()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Successed = false,
                Message = message == null ? "Internal Server Error." : message,
                
            };
        }
        public static PaginatedResult<T> EmptyCollection(string? message = "Empty Collection")
        {
            return new PaginatedResult<T>()
            {
                StatusCode = HttpStatusCode.OK,
                Message = message,
                Successed = true
            };
        }
        #endregion
    }
}
