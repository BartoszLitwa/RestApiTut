using Microsoft.AspNetCore.Mvc;

namespace TweetBook.Contracts.V1.Responses.Queries
{
    public  class GetAllPostsQuery
    {
        // In Swagger it will be visible as a "userId"
        // Requires package Microsoft.AspNetCore.Mvc;
        [FromQuery(Name = "userId")]
        public string UserId { get; set; }
    }
}
