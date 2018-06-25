namespace Peschu.Test.Services
{
    using Data.Models;
    using FluentAssertions;
    using Peschu.Data;
    using Peschu.Services.Implementations;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Xunit;

    public class ArticleServiceTest
    {
        private readonly PeschuDbContext db;
        private readonly ArticleService service;

        public ArticleServiceTest()
        {
            Tests.Initialilze();
            this.db = Tests.GetDatabase();
            this.service = new ArticleService(db);
        }

        [Fact]
        public async Task CreateAsyncShouldAddToTheDbAndReturnTrue()
        {
            // Arrange
            //var db = Tests.GetDatabase();
            var title = "Test";
            var description = "Test";
            var subject = Subject.Economics;
            var contents = "Test";
            var created = DateTime.Now;
            var authorId = "Test";

            // Act
            var result = await this.service.CreateAsync(title, description, subject, contents, created, authorId);

            // Assert
            result
                .Should()
                .BeOfType(typeof(int))
                .And
                .BeGreaterThan(0);
            this.db.Articles.Count()
                .Should()
                .Be(1);

        }

        [Fact]
        public async Task EditExistingArticleShouldChangeArticleAndReturnTrue()
        {
            // Arrange
            var articleId = 1;
            var article = new Article
            {
                Id = articleId,
                Title = "Test",
                Description = "Test"
            };
            this.db.Add(article);
            await this.db.SaveChangesAsync();
            var newTitle = "New Title";
            var newDescription = "New Description";
            var newSubject = Subject.Music;
            var newContents = "New contents";

            // Act
            var result = await this.service.Edit(articleId, newTitle, newDescription, newSubject, newContents);
            var editedArticle = this.db.Articles.Find(articleId);

            // Assert
            result
                .Should()
                .Be(true);

            editedArticle.Title
                .Should()
                .Be(newTitle);
            editedArticle.Description
                .Should()
                .Be(newDescription);
            editedArticle.Subject
                .Should()
                .Be(newSubject);
            editedArticle.Contents
                .Should()
                .Be(newContents);
        }

        [Fact]
        public async Task EditNonExistingArticleShouldReturnFalse()
        {
            // Arrange
            var articleId = 1;
            var newTitle = "New Title";
            var newDescription = "New Description";
            var newSubject = Subject.Music;
            var newContents = "New contents";

            // Act
            var result = await this.service.Edit(articleId, newTitle, newDescription, newSubject, newContents);
            var editedArticle = this.db.Articles.Find(articleId);

            // Assert
            result
                .Should()
                .Be(false);

            editedArticle
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task DeletingExistingArticleShouldDeleteAndReturnTrue()
        {
            // Arrange
            var articleId = 1;
            var article = new Article
            {
                Id = articleId,
                IsDeleted = false
            };
            this.db.Add(article);
            await this.db.SaveChangesAsync();

            // Act
            var result = await this.service.Delete(articleId);
            var deletedArticle = this.db.Articles.Find(articleId);

            // Assert
            result
                .Should()
                .BeTrue();
            deletedArticle.IsDeleted
                .Should()
                .BeTrue();                
        }

        [Fact]
        public async Task CleanupForUserShouldPhysicallyRemoveAllUserArticles()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var user = new User { Id = Guid.NewGuid().ToString() };
            var articleOne = new Article { Id = 1, Author = user };
            var articleTwo = new Article { Id = 2, Author = user };
            var articleThree = new Article { Id = 3, Author = user };
            this.db.AddRange(articleOne, articleTwo, articleThree);
            await this.db.SaveChangesAsync();

            // Act
            var result = await this.service.CleanupForUser(userId);
            var userArticles = this
                .db
                .Articles
                .Where(a => a.AuthorId == userId)
                .ToList();

            // Assert
            result
                .Should()
                .BeTrue();

            userArticles
                .Should()
                .HaveCount(0);
        }

        [Fact]
        public async Task DeletingNonExistingArticleShouldReturnFalse()
        {
            // Arrange
            var articleId = 1;

            // Act
            var result = await this.service.Delete(articleId);
            var deletedArticle = this.db.Articles.Find(articleId);

            // Assert
            result
                .Should()
                .BeFalse();
            deletedArticle
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task GetByResourceIdAsyncShouldReturnCorrectResultWithFilterAndORder()
        {
            // ARRANGE
            var firstResource = new Resource { Id = 1, Title = "First Resource" };
            var secondResource = new Resource { Id = 2, Title = "Second Resource" };
            var thirdResource = new Resource { Id = 3, Title = "Third Resource" };

            var firstArticle = new Article { Id = 1, Title = "First", Created = DateTime.Now.AddDays(-2), IsDeleted = false };
            firstArticle.Resources.Add(new ArticleResource { ResourceId = firstResource.Id});
            firstArticle.Resources.Add(new ArticleResource { ResourceId = secondResource.Id });
            firstArticle.Resources.Add(new ArticleResource { ResourceId = thirdResource.Id });

            var secondArticle = new Article { Id = 2, Title = "Second", Created = DateTime.Now.AddDays(-1), IsDeleted = true };
            secondArticle.Resources.Add(new ArticleResource { ResourceId = secondResource.Id });
            secondArticle.Resources.Add(new ArticleResource { ResourceId = thirdResource.Id });

            var thirdArticle = new Article { Id = 3, Title = "Third", Created = DateTime.Now, IsDeleted = false };
            thirdArticle.Resources.Add(new ArticleResource { ResourceId = thirdResource.Id });

            this.db.AddRange(firstArticle, secondArticle, thirdArticle);
            await this.db.SaveChangesAsync();

            //var articleService = new ArticleService(db);

            // ACT
            var result = await this.service.GetByResourceIdAsync(thirdResource.Id);

            // ASSERT
            result
                .Should()
                .Match(r => r.ElementAt(0).Id == thirdArticle.Id
                    && r.ElementAt(1).Id == firstArticle.Id)
                .And
                .HaveCount(2);
        }
    }
}
