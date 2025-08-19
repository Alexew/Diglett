using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;

namespace Diglett.Core.Web
{
    public sealed class NullView : IView
    {
        public static NullView Instance { get; } = new();
        public string Path => string.Empty;

        public Task RenderAsync(ViewContext viewContext)
        {
            Guard.NotNull(viewContext);

            return Task.CompletedTask;
        }
    }
}
