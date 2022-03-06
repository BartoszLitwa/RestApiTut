using System;
using TweetBook.Contracts.V1.Responses.Queries;

namespace TweetBook.Services
{
    public interface IUriService
    {
        Uri GetPostUri(string postId);
        Uri GetAllPostsUri(PaginationQuery pagination = null);
    }
}
