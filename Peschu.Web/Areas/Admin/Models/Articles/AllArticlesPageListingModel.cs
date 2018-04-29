namespace Peschu.Web.Areas.Admin.Models.Articles
{    
    using Services.Models;
    using System.Collections.Generic;
    using Web.Models;

    public class AllArticlesPageListingModel : PageListingBaseModel
    {
        public IEnumerable<ArticleListingModel> Articles { get; set; }
    }
}
