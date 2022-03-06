using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TweetBook.Domain.Pagination;
using TweetBook.Domain.Posts;

namespace TweetBook.Services
{
    public interface IPostService
    {
        Task<List<Post>> GetPostsAsync(PaginationFilter paginationFilter = null);
        Task<Post> GetPostByIdAsync(Guid id);
        Task<bool> CreatePostAsync(Post post);
        Task<bool> UpdatePostAsync(Post postToUpdate);
        Task<bool> DeletePostAsync(Guid postId);
        Task<bool> UserOwnsPostAsync(Guid postId, string userId);
    }
}
