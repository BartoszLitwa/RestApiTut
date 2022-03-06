using System;
using System.Collections.Generic;
using TweetBook.Contracts.V1.Responses.Pagination;
using TweetBook.Contracts.V1.Responses.Post;
using TweetBook.Contracts.V1.Responses.Queries;
using TweetBook.Domain.Pagination;
using TweetBook.Services;

namespace TweetBook.Helpers
{
    public static class PaginationHelpers
    {
        public static PagedResponse<T> CreatePaginatedResponse<T>(this IUriService _uriService, PaginationFilter pagination, List<T> response)
        {
            var nextPage = pagination.PageNumber >= PaginationQuery.StartingPageNumber && response.Count == pagination.PageSize ?
                _uriService.GetAllPostsUri(new PaginationQuery { PageNumber = pagination.PageNumber + 1, PageSize = pagination.PageSize }).ToString()
                : null;

            var previousPage = pagination.PageNumber - 1 >= PaginationQuery.StartingPageNumber ?
                _uriService.GetAllPostsUri(new PaginationQuery { PageNumber = pagination.PageNumber - 1, PageSize = pagination.PageSize }).ToString()
                : null;

            return new PagedResponse<T>
            {
                Data = response,
                NextPage = nextPage,
                PreviousPage = previousPage,
                PageNumber = pagination.PageNumber,
                PageSize = pagination.PageSize
            };
        }
    }
}
