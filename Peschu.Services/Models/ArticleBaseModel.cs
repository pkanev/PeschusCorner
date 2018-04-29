namespace Peschu.Services.Models
{
    using Common.Mapping;
    using Data.Models;
    using System;
        
    public abstract class ArticleBaseModel : IMapFrom<Article>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public Subject Subject { get; set; }

        public string Description { get; set; }

        public DateTime Created { get; set; }
    }
}
