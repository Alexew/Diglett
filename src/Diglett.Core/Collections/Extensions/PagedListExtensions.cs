using Diglett.Core.Collections;

namespace Diglett
{
    public static class PagedListExtensions
    {
        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize)
        {
            return new PagedList<T>(source, pageIndex, pageSize);
        }

        public static IPagedList<T> ToPagedList<T>(this IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            return new PagedList<T>(source, pageIndex, pageSize, totalCount);
        }

        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> source, int pageIndex, int pageSize)
        {
            Guard.PagingArgsValid(pageIndex, pageSize, nameof(pageIndex), nameof(pageSize));

            if (pageIndex == 0 && pageSize == int.MaxValue)
            {
                return source;
            }
            else
            {
                var skip = pageIndex * pageSize;

                return skip == 0
                    ? source.Take(pageSize)
                    : source.Skip(skip).Take(pageSize);
            }
        }
    }
}
