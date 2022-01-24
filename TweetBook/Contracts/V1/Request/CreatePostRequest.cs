using System;

namespace TweetBook.Contracts.V1.Request
{
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
