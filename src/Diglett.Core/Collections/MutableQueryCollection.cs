using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace Diglett.Core.Collections
{
    public class MutableQueryCollection : QueryCollection
    {
        private readonly Dictionary<string, StringValues> _store;

        public MutableQueryCollection()
            : this(new Dictionary<string, StringValues>(StringComparer.OrdinalIgnoreCase)) { }

        public MutableQueryCollection(QueryString queryString)
            : this(queryString.ToString()) { }

        public MutableQueryCollection(string queryString)
            : this(QueryHelpers.ParseQuery(queryString)) { }

        public MutableQueryCollection(Dictionary<string, StringValues> store)
            : base(store)
        {
            Guard.NotNull(store);

            _store = store;
        }

        public Dictionary<string, StringValues> Store
        {
            get => _store;
        }

        public virtual MutableQueryCollection Add(string name, string value, bool isUnique = false)
        {
            Guard.NotEmpty(name);

            if (_store.TryGetValue(name, out var existingValues))
            {
                var passedValues = new StringValues(value.SplitSafe(',').ToArray());
                _store[name] = isUnique ? passedValues : StringValues.Concat(existingValues, passedValues);
            }
            else
            {
                _store.Add(name, value);
            }

            return this;
        }

        public virtual MutableQueryCollection Add(string name, StringValues values, bool isUnique = false)
        {
            Guard.NotEmpty(name);

            if (_store.TryGetValue(name, out var existingValues))
            {
                _store[name] = isUnique ? values : StringValues.Concat(existingValues, values);
            }
            else
            {
                _store.Add(name, values);
            }

            return this;
        }

        public MutableQueryCollection Remove(string name)
        {
            Guard.NotEmpty(name);
            _store.TryRemove(name, out _);
            return this;
        }

        public MutableQueryCollection Clear()
        {
            _store.Clear();
            return this;
        }

        public QueryString ToQueryString()
        {
            return QueryString.Create(_store);
        }

        public override string ToString()
        {
            return ToQueryString().Value ?? string.Empty;
        }
    }
}
