namespace Peschu.Web.Areas.Admin.Models.Resources
{
    using Models.Articles;
    using Services.Models;

    public class AllArticlesPerResourcePageListingModel : AllArticlesPageListingModel
    {
        public ResourceDetailsModel Resource { get; set; }
    }
}
