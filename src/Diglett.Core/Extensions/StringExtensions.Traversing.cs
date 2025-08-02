using Diglett.Core.Utilities;
using Microsoft.Extensions.Primitives;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Diglett
{
    public static partial class StringExtensions
    {
        public static IEnumerable<string> ReadLines(this string? input, bool trimLines = false, bool removeEmptyLines = false)
        {
            if (input.IsEmpty())
                yield break;

            using var sr = new StringReader(input!);
            string? line;

            while ((line = sr.ReadLine()) != null)
            {
                var segment = new StringSegment(line);

                if (trimLines)
                    segment = segment.Trim();

                if (removeEmptyLines && IsEmpty(line))
                    continue;

                yield return segment.Value!;
            }
        }

        public static string? SplitPascalCase(this string? value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            var sb = new ValueStringBuilder(value.Length + 5);
            char[] ca = value.ToCharArray();

            sb.Append(ca[0]);

            for (int i = 1; i < ca.Length - 1; i++)
            {
                char c = ca[i];

                if (char.IsUpper(c) && (char.IsLower(ca[i + 1]) || char.IsLower(ca[i - 1])))
                    sb.Append(' ');

                sb.Append(c);
            }

            if (ca.Length > 1)
                sb.Append(ca[^1]);

            return sb.ToString();
        }

        public static IEnumerable<string> SplitSafe(
            this string? input,
            string? separator,
            StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        {
            if (string.IsNullOrEmpty(input))
                return [];

            if (separator == null)
            {
                for (var i = 0; i < input.Length; i++)
                {
                    var c = input[i];

                    if (c == ';' || c == ',' || c == '|')
                        return Tokenize(input, c, options);

                    if (c == '\r' && (i + 1) < input.Length & input[i + 1] == '\n')
                        return input.ReadLines(false, true);
                }

                if (options.HasFlag(StringSplitOptions.TrimEntries))
                    input = input.Trim();

                return options.HasFlag(StringSplitOptions.RemoveEmptyEntries) && string.IsNullOrWhiteSpace(input)
                    ? []
                    : [input];
            }
            else
            {
                return separator.Length == 1
                    ? Tokenize(input, separator[0], options)
                    : input.Split(separator, options);
            }
        }

        public static IEnumerable<string> SplitSafe(this string? input, char separator, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        {
            return string.IsNullOrEmpty(input) ? [] : Tokenize(input, separator, options);
        }

        public static bool SplitToPair(this string? value, [MaybeNullWhen(false)] out string leftPart, out string rightPart, string? delimiter, bool splitAfterLast = false)
        {
            leftPart = value;
            rightPart = string.Empty;

            if (string.IsNullOrEmpty(value) || string.IsNullOrEmpty(delimiter))
                return false;

            var idx = splitAfterLast
                ? value.LastIndexOf(delimiter)
                : value.IndexOf(delimiter);

            if (idx == -1)
                return false;

            leftPart = value[..idx];
            rightPart = value[(idx + delimiter.Length)..];

            return true;
        }

        public static IEnumerable<string> Tokenize(this string input, char separator, StringSplitOptions options = StringSplitOptions.None)
        {
            return Tokenize(input, [separator], options);
        }

        public static IEnumerable<string> Tokenize(this string input, params char[] separators)
        {
            return Tokenize(input, separators, StringSplitOptions.None);
        }

        public static IEnumerable<string> Tokenize(this string input, char[] separators, StringSplitOptions options)
        {
            IEnumerable<StringSegment> tokenizer = options.HasFlag(StringSplitOptions.TrimEntries)
                ? new TrimmingTokenizer(input, separators)
                : new StringTokenizer(input, separators);

            if (options.HasFlag(StringSplitOptions.RemoveEmptyEntries))
            {
                foreach (var segment in tokenizer)
                {
                    if (segment.Length > 0)
                    {
                        yield return segment.Value!;
                    }
                }
            }
            else
            {
                foreach (var segment in tokenizer)
                {
                    yield return segment.Value!;
                }
            }
        }
    }
}
