using FluentAssertions;
using System.Net;
using System.Threading.Tasks;
using TweetBook.Contracts.V1;
using System.Net.Http.Json;
using System.Collections.Generic;
using TweetBook.Domain;
using Xunit;
using TweetBook.Contracts.V1.Request;

namespace TweetBook.IntegrationTests.Controllers
{
    public class PostController : IntegrationTest
    {
        [Fact]
        public async Task GetAll_WithoutAnyPosts_ReturnEmptyResponse()
        {
            // Arrange
            await AuthenticateAsync();

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Posts.GetAll);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            (await response.Content.ReadFromJsonAsync<List<Post>>()).Should().BeEmpty();
        }

        [Fact]
        public async Task Get_ReturnsPost_WehnPostExistsInTheDatabase()
        {
            // Arrange
            await AuthenticateAsync();
            var post = await CreatePostAsync(new CreatePostRequest { Title = "TestPost", Content = "Test content" });

            // Act
            var response = await TestClient.GetAsync(ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString()));

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedPost = await response.Content.ReadFromJsonAsync<Post>();

            returnedPost.Id.Should().Be(post.Id);
            returnedPost.Title.Should().Be("TestPost");
        }
    }
}
