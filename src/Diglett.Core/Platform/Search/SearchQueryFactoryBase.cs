using Diglett.Core.Collections;
using Microsoft.AspNetCore.Http;

namespace Diglett.Core.Search
{
    public abstract class SearchQueryFactoryBase
    {
        protected readonly IHttpContextAccessor _httpContextAccessor;

        private Multimap<string, string>? _aliases;

        protected SearchQueryFactoryBase(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected abstract string[] Tokens { get; }

        protected virtual Multimap<string, string> Aliases
        {
            get
            {
                if (_aliases == null)
                {
                    _aliases = [];

                    var request = _httpContextAccessor?.HttpContext?.Request;
                    if (request != null)
                    {
                        var tokens = Tokens;

                        if (request.HasFormContentType)
                        {
                            request.Form?.Keys
                                .Where(x => x.HasValue() && !tokens.Contains(x))
                                .Select(x => new { key = x, val = request.Form[x] })
                                .Each(x => _aliases.AddRange(x.key, x.val.SelectMany(y => y.SplitSafe(','))));
                        }

                        request.Query?.Keys
                            .Where(x => x.HasValue() && !tokens.Contains(x))
                            .Select(x => new { key = x, val = request.Query[x] })
                            .Each(x => _aliases.AddRange(x.key, x.val.SelectMany(y => y.SplitSafe(','))));
                    }
                }

                return _aliases;
            }
        }

        protected virtual T? GetValueFor<T>(string key)
        {
            return TryGetValueFor(key, out T? value) ? value : default;
        }

        protected virtual bool TryGetValueFor<T>(string key, out T? value)
        {
            var request = _httpContextAccessor?.HttpContext?.Request;

            if (request != null && key.HasValue())
                return request.TryGetValueAs(key, out value);

            value = default;
            return false;
        }
    }
}
