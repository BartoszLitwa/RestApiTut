using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TweetBook.Data;
using TweetBook.Domain;

namespace TweetBook.Services
{
    public class TagService : ITagService
    {
        private readonly DataContext _dataContext;

        public TagService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task AddTagsFromPostAsync(Post post)
        {
            for(int i = 0; i < post.Tags.Count; i++)
            {
                var existtingTag = await _dataContext.Tags.FirstOrDefaultAsync(x => x.Name == post.Tags[i].TagName);

                if (existtingTag != null)
                    continue;

                await _dataContext.Tags.AddAsync(new Tag { Name = post.Tags[i].TagName});
            }

            await _dataContext.SaveChangesAsync();
        }

        public async Task<bool> CreateAsync(Tag tag)
        {
            await _dataContext.Tags.AddAsync(tag);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<List<Tag>> GetAllAsync()
        {
            return await _dataContext.Tags.ToListAsync();
        }

        public async Task<Tag> GetTagByNameAsync(string requestName)
        {
            return await _dataContext.Tags.FirstOrDefaultAsync(x => x.Name == requestName);
        }

        public async Task<bool> DeleteTagAsync(string tagName)
        {
            var tag = await _dataContext.Tags.FirstOrDefaultAsync(x => x.Name == tagName);

            if (tag != null)
                _dataContext.Tags.Remove(tag);

            return await _dataContext.SaveChangesAsync() > 0;
        }
    }
}
