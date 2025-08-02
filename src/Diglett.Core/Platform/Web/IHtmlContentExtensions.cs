using Diglett.Core.Http;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Localization;
using System.Text;

namespace Diglett.Core.Web
{
    public static class IHtmlContentExtensions
    {
        public static HtmlString ToHtmlString(this IHtmlContent content)
        {
            Guard.NotNull(content);

            if (content is HtmlString htmlString)
            {
                return htmlString;
            }
            else
            {
                var sb = new StringBuilder(100);
                using var writer = new StringWriter(sb);
                content.WriteTo(writer, WebHelper.HtmlEncoder);

                return new HtmlString(writer.ToString());
            }
        }

        public static bool HasContent(this IHtmlContent content)
        {
            Guard.NotNull(content);

            switch (content)
            {
                case TagBuilder:
                case LocalizedHtmlString:
                case TagHelperAttribute:
                    return true;
                case HtmlString htmlString:
                    return htmlString.Value.HasValue();
                case LocalizedString locString:
                    return locString.Value.HasValue();
                case TagHelperContent tagHelperContent:
                    return !tagHelperContent.IsEmptyOrWhiteSpace;
                case HtmlContentBuilder builder:
                    if (builder.Count == 0)
                        return false;
                    break;
                case TagHelperOutput output:
                    var hasValue = output.TagName.HasValue()
                        || !output.Content.IsEmptyOrWhiteSpace
                        || !output.PreElement.IsEmptyOrWhiteSpace
                        || !output.PreContent.IsEmptyOrWhiteSpace
                        || !output.PostContent.IsEmptyOrWhiteSpace
                        || !output.PostElement.IsEmptyOrWhiteSpace;

                    return hasValue;
            }

            using var writer = new HasContentWriter();

            content.WriteTo(writer, NullHtmlEncoder.Default);

            return writer.HasContent;
        }

        class HasContentWriter : TextWriter
        {
            public override Encoding Encoding => Encoding.UTF8;

            public bool HasContent { get; private set; }

            public override void Write(char value)
            {
                if (!HasContent && !char.IsWhiteSpace(value))
                {
                    HasContent = true;
                }
            }

            public override void Write(string? value)
            {
                if (!HasContent && !string.IsNullOrWhiteSpace(value))
                {
                    HasContent = true;
                }
            }
        }
    }
}
