using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Diglett.Web.TagHelpers
{
    [HtmlTargetElement("*", Attributes = IfAttributeName)]
    public class IfTagHelper : TagHelper
    {
        const string IfAttributeName = "dt-if";

        public override int Order => int.MinValue;

        [HtmlAttributeName(IfAttributeName)]
        public bool Condition { get; set; } = true;

        public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (!Condition)
            {
                output.SuppressOutput();
            }

            return Task.CompletedTask;
        }
    }
}
