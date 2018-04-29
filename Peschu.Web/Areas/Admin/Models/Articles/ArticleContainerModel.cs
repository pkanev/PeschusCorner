namespace Peschu.Web.Areas.Admin.Models.Articles
{
    using Services.Models;
    using System.Collections.Generic;

    public class ArticleContainerModel
    {
        public ArticleFormModel ArticleFormModel { get; set; }

        public IEnumerable<TagListingModel> Tags { get; set; }

        public IEnumerable<ResourceListingModel> Resources { get; set; }
    }
}
