namespace Peschu.Web.Areas.Admin.Models.Resources
{
    using Data.Models;
    using Common.Mapping;
    using Services.Models;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class ResourceFormModel : IMapFrom<ResourceDetailsModel>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ResourceTitleMinLength)]
        [MaxLength(ResourceTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [Url(ErrorMessage = "Please enter a valid url")]
        public string Url { get; set; }

        public ResourceType ResourceType { get; set; }
    }
}
