using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetBook.Data;
using TweetBook.Domain.Pagination;
using TweetBook.Domain.Posts;

namespace TweetBook.Services
{
    public class PostService : IPostService
    {
        private readonly DataContext _dataContext;
        private readonly ITagService _tagService;

        public PostService(DataContext dataContext, ITagService tagService)
        {
            _dataContext = dataContext;
            _tagService = tagService;
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            await _dataContext.Posts.AddAsync(post);

            await _tagService.AddTagsFromPostAsync(post);

            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<Post> GetPostByIdAsync(Guid id)
        {
            return await _dataContext.Posts.Include(p => p.Tags)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetPostsAsync(PaginationFilter paginationFilter = null)
        {
            if(paginationFilter == null)
            {
                return await _dataContext.Posts.Include(p => p.Tags).ToListAsync();
            }

            // Calculate how many entries we have to skip
            // PageNumber Starts from 0
            var skipAmount = paginationFilter.PageNumber * paginationFilter.PageSize;

            return await _dataContext.Posts
                .Include(p => p.Tags)
                .Skip(skipAmount)
                .Take(paginationFilter.PageSize).ToListAsync();
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            _dataContext.Posts.Update(postToUpdate);

            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);

            if(post == null)
                return false;

            _dataContext.Posts.Remove(post);
            
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            return await _dataContext.Posts
                // Disable tracking of that entity to prevent future conflicts and performance issues
                .AsNoTracking()
                .AnyAsync(x => x.Id == postId && x.UserId == userId);
        }
    }
}
