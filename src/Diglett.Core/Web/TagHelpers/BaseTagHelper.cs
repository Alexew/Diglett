#nullable disable

using Diglett.Core.Utilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Diglett.Web.TagHelpers
{
    public abstract class BaseTagHelper : TagHelper
    {
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        [HtmlAttributeNotBound]
        public TagHelperOutput Output { get; set; }

        public string Id { get; set; }

        protected virtual string GenerateTagId(TagHelperContext context)
            => context.TagName + '-' + CommonHelper.GenerateRandomDigitCode(10);

        public override sealed void Process(TagHelperContext context, TagHelperOutput output)
        {
            Output = output;
            ProcessCommon(context, output);
            ProcessCore(context, output);
        }

        public override sealed Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            Output = output;
            ProcessCommon(context, output);
            return ProcessCoreAsync(context, output);
        }

        private void ProcessCommon(TagHelperContext context, TagHelperOutput output)
        {
            Id = Id.NullEmpty() ?? GenerateTagId(context);

            if (Id.HasValue())
            {
                output.Attributes.SetAttribute("id", Id);
            }
        }

        protected virtual void ProcessCore(TagHelperContext context, TagHelperOutput output) { }
        protected virtual Task ProcessCoreAsync(TagHelperContext context, TagHelperOutput output)
        {
            ProcessCore(context, output);
            return Task.CompletedTask;
        }
    }
}
