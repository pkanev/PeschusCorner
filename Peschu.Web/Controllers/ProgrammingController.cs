namespace Peschu.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Threading.Tasks;

    public class ProgrammingController : BaseController
    {
        public ProgrammingController(IArticleService articles) : base(articles)
        {
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await this.GetPaginatedArticlesWithTagsForSubjectAsync(Subject.Programming, page);
            model.Area = string.Empty;
            model.Controller = "Programming";
            model.Action = nameof(Index);
            return View(model);
        }
    }
}
