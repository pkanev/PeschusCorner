namespace Peschu.Services.Models
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;

    public class ResourceListingModel : IMapFrom<Resource>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ResourceType { get; set; }
        
        public void ConfigureMapping(Profile mapper)
            => mapper
                .CreateMap<Resource, ResourceListingModel>()
                .ForMember(r => r.ResourceType, cfg => cfg.MapFrom(r => r.ResourceType.ToString()));
    }
}
