namespace Peschu.Services
{
    using Data.Models;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IArticleService
    {
        Task<int> CreateAsync(
            string title,
            string description,
            Subject subject,
            string contents,
            DateTime created,
            string authorId);

        Task<bool> Edit(
            int id,
            string title,
            string description,
            Subject subject,
            string contents);

        Task<bool> Delete(int id);

        Task<bool> CleanupForUser(string id);

        Task<IEnumerable<ArticleListingModel>> GetAllAsync(int page = 1, int pageSize = 10);

        Task<int> Total();

        Task<int> Total(Subject subject);

        Task<int> TotalPerTag(int id);

        Task<int> TotalPerResource(int id);

        Task<ArticleDetailsModel> GetById(int id);

        Task<IEnumerable<ArticleListingModel>> GetByTagIdAsync(int id, int page = 1, int pageSize = 10);

        Task<IEnumerable<ArticleListingModel>> GetByResourceIdAsync(int id, int page = 1, int pageSize = 10);

        Task<ICollection<ArticlesWithTagsListingModel>> GetAllWithTagsAsync(int page = 1, int pageSize = 10);

        Task<ICollection<ArticlesWithTagsListingModel>> GetWithTagsBySubjectAsync(Subject subject, int page = 1, int pageSize = 10);

        Task<IEnumerable<ArticlesWithTagsListingModel>> GetWithTagsByTagIdAsync(int id, int page = 1, int pageSize = 10);

        bool Exists(int id);
    }
}
