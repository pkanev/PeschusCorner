namespace Peschu.Web.Controllers
{
    using Data.Models;
    using Infrastructure.Filters;
    using Microsoft.AspNetCore.Mvc;
    using Models.Articles;
    using Services;
    using System;
    using System.Threading.Tasks;

    public class BaseController : Controller
    {
        private readonly IArticleService articles;
        private readonly int pageSize = WebConstants.PageSize;

        protected BaseController(IArticleService articles)
        {
            this.articles = articles;
        }

        protected IArticleService Articles => this.articles;
        protected int PageSize => this.pageSize;

        protected async Task<ArticlesWithTagsPageListingModel> GetPaginatedArticlesWithTagsForSubjectAsync(Subject subject, int page)
        {
            var model = await this.SetupModelPages(page, true, subject);
            model.Articles = await this.Articles.GetWithTagsBySubjectAsync(subject, page, this.PageSize);

            return model;
        }

        protected async Task<ArticlesWithTagsPageListingModel> SetupModelPages(int page, bool hasSubject = false, Subject subject = Subject.Economics)
        {
            var totalPages = 0;
            if (hasSubject)
            {
                totalPages = (int)Math.Ceiling(await this.Articles.Total(subject) / (decimal)this.PageSize);
            }
            else
            {
                totalPages = (int)Math.Ceiling(await this.Articles.Total() / (decimal)this.PageSize);
            }
            
            page = page < 1 ? 1 : page;
            page = page > totalPages ? totalPages : page;

            var model = new ArticlesWithTagsPageListingModel
            {
                CurrentPage = page,
                TotalPages = totalPages
            };

            return model;
        }
    }
}
