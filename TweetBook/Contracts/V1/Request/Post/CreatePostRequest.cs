using System;
using System.Collections.Generic;
using TweetBook.Domain;

namespace TweetBook.Contracts.V1.Request.Post
{
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string[] Tags { get; set; }

        public CreatePostRequest()
        {
            Tags = new string[0];
        }
    }
}
