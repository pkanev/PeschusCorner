namespace Peschu.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    
    public class TagService : ITagService
    {
        private readonly PeschuDbContext db;

        public TagService(PeschuDbContext db)
        {
            this.db = db;
        }

        public async Task<int> CreateAsync(string title)
        {
            var tagExists = await this.db
                .Tags
                .Where(t => t.Title.ToLower() == title.ToLower())
                .Select(t => new
                {
                    Id = t.Id
                })
                .FirstOrDefaultAsync();

            if (tagExists != null)
            {
                return tagExists.Id;
            }

            var tag = new Tag
            {
                Title = title
            };

            await this.db.Tags.AddAsync(tag);
            await this.db.SaveChangesAsync();
            return tag.Id;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tag = await this.db.Tags.FindAsync(id);
            if (tag == null)
            {
                return false;
            }

            this.db.Remove(tag);
            var result = await this.db.SaveChangesAsync();
            if(result < 1)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> EditAsync(int id, string title)
        {
            var tag = await this.db.Tags.FindAsync(id);
            if (tag == null)
            {
                return false;
            }

            tag.Title = title;
            var result = await this.db.SaveChangesAsync();
            if (result != 1)
            {
                return false;
            }

            return true;
        }

        public async Task<IEnumerable<TagListingModel>> GetAllAsync()
            => await this.db
                .Tags
                .OrderBy(t => t.Title)
                .ProjectTo<TagListingModel>()
                .ToListAsync();

        public async Task<TagListingModel> GetByIdAsync(int id)
            => await this.db
                .Tags
                .Where(t => t.Id == id)
                .ProjectTo<TagListingModel>()
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<TagListingModel>> GetForArticleAsync(int articleId)
            => await this.db
                .Tags
                .Where(t => t.Articles.Any(a => a.ArticleId == articleId))
                .OrderBy(t => t.Title)
                .ProjectTo<TagListingModel>()
                .ToListAsync();

        public async Task<bool> LinkToArticleAsync(int tagId, int articleId)
        {
            var article = await this.db.Articles.FindAsync(articleId);
            var tag = await this.db.Tags.FindAsync(tagId);

            if (article == null || tag == null)
            {
                return false;
            }

            var tagInArticle = await this.db
                .FindAsync<ArticleTag>(articleId, tagId);
            if (tagInArticle != null)
            {
                return false;
            }

            article.Tags.Add(new ArticleTag{ TagId = tagId });
            var result = await this.db.SaveChangesAsync();
            if (result != 1)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> UnLinkFromArticleAsync(int tagId, int articleId)
        {
            var tagInArticle = await this.db
                .FindAsync<ArticleTag>(articleId, tagId);
            if (tagInArticle == null)
            {
                return false;
            }

            this.db.Remove(tagInArticle);
            var result = await this.db.SaveChangesAsync();
            if (result != 1)
            {
                return false;
            }

            return true;
        }
    }
}
