namespace Peschu.Web.Areas.Admin.Models.Tags
{
    using Services.Models;

    public class UnlinkTagModel
    {
        public TagListingModel Tag { get; set; }

        public ArticleDetailsModel Article { get; set; }
    }
}
