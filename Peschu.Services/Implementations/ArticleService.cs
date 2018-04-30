namespace Peschu.Services.Implementations
{
    using AutoMapper.QueryableExtensions;
    using Data;
    using Data.Models;
    using Models;
    using System;    
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;

    public class ArticleService : IArticleService{
        private readonly PeschuDbContext db;

        public ArticleService(PeschuDbContext db)
        {
            this.db = db;
        }

        public async Task<int> CreateAsync(
            string title,
            string description,
            Subject subject,
            string contents,
            DateTime created,
            string authorId)
        {
            var article = new Article
            {
                Title = title,
                Description = description,
                Subject = subject,
                Contents = contents,
                Created = created,
                AuthorId = authorId,
                IsDeleted = false
            };

            await this.db.Articles.AddAsync(article);
            await this.db.SaveChangesAsync();

            return article.Id;
        }


        public async Task<bool> Edit(int id, string title, string description, Subject subject, string contents)
        {
            var article = await this.db.Articles.FindAsync(id);
            if(article == null)
            {
                return false;
            }

            article.Title = title;
            article.Description = description;
            article.Subject = subject;
            article.Contents = contents;

            await this.db.SaveChangesAsync();            
            return true;
        }

        public async Task<bool> Delete(int id)
        {
            var article = await this.db.Articles.FindAsync(id);
            if(article == null)
            {
                return false;
            }

            article.IsDeleted = true;
            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CleanupForUser(string id)
        {
            var articles = await this.db
                .Articles
                .Where(a => a.AuthorId == id)
                .ToListAsync();

            this.db.RemoveRange(articles);
            await this.db.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<ArticleListingModel>> GetAllAsync(int page, int pageSize)
            => await this.db
                .Articles
                .Where(a => a.IsDeleted == false)
                .OrderByDescending(a => a.Created)
                .Skip(((page < 1 ? 1 : page) - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ArticleListingModel>()
                .ToListAsync();

        public async Task<int> Total()
            => await this.db
                    .Articles
                    .Where(a => a.IsDeleted == false)
                    .CountAsync();

        public async Task<int> Total(Subject subject)
            => await this.db
                    .Articles
                    .Where(a => a.IsDeleted == false && a.Subject == subject)
                    .CountAsync();

        public async Task<int> TotalPerTag(int id)
            => await this.db
                .Articles
                .Where(a => a.IsDeleted == false && a.Tags.Any(t => t.TagId == id))
                .CountAsync();

        public async Task<int> TotalPerResource(int id)
            => await this.db
                .Articles
                .Where(a => a.IsDeleted == false && a.Resources.Any(r => r.ResourceId == id))
                .CountAsync();

        public async Task<ArticleDetailsModel> GetById(int id)
            => await this.db
                .Articles
                .Where(a => a.Id == id && a.IsDeleted == false)
                .ProjectTo<ArticleDetailsModel>()
                .FirstOrDefaultAsync();

        public async Task<IEnumerable<ArticleListingModel>> GetByTagIdAsync(int id, int page, int pageSize)
            => await this.db
                .Articles
                .Where(a => a.IsDeleted == false && a.Tags.Any(t => t.TagId == id))
                .OrderByDescending(a => a.Created)
                .Skip(((page < 1 ? 1 : page) - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ArticleListingModel>()
                .ToListAsync();
        
        public async Task<IEnumerable<ArticleListingModel>> GetByResourceIdAsync(int id, int page = 1, int pageSize = 10)
            => await this.db
                .Articles
                .Where(a => a.IsDeleted == false && a.Resources.Any(r => r.ResourceId == id))
                .OrderByDescending(a => a.Created)
                .Skip(((page < 1 ? 1 : page) - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ArticleListingModel>()
                .ToListAsync();

        public bool Exists(int id)
            => this.db
                .Articles
                .Any(a => a.Id == id);

        public async Task<ICollection<ArticlesWithTagsListingModel>> GetAllWithTagsAsync(int page = 1, int pageSize = 10, string search = "")
            => await this.db
                .Articles
                .Where(a => 
                    a.IsDeleted == false
                    && (search == null || search == string.Empty || a.Title.ToLower().Contains(search.ToLower()))
                )
                .OrderByDescending(a => a.Created)
                .Skip(((page < 1 ? 1 : page) - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ArticlesWithTagsListingModel>()
                .ToListAsync();

        public async Task<ICollection<ArticlesWithTagsListingModel>> GetWithTagsBySubjectAsync(Subject subject, int page = 1, int pageSize = 10)
            => await this.db
                .Articles
                .Where(a => a.IsDeleted == false && a.Subject == subject)
                .OrderByDescending(a => a.Created)
                .Skip(((page < 1 ? 1 : page) - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ArticlesWithTagsListingModel>()
                .ToListAsync();

        public async Task<IEnumerable<ArticlesWithTagsListingModel>> GetWithTagsByTagIdAsync(int id, int page = 1, int pageSize = 10)
            => await this.db
                .Articles
                .Where(a => a.IsDeleted == false && a.Tags.Any(t => t.TagId == id))
                .OrderByDescending(a => a.Created)
                .Skip(((page < 1 ? 1 : page) - 1) * pageSize)
                .Take(pageSize)
                .ProjectTo<ArticlesWithTagsListingModel>()
                .ToListAsync();
    }
}
