using Diglett.Web.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Diglett.Web.TagHelpers
{
    public static class TagHelperOutputExtensions
    {
        #region CSS

        public static CssClassList GetClassList(this TagHelperOutput output)
        {
            return new CssClassList(output.Attributes);
        }

        public static void AppendCssClass(this TagHelperOutput output, string cssClass)
        {
            AddInAttributeValue(output.Attributes, "class", ' ', cssClass, false);
        }

        public static void PrependCssClass(this TagHelperOutput output, string cssClass)
        {
            AddInAttributeValue(output.Attributes, "class", ' ', cssClass, true);
        }

        private static void AddInAttributeValue(TagHelperAttributeList attributes, string name, char separator, string value, bool prepend = false)
        {
            if (!attributes.TryGetAttribute(name, out var attribute))
            {
                attributes.Add(new TagHelperAttribute(name, value));
            }
            else
            {
                var currentValue = attribute.ValueAsString();

                if (DictionaryExtensions.TryAddInValue(value, currentValue, separator, prepend, out var mergedValue))
                {
                    attributes.SetAttribute(name, mergedValue);
                }
            }
        }

        #endregion

        #region Attributes

        public static void MergeAttribute(this TagHelperOutput output, string name, object? value, bool replace = false)
        {
            Guard.NotEmpty(name);

            if (output.Attributes.ContainsName(name) && replace)
                output.Attributes.SetAttribute(name, value);
            else
                output.Attributes.Add(name, value);
        }

        #endregion
    }
}
