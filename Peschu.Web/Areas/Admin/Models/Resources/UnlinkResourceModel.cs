namespace Peschu.Web.Areas.Admin.Models.Resources
{
    using Services.Models;

    public class UnlinkResourceModel
    {
        public ResourceDetailsModel Resource { get; set; }

        public ArticleDetailsModel Article { get; set; }
    }
}
