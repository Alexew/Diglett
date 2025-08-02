using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Diglett.Web.Rendering
{
    public class CssClassList : Disposable
    {
        private readonly HashSet<string> _list;
        private readonly object _source;

        internal CssClassList(object source)
        {
            Guard.NotNull(source);

            string? currentValue = null;

            if (source is TagHelperAttributeList list)
            {
                if (list.TryGetAttribute("class", out var attribute))
                {
                    currentValue = attribute.Value?.ToString();
                }
            }
            else if (source is IDictionary<string, string> dict)
            {
                dict.TryGetValue("class", out currentValue);
            }
            else
            {
                throw new ArgumentException($"Source must be {nameof(TagHelperAttributeList)} or {nameof(IDictionary<string, string>)}", nameof(source));
            }

            _list = new HashSet<string>(currentValue.HasValue()
                ? currentValue!.Trim().Tokenize([' '], StringSplitOptions.RemoveEmptyEntries) :
                []);

            _source = source;
        }

        public bool Add(params string[] classValues)
        {
            int len = _list.Count;

            foreach (var classValue in classValues)
            {
                ValidateClass(classValue, nameof(classValues));

                if (classValue.HasValue())
                {
                    _list.Add(classValue);
                }
            }

            return _list.Count != len;
        }

        public bool Remove(params string[] classValues)
        {
            int len = _list.Count;

            foreach (var classValue in classValues)
            {
                ValidateClass(classValue, nameof(classValues));

                if (classValue.HasValue())
                {
                    _list.Remove(classValue);
                }
            }

            return _list.Count != len;
        }

        public void ApplyTo(TagHelperAttributeList target)
        {
            Guard.NotNull(target);

            if (_list.Count == 0)
            {
                target.RemoveAll("class");
            }
            else
            {
                target.SetAttribute("class", string.Join(' ', _list));
            }
        }

        public void ApplyTo(IDictionary<string, string> target)
        {
            Guard.NotNull(target);

            if (_list.Count == 0)
            {
                target.TryRemove("class", out _);
            }
            else
            {
                target["class"] = string.Join(' ', _list);
            }
        }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (_source is TagHelperAttributeList list)
                {
                    ApplyTo(list);
                }
                else if (_source is IDictionary<string, string> dict)
                {
                    ApplyTo(dict);
                }
            }
        }

        private static void ValidateClass(string value, string paramName)
        {
            if (value.Any(char.IsWhiteSpace))
            {
                throw new ArgumentException($"The class provided ('{value}') contains whitespace characters, which is not valid", paramName);
            }
        }
    }
}
