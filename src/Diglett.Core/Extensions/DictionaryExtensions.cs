using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Diglett
{
    public static class DictionaryExtensions
    {
        public static bool TryRemove<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, [MaybeNullWhen(false)] out TValue? value)
            where TKey : notnull
        {
            value = default;

            if (source is null || key is null)
                return false;

            if (source is ConcurrentDictionary<TKey, TValue> concurrentDict)
                return concurrentDict.TryRemove(key, out value);

            Guard.NotNull(source);

            if (source.TryGetValue(key, out value))
            {
                source.Remove(key);
                return true;
            }

            return false;
        }

        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue?> values, IEnumerable<KeyValuePair<TKey, TValue?>> other)
            where TKey : notnull
        {
            Guard.NotNull(values);
            Guard.NotNull(other);

            foreach (var kvp in other)
            {
                if (values.ContainsKey(kvp.Key))
                {
                    throw new ArgumentException("An item with the same key has already been added.");
                }

                values.Add(kvp);
            }
        }
    }
}
