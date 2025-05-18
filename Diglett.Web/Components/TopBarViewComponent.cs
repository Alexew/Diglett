using Microsoft.AspNetCore.Mvc;

namespace Diglett.Web.Components
{
    public class TopBarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}