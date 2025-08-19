using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Diglett.Web
{
    public class PartialContext
    {
        public PartialContext(ActionContext actionContext)
            : this(actionContext, null) { }

        public PartialContext(ActionContext actionContext, object? model)
        {
            ActionContext = Guard.NotNull(actionContext);
            Model = model;

            if (actionContext is ViewContext viewContext)
            {
                ViewData = viewContext.ViewData;
                TempData = viewContext.TempData;
                Writer = viewContext.Writer;
            }
        }

        public ActionContext ActionContext { get; set; }
        public HttpContext HttpContext => ActionContext.HttpContext;

        public string ViewName { get; set; } = null!;
        public bool IsMainPage { get; set; }

        public object? Model { get; set; }
        public ViewDataDictionary? ViewData { get; set; }
        public ITempDataDictionary? TempData { get; set; }
        public TextWriter? Writer { get; }
    }
}
