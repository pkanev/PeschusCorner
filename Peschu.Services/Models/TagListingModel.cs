namespace Peschu.Services.Models
{
    using Data.Models;
    using Common.Mapping;

    public class TagListingModel : IMapFrom<Tag>
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
