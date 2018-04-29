namespace Peschu.Web.Controllers
{
    using Data.Models;
    using Microsoft.AspNetCore.Mvc;
    using Services;
    using System.Threading.Tasks;

    public class EconomicsController : BaseController
    {
        public EconomicsController(IArticleService articles) : base(articles)
        {
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var model = await this.GetPaginatedArticlesWithTagsForSubjectAsync(Subject.Economics, page);
            model.Area = string.Empty;
            model.Controller = "Economics";
            model.Action = nameof(Index);
            return View(model);
        }

    }
}
