namespace Peschu.Services
{
    using Data.Models;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ITagService
    {
        Task<IEnumerable<TagListingModel>> GetAllAsync();

        Task<TagListingModel> GetByIdAsync(int id);

        Task<int> CreateAsync(string title);

        Task<bool> EditAsync(int id, string title);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<TagListingModel>> GetForArticleAsync(int articleId);

        Task<bool> LinkToArticleAsync(int tagId, int articleId);

        Task<bool> UnLinkFromArticleAsync(int tagId, int articleId);
    }
}
