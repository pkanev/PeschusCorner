namespace Peschu.Data.Models
{
    public class ArticleResource
    {
        public int ArticleId { get; set; }

        public Article Article { get; set; }

        public int ResourceId { get; set; }

        public Resource Resource { get; set; }
    }
}
