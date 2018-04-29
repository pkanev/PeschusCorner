namespace Peschu.Web.Areas.Admin.Controllers
{
    using AutoMapper;
    using Microsoft.AspNetCore.Mvc;
    using Models.Resources;
    using Services;
    using System;
    using System.Threading.Tasks;

    using static WebConstants;

    public class ResourcesController : BaseAdminController
    {
        private readonly IResourceService resources;
        private readonly IArticleService articles;
        private readonly int pageSize = WebConstants.PageSize;

        public ResourcesController(IResourceService resources, IArticleService articles)
        {
            this.resources = resources;
            this.articles = articles;
        }

        public async Task<IActionResult> Index([FromQuery] int page = 1)
        {
            var totalPages = (int)Math.Ceiling(await this.resources.Total() / (decimal)this.pageSize);
            page = page < 1 ? 1 : page;
            page = page > totalPages ? totalPages : page;

            var model = new AllResourcesPageListingModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Resources = await this.resources.GetAllAsync(page, this.pageSize),
                Area = AdminArea,
                Controller = "Resources",
                Action = nameof(Index)
            };

            return View(model);
        }

        public IActionResult Create(int id)
        {
            var articleExists = this.articles.Exists(id);
            if(!articleExists)
            {
                return NotFound();
            }

            var model = new ResourceFormContainerModel
            {
                Form = new ResourceFormModel { },
                ArticleId = id
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, ResourceFormModel model)
        {
            if(!ModelState.IsValid)
            {
                var returnModel = new ResourceFormContainerModel
                {
                    ArticleId = id,
                    Form = model
                };
                return View(returnModel);
            }

            if(id <= 0)
            {
                return NotFound();
            }

            var articleExists = this.articles.Exists(id);
            if (!articleExists)
            {
                return NotFound();
            }

            var newId = await this.resources.CreateAsync(model.Title, model.Url, model.ResourceType);

            if(newId <= 0)
            {
                throw new InvalidOperationException("The resource was not saved in the database.");
            }

            await this.resources.LinkToArticleAsync(newId, id);

            TempData[TempDataSuccessMessageKey] = $"{model.Title} was added to the resources.";
            return RedirectToAction("Edit", "Articles", new { Area = WebConstants.AdminArea, Id = id });
        }

        public async Task<IActionResult> Details(int id, [FromQuery]int page = 1)
        {
            var resource = await this.resources.GetByIdAsync(id);
            if(resource == null)
            {
                return NotFound();
            }

            var totalPages = (int)Math.Ceiling(await this.articles.TotalPerResource(id) / (decimal)this.pageSize);
            page = page < 1 ? 1 : page;
            page = page > totalPages ? totalPages : page;

            var model = new AllArticlesPerResourcePageListingModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Resource = resource,
                Articles = await this.articles.GetByResourceIdAsync(id),
                Area = AdminArea,
                Controller = "Resources",
                Action = nameof(Details),
                RouteId = id
            };

            return View(model);
        }

        public async Task<IActionResult> Edit(int id, [FromQuery]int articleId = 0)
        {
            var form = Mapper.Map<ResourceFormModel>(await this.resources.GetByIdAsync(id));
                
            if (form == null)
            {
                return NotFound();
            }

            var model = new ResourceFormContainerModel
            {
                Form = form,
                ArticleId = articleId
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromQuery] int articleId, ResourceFormModel model)
        {
            if(!ModelState.IsValid)
            {
                var returnModel = new ResourceFormContainerModel
                {
                    Form = model,
                    ArticleId = articleId
                };

                return View(returnModel);
            }

            var result = await this.resources.EditAsync(id, model.Title, model.Url, model.ResourceType);

            if(!result)
            {
                return NotFound();
            }

            TempData[TempDataSuccessMessageKey] = $"{model.Title} was edited successfully.";
            return RedirectToAction(nameof(Details), new { Id = id });
        }

        public async Task<IActionResult> Remove(int id, [FromQuery] int articleId)
        {
            var article = await this.articles.GetById(articleId);
            var resource = await this.resources.GetByIdAsync(id);
            if(article == null || resource == null)
            {
                return NotFound();
            }

            var model = new UnlinkResourceModel
            {
                Article = article,
                Resource = resource
            };

            return View(model);
        }

        public async Task<IActionResult> Unlink(int id, [FromQuery] int articleId)
        {
            var result = await this.resources.UnLinkFromArticleAsync(id, articleId);
            if (!result)
            {
                return NotFound();
            }

            TempData[TempDataSuccessMessageKey] = "Resource unlinked successfully.";
            return RedirectToAction("Edit", "Articles", new { area = WebConstants.AdminArea, id = articleId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var resource = await this.resources.GetByIdAsync(id);
            if(resource == null)
            {
                return NotFound();
            }

            return View(resource);
        }

        public async Task<IActionResult> Destroy(int id)
        {
            var result = await this.resources.DeleteAsync(id);
            if (result == false)
            {
                return NotFound();
            }

            TempData[TempDataSuccessMessageKey] = $"Resource was deleted successfully.";
            return RedirectToAction(nameof(Index));
        }
    }
}
