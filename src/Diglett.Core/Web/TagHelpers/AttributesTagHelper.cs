using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Diglett.Web.TagHelpers
{
    [HtmlTargetElement("*", Attributes = AttributesName)]
    [HtmlTargetElement("*", Attributes = ConditionalAttributePrefix + "*")]
    public class AttributesTagHelper : TagHelper
    {
        const string AttributesName = "attrs";
        const string ConditionalAttributePrefix = "attr-";

        private IDictionary<string, (bool Condition, string Value)>? _conditionalAttributes;

        public override int Order => -1100;

        [HtmlAttributeName(AttributesName)]
        public AttributeDictionary? Attributes { get; set; }

        [HtmlAttributeName("dt-all-conditional-attrs", DictionaryAttributePrefix = ConditionalAttributePrefix)]
        public IDictionary<string, (bool Condition, string Value)> ConditionalAttributes
        {
            get => _conditionalAttributes ??= new Dictionary<string, (bool, string)>(StringComparer.OrdinalIgnoreCase);
            set => _conditionalAttributes = value;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (Attributes != null)
            {
                foreach (var attr in Attributes)
                {
                    if (attr.Value.HasValue())
                        Add(attr.Key, attr.Value!, output);
                }
            }

            if (_conditionalAttributes != null && _conditionalAttributes.Count > 0)
            {
                foreach (var kvp in _conditionalAttributes)
                {
                    if (kvp.Value.Condition)
                        Add(kvp.Key, kvp.Value.Value, output);
                }
            }
        }

        private static void Add(string key, string value, TagHelperOutput output)
        {
            if (key == "class")
                output.AppendCssClass(value);
            else
                output.MergeAttribute(key, value, key == "value");
        }
    }
}
