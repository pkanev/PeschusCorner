namespace Peschu.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static DataConstants;

    public class Article
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

        public DateTime Created { get; set; }

        public bool IsDeleted { get; set; }

        public string AuthorId { get; set; }

        public User Author { get; set; }

        public ICollection<ArticleResource> Resources { get; set; } = new HashSet<ArticleResource>();

        public ICollection<ArticleTag> Tags { get; set; } = new HashSet<ArticleTag>();

    }
}
