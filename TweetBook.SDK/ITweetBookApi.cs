using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Request.Post;
using TweetBook.Contracts.V1.Responses.Pagination;
using TweetBook.Contracts.V1.Responses.Post;

namespace TweetBook.SDK
{
    [Headers("Authorization: Bearer")]
    public interface ITweetBookApi
    {
        [Get("/" + ApiRoutes.Posts.GetAll)]
        Task<ApiResponse<PagedResponse<List<PostResponse>>>> GetAllAsync();

        [Get("/" + ApiRoutes.Posts.Get)]
        Task<ApiResponse<Response<PostResponse>>> GetAsync(Guid postId);

        [Post("/" + ApiRoutes.Posts.Create)]
        Task<ApiResponse<Response<PostResponse>>> CreateAsync([Body] CreatePostRequest createPostRequest);

        [Put("/" + ApiRoutes.Posts.Update)]
        Task<ApiResponse<Response<PostResponse>>> UpdateAsync(Guid postId, [Body] UpdatePostRequest createPostRequest);

        [Delete("/" + ApiRoutes.Posts.Delete)]
        Task<ApiResponse<Response<string>>> DeleteAsync(Guid postId);
    }
}
