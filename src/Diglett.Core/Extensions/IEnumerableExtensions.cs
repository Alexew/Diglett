using Diglett.Core.Common;
using Diglett.Core.Domain;

namespace Diglett
{
    public static class IEnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this T[]? source)
        {
            if (source == null)
                return true;

            return source.Length == 0;
        }

        public static bool IsNullOrEmpty<T>(this IEnumerable<T>? source)
        {
            if (source == null)
                return true;

            if (source.TryGetNonEnumeratedCount(out var count))
                return count == 0;

            return !source.Any();
        }

        public static void Each<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source is List<T> list)
            {
                list.ForEach(action);
                return;
            }

            foreach (T t in source)
            {
                action(t);
            }
        }

        public static IEnumerable<TEntity> OrderBySequence<TEntity>(this IEnumerable<TEntity> source, IEnumerable<int> ids)
            where TEntity : BaseEntity
        {
            Guard.NotNull(source);
            Guard.NotNull(ids);

            var sorted = from id in ids
                         join entity in source on id equals entity.Id
                         select entity;

            return sorted;
        }
    }
}
