namespace Peschu.Web.Areas.Admin.Models.Articles
{
    using Common.Mapping;
    using Data.Models;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Services.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class ArticleFormModel : IMapFrom<ArticleDetailsModel>
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ArticleTitleMinLength)]
        [MaxLength(ArticleTitleMaxLength)]
        public string Title { get; set; }

        public Subject Subject { get; set; }

        [Required]
        [MinLength(ArticleDescriptionMinLength)]
        [MaxLength(ArticleDescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public string Contents { get; set; }

        [Display(Name = "Add new tags")]
        public string NewTags { get; set; }

        public IEnumerable<string> ResourceIds { get; set; } = new List<string>();

        public IEnumerable<SelectListItem> Resources { get; set; }

        public IEnumerable<string> TagIds { get; set; } = new List<string>();

        public IEnumerable<SelectListItem> Tags { get; set; }
    }
}
