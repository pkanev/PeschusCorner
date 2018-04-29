namespace Peschu.Web.Controllers
{
    using System.Diagnostics;
    using Microsoft.AspNetCore.Mvc;
    using Models;

    public class HomeController : Controller
    {
        public IActionResult Index() => RedirectToAction("Index", "Articles");

        public IActionResult About() => View();

        public IActionResult Contact() => View();

        public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
