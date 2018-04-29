namespace Peschu.Web.Areas.Admin.Models.Articles
{
    using Services.Models;

    public class AllArticlesPerTagPageListingModel : AllArticlesPageListingModel
    {
        public TagListingModel Tag { get; set; }
    }
}
