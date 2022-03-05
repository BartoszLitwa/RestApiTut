﻿using System;
using System.Collections.Generic;
using TweetBook.Contracts.V1.Responses.Tag;
using TweetBook.Domain.Tags;

namespace TweetBook.Contracts.V1.Responses.Post
{
    public class PostResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public IEnumerable<TagResponse> Tags { get; set; }
    }
}
