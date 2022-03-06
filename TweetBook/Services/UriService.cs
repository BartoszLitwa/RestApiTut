using Microsoft.AspNetCore.WebUtilities;
using System;
using TweetBook.Contracts.V1;
using TweetBook.Contracts.V1.Responses.Queries;

namespace TweetBook.Services
{
    public class UriService : IUriService
    {
        // Private base Uri because it is dependant where we run our api
        private readonly string _baseUri;

        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetAllPostsUri(PaginationQuery pagination = null)
        {
            var uri = new Uri(_baseUri);

            if(pagination == null)
                return uri;

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", pagination.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", pagination.PageSize.ToString());

            return new Uri(modifiedUri);
        }

        public Uri GetPostUri(string postId)
        {
            return new Uri($"{_baseUri}/{ApiRoutes.Posts.Get.Replace("{postId}", postId)}");
        }
    }
}
