namespace Peschu.Web.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : BaseAdminController
    {
        public IActionResult Index() => RedirectToAction("Index", "Articles");        
    }
}
