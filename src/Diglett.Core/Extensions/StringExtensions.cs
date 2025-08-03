using System.Diagnostics;

namespace Diglett
{
    public static partial class StringExtensions
    {
        public const string NotAvailable = "n/a";
        public const string DumpStr = "------------------------------------------------";

        public static string ToSafe(this string? value, string? defaultValue = null)
        {
            if (!string.IsNullOrEmpty(value))
                return value;

            return (defaultValue ?? string.Empty);
        }

        public static string? TrimSafe(this string? value)
        {
            return !string.IsNullOrEmpty(value) ? value.Trim() : value;
        }

        public static string EmptyNull(this string? value)
        {
            return value ?? string.Empty;
        }

        public static string? NullEmpty(this string? value)
        {
            return string.IsNullOrEmpty(value) ? null : value;
        }

        public static string NaIfEmpty(this string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? NotAvailable : value;
        }

        public static string OrDefault(this string? value, string defaultValue)
        {
            return string.IsNullOrEmpty(value) ? defaultValue : value;
        }

        public static void Dump(this string? value, bool appendMarks = false)
        {
            Debug.WriteLine(value);
            Debug.WriteLineIf(appendMarks, DumpStr);
        }
    }
}
