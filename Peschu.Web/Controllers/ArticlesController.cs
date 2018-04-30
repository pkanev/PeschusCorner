namespace Peschu.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models.Articles;
    using Services;
    using System.Threading;
    using System.Threading.Tasks;

    public class ArticlesController : BaseController
    {
        private readonly IResourceService resources;
        private readonly ITagService tags;

        public ArticlesController(IArticleService articles, IResourceService resources, ITagService tags) : base(articles)
        {
            this.resources = resources;
            this.tags = tags;
        }

        protected IResourceService Resources => this.resources;

        protected ITagService Tags => this.tags;

        public async Task<IActionResult> Index(int page = 1, [FromQuery] string search = "")
        {
            if (search == null)
            {
                search = string.Empty;
            }

            var model = await this.SetupModelPages(page);
            model.Articles = await this.Articles.GetAllWithTagsAsync(page, this.PageSize, search);
            model.Area = string.Empty;
            model.Controller = "Articles";
            model.Action = nameof(Index);
            model.Search = search;

            return View(model);
        }

        public async Task<IActionResult> Read(int id)
        {
            var article = await this.Articles.GetById(id);
            if (article == null)
            {
                return NotFound();
            }

            var model = new ArticleReadContainer
            {
                Article = article,
                Resources = await this.Resources.GetDetailedListByArticleIdAsync(id),
                Tags = await this.Tags.GetForArticleAsync(id)
            };
   
            return View(model);
        }
    }
}
