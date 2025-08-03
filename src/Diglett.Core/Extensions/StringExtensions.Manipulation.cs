using System.Globalization;
using System.Text;

namespace Diglett
{
    public static partial class StringExtensions
    {
        public static string FormatInvariant(this string format, params object?[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, format, args);
        }

        public static string FormatCurrent(this string format, params object?[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }

        public static string? Truncate(this string? value, int maxLength, string end = "")
        {
            Guard.NotNull(end);
            Guard.IsPositive(maxLength);

            int lenSubStr = maxLength - end.Length;

            if (lenSubStr <= 0)
                throw new ArgumentException("Length of suffix string is greater than or equal to maxLength", nameof(maxLength));

            if (value != null && value.Length > maxLength)
                return value[..lenSubStr].Trim() + end;
            else
                return value;
        }

        public static string EnsureStartsWith(this string value, char prefix)
        {
            Guard.NotNull(value);

            return value.StartsWith(prefix) ? value : (prefix + value);
        }

        public static string EnsureEndsWith(this string value, char suffix)
        {
            Guard.NotNull(value);

            return value.EndsWith(suffix) ? value : (value + suffix);
        }

        public static string Grow(this string? value, string? append, string delimiter = " ")
        {
            if (string.IsNullOrEmpty(value))
                return string.IsNullOrEmpty(append) ? string.Empty : append;

            if (string.IsNullOrEmpty(append))
                return string.IsNullOrEmpty(value) ? string.Empty : value;

            return value + delimiter + append;
        }

        public static void Grow(this StringBuilder sb, string? append, string delimiter = " ")
        {
            if (!string.IsNullOrWhiteSpace(append))
            {
                if (sb.Length > 0 && delimiter != null)
                {
                    sb.Append(delimiter);
                }

                sb.Append(append);
            }
        }

        public static string LeftPad(this string? value, string? format = null, char pad = ' ', int count = 1)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (count < 1)
                return value;

            var left = new string(pad, count);
            var right = value;

            if (!string.IsNullOrWhiteSpace(format))
                right = string.Format(CultureInfo.InvariantCulture, format, value);

            if (right.StartsWith(left))
                return right;
            else
                return left + right;
        }

        public static string RightPad(this string? value, string? format = null, char pad = ' ', int count = 1)
        {
            if (string.IsNullOrEmpty(value))
                return string.Empty;

            if (count < 1)
                return value;

            var left = value;
            var right = new string(pad, count);

            if (!string.IsNullOrWhiteSpace(format))
                left = string.Format(CultureInfo.InvariantCulture, format, value);

            if (left.EndsWith(right))
                return left;
            else
                return left + right;
        }

        public static string? Capitalize(this string? word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return word;

            return word[..1].ToUpper() + word[1..].ToLower();
        }

        public static string? Uncapitalize(this string? word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return word;

            return word[..1].ToLower() + word[1..];
        }
    }
}
