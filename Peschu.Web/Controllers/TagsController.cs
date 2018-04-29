namespace Peschu.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models.Articles;
    using Services;
    using System.Threading.Tasks;

    public class TagsController : BaseController
    {
        private readonly ITagService tags;

        public TagsController(IArticleService articles, ITagService tags) : base(articles)
        {
            this.tags = tags;
        }

        protected ITagService Tags => this.tags;

        public async Task<IActionResult> Index(int id, int page = 1)
        {
            var tag = await this.Tags.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            var model = new ArticlesWithTagsForTagPageListingModel
            {
                ArticlesContainer = await this.SetupModelPages(page),
                Title = tag.Title
            };

            model.ArticlesContainer.Articles = await this.Articles.GetWithTagsByTagIdAsync(id, page, this.PageSize);
            model.ArticlesContainer.Area = string.Empty;
            model.ArticlesContainer.Controller = "Tags";
            model.ArticlesContainer.Action = nameof(Index);
            model.Title = tag.Title;
            return View(model);
        }
    }
}
