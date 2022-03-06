namespace TweetBook.Contracts.V1.Responses.Queries
{
    public class PaginationQuery
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public const int MaxPageSize = 20;
        public const int MinPageSize = 1;
        public const int StartingPageNumber = 0;

        public PaginationQuery()
        {
            PageNumber = StartingPageNumber;
            PageSize = MaxPageSize;
        }

        public PaginationQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < StartingPageNumber ? StartingPageNumber : pageNumber;

            PageSize = pageSize > MaxPageSize ? MaxPageSize :
                pageSize < MinPageSize ? MinPageSize : pageSize;
        }
    }
}
