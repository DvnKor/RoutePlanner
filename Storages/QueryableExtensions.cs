using System.Linq;

namespace Storages
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> LimitByOffset<T>(this IQueryable<T> query, int offset, int limit)
        {
            return query
                .Skip(offset)
                .Take(limit);
        }
    }
}