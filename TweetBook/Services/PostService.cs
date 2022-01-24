using System;
using System.Collections.Generic;
using System.Linq;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class PostService : IPostService
    {
        private List<Post> _posts;

        public PostService()
        {
            _posts = new List<Post>();
            for (int i = 0; i < 5; i++)
            {
                _posts.Add(new Post
                {
                    Id = Guid.NewGuid(),
                    Title = $"Post Name {i}"
                });
            }
        }

        public void AddPost(Post post)
        {
            _posts.Add(post);
        }

        public Post GetPostById(Guid id)
        {
            return _posts.SingleOrDefault(x => x.Id == id);
        }

        public List<Post> GetPosts()
        {
            return _posts;
        }

        public bool UpdatePost(Post postToUpdate)
        {
            var exist = GetPostById(postToUpdate.Id) != null;

            if (!exist)
                return false;

            var index = _posts.FindIndex(x => x.Id == postToUpdate.Id);
            _posts[index] = postToUpdate;

            return true;
        }
    }
}
