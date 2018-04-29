namespace Peschu.Services.Models
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;

    public class ArticleListingModel : ArticleBaseModel, IHaveCustomMapping
    {   
        public string Author { get; set; }
        
        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<Article, ArticleListingModel>()
                .ForMember(a => a.Author, cfg => cfg.MapFrom(a => a.Author.UserName));
    }
}
