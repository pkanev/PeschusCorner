namespace Peschu.Services.Models
{
    using AutoMapper;
    using Common.Mapping;
    using Data.Models;
    using Services.Helpers;

    public class ResourceDetailsModel : IMapFrom<Resource>, IHaveCustomMapping
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }

        public string EmbedUrl { get; set; }

        public ResourceType ResourceType { get; set; }

        public void ConfigureMapping(Profile mapper)
            => mapper.CreateMap<Resource, ResourceDetailsModel>()
                .ForMember(rdm => rdm.EmbedUrl, cfg => cfg.MapFrom(r => ClipboardFusionHelper.ProcessText(r.Url)));
    }
}
