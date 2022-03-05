using Cosmonaut;
using Cosmonaut.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Domain.Posts;

namespace TweetBook.Services
{
    public class CosmosPostService : IPostService
    {
        private readonly ICosmosStore<CosmosPostDto> _cosmosStore;

        public CosmosPostService(ICosmosStore<CosmosPostDto> cosmosStore)
        {
            _cosmosStore = cosmosStore;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            var cosmosPosts = new CosmosPostDto
            {
                Id = Guid.NewGuid().ToString(),
                Title = post.Title,
                Content = post.Content
            };

            var response = await _cosmosStore.AddAsync(cosmosPosts);
            post.Id = Guid.Parse(response.Entity.Id);

            return response.IsSuccess;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var response = await _cosmosStore.RemoveByIdAsync(postId.ToString(), postId.ToString());
            return response.IsSuccess;
        }

        public async Task<Post> GetPostByIdAsync(Guid id)
        {
            var post = await _cosmosStore.FindAsync(id.ToString(), id.ToString());

            return post == null ? null : new Post { Id = Guid.Parse(post.Id), Title = post.Title, Content = post.Content };
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            var posts = await _cosmosStore.Query().ToListAsync();

            return posts.Select(x => new Post { Id = Guid.Parse(x.Id), Title = x.Title, Content = x.Content }).ToList();
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            var cosmosPost = new CosmosPostDto
            {
                Id = postToUpdate.Id.ToString(),
                Title = postToUpdate.Title,
                Content = postToUpdate.Content
            };

            var response = await _cosmosStore.UpdateAsync(cosmosPost);

            return response.IsSuccess;
        }

        public Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
