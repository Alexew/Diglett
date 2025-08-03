using Diglett.Core.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Diglett
{
    public static class HttpRequestExtensions
    {
        public static bool TryGetValue(this HttpRequest request, string key, out StringValues values)
        {
            values = StringValues.Empty;

            if (request.HasFormContentType)
                values = request.Form[key];

            if (values == StringValues.Empty)
                values = request.Query[key];

            return values != StringValues.Empty;
        }

        public static bool TryGetValueAs<T>(this HttpRequest request, string key, out T? value)
        {
            value = default;

            if (request.TryGetValue(key, out var values))
                return ConvertUtility.TryConvert(values.ToString(), out value);

            return false;
        }
    }
}
