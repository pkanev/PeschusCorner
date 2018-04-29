namespace Peschu.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using static DataConstants;

    public class Resource
    {
        public int Id { get; set; }

        [Required]
        [MinLength(ResourceTitleMinLength)]
        [MaxLength(ResourceTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        public ResourceType ResourceType { get; set; }

        public ICollection<ArticleResource> Articles { get; set; } = new HashSet<ArticleResource>();
    }
}
