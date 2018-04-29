namespace Peschu.Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Tag
    {
        public int Id { get; set; }

        [Required]
        [MinLength(TagTitleMinLength)]
        [MaxLength(TagTitleMaxLength)]
        public string Title { get; set; }

        public ICollection<ArticleTag> Articles { get; set; } = new HashSet<ArticleTag>();
    }
}
