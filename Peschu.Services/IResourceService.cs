namespace Peschu.Services
{
    using Data.Models;
    using Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IResourceService
    {
        Task<IEnumerable<ResourceListingModel>> GetAllAsync(int page = 1, int pageSize = 10);

        Task<ResourceDetailsModel> GetByIdAsync(int id);

        Task<IEnumerable<ResourceDetailsModel>> GetDetailedListByArticleIdAsync(int id);

        Task<int> Total();

        Task<int> CreateAsync(string title, string url, ResourceType resourceType);
        
        Task<bool> EditAsync(int id, string title, string url, ResourceType resourceType);

        Task<bool> DeleteAsync(int id);

        Task<IEnumerable<ResourceListingModel>> GetForArticleAsync(int articleId);

        Task<bool> LinkToArticleAsync(int resourceId, int articleId);

        Task<bool> UnLinkFromArticleAsync(int resourceId, int articleId);
    }
}
