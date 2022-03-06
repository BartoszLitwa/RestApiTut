﻿namespace TweetBook.Contracts.V1.Responses.Pagination
{
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
        public string NextPage { get; set; }
        public string PreviousPage { get; set; }

        // Only for Refit autogenerated SDK
        public PagedResponse() { }

        public PagedResponse(IEnumerable<T> data)
        {
            Data = data;
        }
    }
}
