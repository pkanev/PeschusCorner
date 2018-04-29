namespace Peschu.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class PeschuDbContext : IdentityDbContext<User>
    {
        public PeschuDbContext(DbContextOptions<PeschuDbContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Resource> Resources { get; set; }

        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Article>()
                .HasOne(a => a.Author)
                .WithMany(a => a.Articles)
                .HasForeignKey(a => a.AuthorId);

            builder
                .Entity<ArticleResource>()
                .HasKey(ar => new { ar.ArticleId, ar.ResourceId });

            builder
                .Entity<Article>()
                .HasMany(a => a.Resources)
                .WithOne(r => r.Article)
                .HasForeignKey(r => r.ArticleId);

            builder
                .Entity<Resource>()
                .HasMany(r => r.Articles)
                .WithOne(a => a.Resource)
                .HasForeignKey(a => a.ResourceId);

            builder
                .Entity<ArticleTag>()
                .HasKey(at => new { at.ArticleId, at.TagId });

            builder
                .Entity<ArticleTag>()
                .HasOne(at => at.Article)
                .WithMany(a => a.Tags)
                .HasForeignKey(at => at.ArticleId);

            builder
                .Entity<ArticleTag>()
                .HasOne(at => at.Tag)
                .WithMany(t => t.Articles)
                .HasForeignKey(at => at.TagId);

            base.OnModelCreating(builder);
        }
    }
}