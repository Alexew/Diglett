using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace Diglett
{
    public static class DictionaryExtensions
    {
        public static TValue? Get<TKey, TValue>(this IDictionary<TKey, TValue> instance, TKey key) where TKey : notnull
        {
            return Guard.NotNull(instance).TryGetValue(key, out var val) ? val : default;
        }

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

        internal static IDictionary<string, string?> AddInValue(this IDictionary<string, string?> instance, string key, char separator, string value, bool prepend = false)
        {
            Guard.NotNull(instance);
            Guard.NotNull(value);
            Guard.NotEmpty(key);

            value = value.Trim(separator);

            if (string.IsNullOrEmpty(value))
                return instance;

            if (!instance.TryGetValue(key, out var currentValue))
            {
                instance[key] = value;
            }
            else
            {
                if (TryAddInValue(value, currentValue, separator, prepend, out var mergedValue))
                {
                    instance[key] = mergedValue;
                }
            }

            return instance;
        }

        internal static bool TryAddInValue(string value, string? currentValue, char separator, bool prepend, [MaybeNullWhen(false)] out string? mergedValue)
        {
            mergedValue = null;

            if (string.IsNullOrWhiteSpace(currentValue))
            {
                mergedValue = value;
            }
            else
            {
                currentValue = currentValue.Trim(separator);

                var hasManyCurrentValues = currentValue.Contains(separator);
                var hasManyAttemptedValues = value.Contains(separator);

                if (!hasManyCurrentValues && !hasManyAttemptedValues)
                {
                    if (!string.Equals(value, currentValue, StringComparison.Ordinal))
                    {
                        mergedValue = prepend
                            ? value + separator + currentValue
                            : currentValue + separator + value;
                    }
                }
                else
                {
                    var currentValues = hasManyCurrentValues
                        ? currentValue.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        : [currentValue];

                    var attemptedValues = hasManyAttemptedValues
                        ? value.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        : [value];

                    var isDirty = false;

                    for (var i = 0; i < attemptedValues.Length; i++)
                    {
                        var attemptedValue = attemptedValues[i];

                        if (!currentValues.Contains(attemptedValue))
                        {
                            if (prepend)
                            {
                                var newCurrentValues = new string[currentValues.Length + 1];
                                newCurrentValues[0] = attemptedValue;
                                Array.Copy(currentValues, 0, newCurrentValues, 1, currentValues.Length);
                                currentValues = newCurrentValues;
                            }
                            else
                            {
                                Array.Resize(ref currentValues, currentValues.Length + 1);
                                currentValues[^1] = attemptedValue;
                            }

                            isDirty = true;
                        }
                    }

                    if (isDirty)
                    {
                        mergedValue = string.Join(separator, currentValues);
                    }
                }
            }

            return mergedValue != null;
        }
    }
}
