namespace Peschu.Web.Areas.Admin.Models.Articles
{
    using Common.Mapping;
    using Data.Models;
    using Services.Models;
    using System;
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants;

    public class EditArticleFormModel : IMapFrom<ArticleDetailsModel>
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
    }
}
