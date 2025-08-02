namespace Diglett
{
    public static partial class StringExtensions
    {
        public static bool EqualsNoCase(this string? value, string? other)
        {
            return string.Compare(value, other, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static bool StartsWithNoCase(this string value, string other)
        {
            return value.StartsWith(other, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EndsWithNoCase(this string value, string other)
        {
            return value.EndsWith(other, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsNoCase(this string value, string other)
        {
            return value.Contains(other, StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsEmpty(this string? value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool IsWhiteSpace(this string value)
        {
            Guard.NotNull(value);

            if (value.Length == 0)
                return false;

            for (int i = 0; i < value.Length; i++)
            {
                if (!char.IsWhiteSpace(value[i]))
                    return false;
            }

            return true;
        }

        public static bool HasValue(this string? value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }
    }
}
