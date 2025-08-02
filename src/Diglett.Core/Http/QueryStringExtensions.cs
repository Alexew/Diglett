using Diglett.Core.Collections;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace Diglett
{
    public static class QueryStringExtensions
    {
        public static QueryString Merge(this QueryString source, string? other)
        {
            if (!string.IsNullOrEmpty(other) && other[0] != '?')
                return Merge(source, new QueryString('?' + other));

            return Merge(source, new QueryString(other));
        }

        public static QueryString Merge(this QueryString source, QueryString other)
        {
            if (!source.HasValue)
                return other;

            if (!other.HasValue)
                return source;

            return MergeQueryStringInternal(
                new MutableQueryCollection(source.Value),
                other);
        }

        public static QueryString Merge(this IQueryCollection? source, string? other)
        {
            if (!string.IsNullOrEmpty(other) && other[0] != '?')
                return Merge(source, new QueryString('?' + other));

            return Merge(source, new QueryString(other));
        }

        public static QueryString Merge(this IQueryCollection? source, QueryString other)
        {
            if (source == null || source.Count == 0)
                return other;

            if (!other.HasValue)
                return QueryString.Empty;

            return MergeQueryStringInternal(
                new MutableQueryCollection(new Dictionary<string, StringValues>(source)),
                other);
        }

        private static QueryString MergeQueryStringInternal(MutableQueryCollection source, QueryString other)
        {
            var modify = QueryHelpers.ParseQuery(other.Value);

            foreach (var kvp in modify)
            {
                source.Add(kvp.Key, kvp.Value, true);
            }

            return source.ToQueryString();
        }
    }
}
