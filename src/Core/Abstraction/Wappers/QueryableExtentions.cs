
using Abstraction.Common.Extensions;

namespace Abstraction.Common.Wappers
{
    public static class QueryableExtentions
    {
        public static async Task<PaginatedResult<T>> ToPaginatedListAsync<T>(this IQueryable<T> source,int pageNumber, int pageSize, string? orderBy) where T : class
        {
            if (source == null)
                throw new Exception("Source is empty.");

            pageNumber = pageNumber == 0 ? 1 : pageNumber;
            pageSize = pageSize == 0 ? 200 : pageSize;
            int count =   source.Count();
            if (count == 0)
                return PaginatedResult<T>.Success(new List<T>(), count, pageNumber, pageSize);

            pageNumber = pageNumber <= 0 ? 1 : pageNumber;
            var items =  source.Sort(orderBy).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return PaginatedResult<T>.Success(items, count, pageNumber, pageSize);
        }
    }
}
