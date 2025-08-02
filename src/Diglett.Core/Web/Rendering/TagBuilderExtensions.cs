using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace Diglett.Web.Rendering
{
    public static class TagBuilderExtensions
    {
        public static TagBuilder AppendCssClass(this TagBuilder builder, Func<string> cssClass)
        {
            builder.Attributes.AddInValue("class", ' ', cssClass(), false);
            return builder;
        }

        public static TagBuilder PrependCssClass(this TagBuilder builder, Func<string> cssClass)
        {
            builder.Attributes.AddInValue("class", ' ', cssClass(), true);
            return builder;
        }

        public static TagBuilder AppendCssClass(this TagBuilder builder, string cssClass)
        {
            builder.Attributes.AddInValue("class", ' ', cssClass, false);
            return builder;
        }

        public static TagBuilder PrependCssClass(this TagBuilder builder, string cssClass)
        {
            builder.Attributes.AddInValue("class", ' ', cssClass, true);
            return builder;
        }

        public static CssClassList GetClassList(this TagBuilder builder)
        {
            return new CssClassList(builder.Attributes);
        }

        public static void AddCssStyle(this TagBuilder builder, string expression, object value)
        {
            Guard.NotEmpty(expression);
            Guard.NotNull(value);

            var style = expression + ": " + Convert.ToString(value, CultureInfo.InvariantCulture);

            if (builder.Attributes.TryGetValue("style", out var str))
            {
                builder.Attributes["style"] = style + "; " + str;
            }
            else
            {
                builder.Attributes["style"] = style;
            }
        }
    }
}
