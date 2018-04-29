namespace Peschu.Web.Models.Articles
{
    using Services.Models;
    using System.Collections.Generic;

    public class ArticleReadContainer
    {
        public ArticleDetailsModel Article { get; set; }

        public IEnumerable<TagListingModel> Tags { get; set; }

        public IEnumerable<ResourceDetailsModel> Resources { get; set; }
    }
}
