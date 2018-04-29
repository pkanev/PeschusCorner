namespace Peschu.Services.Models
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class ArticlesWithTagsListingModel : ArticleBaseModel, IHaveCustomMapping
    {
        public IEnumerable<TagListingModel> Tags { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<Article, ArticlesWithTagsListingModel>()
                .ForMember(a => a.Tags, cfg => 
                    cfg.MapFrom(a => a
                        .Tags
                        .Select(t => new TagListingModel
                        {
                            Id = t.TagId,
                            Title = t.Tag.Title
                        })));
    }
}
