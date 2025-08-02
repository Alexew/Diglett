using Diglett.Core.Collections;
using Microsoft.AspNetCore.Http;

namespace Diglett.Core.Web
{
    public class DefaultWebHelper : IWebHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DefaultWebHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public HttpContext? HttpContext
        {
            get => _httpContextAccessor.HttpContext;
        }

        public string ModifyQueryString(string? url, string? queryModification, string? removeParamName = null, string? anchor = null)
        {
            var request = HttpContext?.Request;

            string baseUri;
            QueryString currentQuery;
            string? currentAnchor;

            if (url == null)
            {
                if (request == null)
                    return string.Empty;

                baseUri = request.PathBase + request.Path;
                currentQuery = request.QueryString;
                currentAnchor = anchor;
            }
            else
            {
                TokenizeUrl(url, out baseUri, out currentQuery, out currentAnchor);
                currentAnchor ??= anchor;
            }

            if (queryModification != null || removeParamName != null)
            {
                var modified = new MutableQueryCollection(currentQuery);

                if (!string.IsNullOrEmpty(removeParamName))
                {
                    modified.Remove(removeParamName);
                }

                currentQuery = modified.Merge(queryModification);
            }

            var result = string.Concat(
                baseUri.AsSpan(),
                currentQuery.ToUriComponent().AsSpan(),
                currentAnchor.LeftPad(pad: '#').AsSpan()
            );

            return result;
        }

        private static void TokenizeUrl(string url, out string baseUri, out QueryString query, out string? anchor)
        {
            baseUri = url;
            query = QueryString.Empty;
            anchor = null;

            var anchorIndex = url.LastIndexOf('#');
            if (anchorIndex >= 0)
            {
                baseUri = url[..anchorIndex];
                anchor = url[anchorIndex..];
            }

            var queryIndex = baseUri.IndexOf('?');
            if (queryIndex >= 0)
            {
                query = new QueryString(baseUri[queryIndex..]);
                baseUri = baseUri[..queryIndex];
            }
        }
    }
}
