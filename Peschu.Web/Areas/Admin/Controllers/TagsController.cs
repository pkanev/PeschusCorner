namespace Peschu.Web.Areas.Admin.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Models.Articles;
    using Models.Tags;
    using Services;
    using System;
    using System.Threading.Tasks;
    using Web;

    using static WebConstants;

    public class TagsController : BaseAdminController
    {
        private readonly ITagService tags;
        private readonly IArticleService articles;
        private readonly int pageSize = WebConstants.PageSize;

        public TagsController(ITagService tags, IArticleService articles)
        {
            this.tags = tags;
            this.articles = articles;
        }

        public async Task<IActionResult> Index() 
            => View(new TagsIndexCompositeModel
                {
                    TagForm = new TagFormModel { },
                    Tags = await this.tags.GetAllAsync()
                });

        [HttpPost]
        public async Task<IActionResult> Create(TagsIndexCompositeModel model)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            var tags = model.TagForm.Title.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var tag in tags)
            {
                await this.tags.CreateAsync(tag.Trim());
            }
            
            TempData[TempDataSuccessMessageKey] = "Tag(s) added successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Details(int id, [FromQuery]int page = 1)
        {
            var tag = await this.tags.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            var totalPages = (int)Math.Ceiling(await this.articles.TotalPerTag(id) / (decimal)this.pageSize);
            page = page < 1 ? 1 : page;
            page = page > totalPages ? totalPages : page;

            var model = new AllArticlesPerTagPageListingModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Articles = await this.articles.GetByTagIdAsync(id, page, this.pageSize),
                Area = AdminArea,
                Controller = "Tags",
                Action = nameof(Details),
                RouteId = id,
                Tag = tag
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tag = await this.tags.GetByIdAsync(id);
            if (tag == null)
            {
                return NotFound();
            }

            var model = Mapper.Map<TagFormModel>(tag);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TagFormModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this.tags.EditAsync(id, model.Title.Trim());
            if(!result)
            {
                return NotFound();
            }

            TempData[TempDataSuccessMessageKey] = $"Tag edited successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int id, [FromQuery] int articleId)
        {
            var article = await this.articles.GetById(articleId);
            var tag = await this.tags.GetByIdAsync(id);

            if(article == null || tag == null)
            {
                return NotFound();
            }

            var model = new UnlinkTagModel
            {
                Article = article,
                Tag = tag
            };

            return View(model);
        }

        public async Task<IActionResult> Unlink(int id, [FromQuery] int articleId)
        {
            var result = await this.tags.UnLinkFromArticleAsync(id, articleId);
            if (!result)
            {
                return NotFound();
            }

            TempData[TempDataSuccessMessageKey] = "Tag removed successfully.";
            return RedirectToAction("Edit", "Articles", new { area = WebConstants.AdminArea, id = articleId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tag = await this.tags.GetByIdAsync(id);
            if(tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }

        public async Task<IActionResult> Destroy(int id)
        {
            var result = await this.tags.DeleteAsync(id);
            if (result == false)
            {
                return NotFound();
            }

            TempData[TempDataSuccessMessageKey] = "Tag deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

    }
}
