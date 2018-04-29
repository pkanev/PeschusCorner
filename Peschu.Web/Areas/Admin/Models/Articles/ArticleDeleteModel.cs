namespace Peschu.Web.Areas.Admin.Models.Articles
{
    using Common.Mapping;
    using Services.Models;

    public class ArticleDeleteModel : IMapFrom<ArticleDetailsModel>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
