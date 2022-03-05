using System.Collections.Generic;
using System.Threading.Tasks;
using TweetBook.Domain.Posts;
using TweetBook.Domain.Tags;

namespace TweetBook.Services
{
    public interface ITagService
    {
        Task AddTagsFromPostAsync(Post post);
        Task<List<Tag>> GetAllAsync();
        Task<Tag> GetTagByNameAsync(string requestName);
        Task<bool> CreateAsync(Tag tag);
        Task<bool> DeleteTagAsync(string tagName);
    }
}
