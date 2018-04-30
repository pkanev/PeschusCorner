namespace Peschu.Web.Models.Articles
{
    using Services.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class ArticlesWithTagsPageListingModel : PageListingBaseModel
    {
        public IEnumerable<ArticlesWithTagsListingModel> Articles { get; set; }

        public bool IsEmpty => this.Articles == null || !this.Articles.Any();

        public string Search { get; set; } = string.Empty;
    }
}
