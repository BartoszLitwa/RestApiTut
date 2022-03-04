using System.Collections.Generic;
using TweetBook.Domain;

namespace TweetBook.Contracts.V1.Request
{
    public class UpdatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
