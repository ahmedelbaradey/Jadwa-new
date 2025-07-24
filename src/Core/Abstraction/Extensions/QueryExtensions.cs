using System.Linq.Dynamic.Core;
namespace Abstraction.Common.Extensions
{
    public static class QueryExtensions
    {
        public static IQueryable<T> Sort<T>(this IQueryable<T> data, string orderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(orderByQueryString))
                return data;
            var orderQuery = OrderQueryBuilder.CreateOrderQuery<T>(orderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return data.OrderBy(e => e);
            return data.OrderBy(orderQuery);
        }
    }
}
