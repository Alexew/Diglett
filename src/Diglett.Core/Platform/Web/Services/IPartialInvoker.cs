using Microsoft.AspNetCore.Html;

namespace Diglett.Web
{
    public interface IPartialInvoker
    {
        Task<IHtmlContent> InvokeAsync(PartialContext context);
    }
}
