namespace Peschu.Web.Models.Articles
{
    using System.Linq;

    public class ArticlesWithTagsForTagPageListingModel
    {
        public ArticlesWithTagsPageListingModel ArticlesContainer { get; set; }
        
        public string Title { get; set; }
    }
}
