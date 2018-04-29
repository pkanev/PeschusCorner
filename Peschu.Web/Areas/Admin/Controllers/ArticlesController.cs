namespace Peschu.Web.Areas.Admin.Controllers
{
    using AutoMapper;
    using Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Models.Articles;
    using Models.Users;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static WebConstants;

    public class ArticlesController : BaseAdminController
    {
        private readonly IArticleService articles;
        private readonly IResourceService resources;
        private readonly ITagService tags;
        private readonly UserManager<User> userManager;
        private readonly int pageSize = PageSize;

        public ArticlesController(
            IArticleService articles,
            IResourceService resources,
            ITagService tags,
            UserManager<User> userManager)
        {
            this.articles = articles;
            this.resources = resources;
            this.tags = tags;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Index([FromQuery] int page = 1)
        {
            var totalPages = (int)Math.Ceiling(await this.articles.Total() / (decimal)this.pageSize);
            page = page < 1 ? 1 : page;
            page = page > totalPages ? totalPages : page;

            var model = new AllArticlesPageListingModel
            {
                CurrentPage = page,
                TotalPages = totalPages,
                Articles = await this.articles.GetAllAsync(page, this.pageSize),
                Area = AdminArea,
                Controller = "Articles",
                Action = nameof(Index)
            };

            return View(model);
        }
            

        public async Task<IActionResult> Create()
        {
            var model = new ArticleFormModel
            {
                Resources = await this.GetResourceListItems(),
                Tags = await this.GetTagsListItems()
            };

            return View(model);
        }
            

        [HttpPost]
        public async Task<IActionResult> Create(ArticleFormModel model)
        {
            if (!ModelState.IsValid)
            {
                model.Resources= await this.GetResourceListItems();
                model.Tags = await this.GetTagsListItems();
                return View(model);
            }

            var currentUserId = this.userManager.GetUserId(this.User);
            
            var articleId = await this.articles.CreateAsync(
                model.Title,
                model.Description,
                model.Subject,
                model.Contents,
                DateTime.UtcNow,
                currentUserId
                );

            await this.LinkResources(articleId, model.ResourceIds);
            await this.LinkTags(articleId, model.TagIds);
            await this.LinkNewTags(articleId, model.NewTags);

            TempData[TempDataSuccessMessageKey] = $"'{model.Title}' was created successfully.";
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var articleForm = Mapper.Map<ArticleFormModel>(await this.articles.GetById(id));
            if (articleForm == null)
            {
                return NotFound();
            }

            articleForm.Resources = await this.GetResourceListItems();
            articleForm.Tags = await this.GetTagsListItems();

            var model = new ArticleContainerModel
            {
                ArticleFormModel = articleForm,
                Resources = await this.resources.GetForArticleAsync(id),
                Tags = await this.tags.GetForArticleAsync(id)
            };

            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Edit(int id, ArticleFormModel model)
        {
            if(!ModelState.IsValid)
            {
                model.Resources = await this.GetResourceListItems();
                model.Tags = await this.GetTagsListItems();

                var returnModel = new ArticleContainerModel
                {
                    ArticleFormModel = model,
                    Resources = await this.resources.GetForArticleAsync(id),
                    Tags = await this.tags.GetForArticleAsync(id)
                };

                return View(returnModel);
            }

            var result = await this.articles.Edit(id, model.Title, model.Description, model.Subject, model.Contents);

            if(result == false)
            {
                return NotFound();
            }

            await this.LinkResources(id, model.ResourceIds);
            await this.LinkTags(id, model.TagIds);
            await this.LinkNewTags(id, model.NewTags);

            TempData[TempDataSuccessMessageKey] = $"'{model.Title}' was edited successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var article = Mapper.Map<ArticleDeleteModel>(await this.articles.GetById(id));
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        public async Task<IActionResult> Destroy(int id)
        {
            var result = await this.articles.Delete(id);
            if(!result)
            {
                return NotFound();
            }

            TempData[TempDataSuccessMessageKey] = "Artilce deleted successfully.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> CleanUpArticles(string id)
        {
            var user = await this.userManager.FindByIdAsync(id);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{id}'.");
            }


            var model = new SimpleUserModel
            {
                Id = id,
                Email = user.Email
            };

            return View(model);
        }

        public async Task<IActionResult> CleanUp(string id)
        {
            var cleaned = await this.articles.CleanupForUser(id);
            if(cleaned)
            {
                TempData[TempDataSuccessMessageKey] = $"Articles cleaned up.";
            }
            else
            {
                TempData[TempDataErrorMessageKey] = $"Articles cleaned up.";
            }

            return RedirectToAction(nameof(Index), new { Area=WebConstants.AdminArea, Controller = "Users" });
        }

        private async Task<IEnumerable<SelectListItem>> GetResourceListItems()
        {
            var resources = await this.resources.GetAllAsync();

            var resourcesListItems = resources
                .Select(r => new SelectListItem
                {
                    Text = r.Title,
                    Value = r.Id.ToString()
                })
                .ToList();

            return resourcesListItems;
        }

        private async Task<IEnumerable<SelectListItem>> GetTagsListItems()
        {
            var tags = await this.tags.GetAllAsync();

            var tagsListItems = tags
                .Select(t => new SelectListItem
                {
                    Text = t.Title,
                    Value = t.Id.ToString()
                })
                .ToList();

            return tagsListItems;
        }
        
        private async Task LinkResources(int articleId, IEnumerable<string> resourceIds)
        {
            foreach (var resourceId in resourceIds)
            {
                var id = int.Parse(resourceId);
                await this.resources.LinkToArticleAsync(id, articleId);
            }
        }

        private async Task LinkTags(int articleId, IEnumerable<string> tagIds)
        {
            foreach (var tagId in tagIds)
            {
                var id = int.Parse(tagId);
                await this.tags.LinkToArticleAsync(id, articleId);
            }
        }

        private async Task LinkNewTags(int articleId, string newTags)
        {
            if(string.IsNullOrWhiteSpace(newTags))
            {
                return;
            }

            var tags = newTags.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var tag in tags)
            {
                var title = tag.Trim();
                var id = await this.tags.CreateAsync(title);

                await this.tags.LinkToArticleAsync(id, articleId);
            }
        }
    }
}
