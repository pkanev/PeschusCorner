namespace Peschu.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Services.Models;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;    

    public class ResourceService : IResourceService
    {
        private readonly PeschuDbContext db;

        public ResourceService(PeschuDbContext db)
        {
            this.db = db;
        }
        public async Task<int> CreateAsync(string title, string url, ResourceType resourceType)
        {
            var resource = new Resource
            {
                Title = title,
                Url = url,
                ResourceType = resourceType
            };

            await this.db.Resources.AddAsync(resource);
            await this.db.SaveChangesAsync();
            return resource.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var resource = await this.db.Resources.FindAsync(id);
            if (resource == null)
            {
                return false;
            }

            this.db.Remove(resource);
            var result = await this.db.SaveChangesAsync();
            if (result < 1)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> EditAsync(int id, string title, string url, ResourceType resourceType)
        {
            var resource = await this.db.Resources.FindAsync(id);
            if (resource == null)
            {
                return false;
            }

            resource.Title = title;
            resource.Url = url;
            resource.ResourceType = resourceType;
            var result = await this.db.SaveChangesAsync();
            if (result != 1)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<ResourceListingModel>> GetAllAsync(int page = 1, int pageSize = 10)
            => await this.db
                .Resources
                .OrderBy(r => r.Title)
                .Skip(((page < 1 ? 1 : page) - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ResourceListingModel>()
                .ToListAsync();

        public async Task<ResourceDetailsModel> GetByIdAsync(int id)
            => await this.db
                .Resources
                .Where(r => r.Id == id)
                .ProjectTo<ResourceDetailsModel>()
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<ResourceDetailsModel>> GetDetailedListByArticleIdAsync(int id)
            => await this.db
                .Resources
                .Where(r => r.Articles.Any(a => a.ArticleId == id))
                .OrderBy(r => r.Title)
                .ProjectTo<ResourceDetailsModel>()
                .ToListAsync();

        public async Task<int> Total()
            => await this.db
                    .Resources
                    .CountAsync();

        public async Task<IEnumerable<ResourceListingModel>> GetForArticleAsync(int articleId)
            => await this.db
                .Resources
                .Where(r => r.Articles.Any(a => a.ArticleId == articleId))
                .ProjectTo<ResourceListingModel>()
                .ToListAsync();

        public async Task<bool> LinkToArticleAsync(int resourceId, int articleId)
        {
            var article = await this.db.Articles.FindAsync(articleId);
            var resource = await this.db.Resources.FindAsync(resourceId);

            if(article == null || resource == null)
            {
                return false;
            }

            var resourceInArticle = await this.db
                .FindAsync<ArticleResource>(articleId, resourceId);
            if (resourceInArticle != null)
            {
                return false;
            }

            article.Resources.Add(new ArticleResource { ResourceId = resourceId });
            var result = await this.db.SaveChangesAsync();
            if(result != 1)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UnLinkFromArticleAsync(int resourceId, int articleId)
        {
            var resourceInArticle = await this.db
                .FindAsync<ArticleResource>(articleId, resourceId);
            if(resourceInArticle == null)
            {
                return false;
            }

            this.db.Remove(resourceInArticle);
            var result = await this.db.SaveChangesAsync();
            if (result != 1)
            {
                return false;
            }

            return true;
        }
    }
}
