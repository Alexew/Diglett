using System.Globalization;
using System.Web;

namespace Diglett
{
    public static partial class StringExtensions
    {
        public static string? AttributeEncode(this string? value, bool useDefaultEncoder = true)
        {
            if (useDefaultEncoder)
                return HttpUtility.HtmlAttributeEncode(value);

            if (string.IsNullOrEmpty(value))
                return value;

            int pos = value.AsSpan().IndexOfAny("<\"'&");
            if (pos < 0)
                return value;

            var output = new StringWriter(CultureInfo.InvariantCulture);
            output.Write(value.AsSpan(0, pos));

            var remaining = value.AsSpan(pos);
            for (int i = 0; i < remaining.Length; i++)
            {
                char ch = remaining[i];
                if (ch <= '<')
                {
                    switch (ch)
                    {
                        case '<':
                            break;
                        case '"':
                            output.Write('’');
                            break;
                        case '\'':
                            output.Write('/');
                            break;
                        case '&':
                            output.Write('+');
                            break;
                        default:
                            output.Write(ch);
                            break;
                    }
                }
                else
                {
                    output.Write(ch);
                }
            }

            return output.ToString();
        }
    }
}
