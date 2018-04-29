namespace Peschu.Web.Areas.Admin.Models.Tags
{
    using Common.Mapping;
    using Services.Models;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class TagFormModel : IMapFrom<TagListingModel>
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter comma separated tags.")]
        [MinLength(TagTitleMinLength)]
        [MaxLength(TagTitleMaxLength)]
        public string Title { get; set; }
    }
}
