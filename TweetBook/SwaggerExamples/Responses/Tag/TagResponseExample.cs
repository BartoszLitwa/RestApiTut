using Swashbuckle.AspNetCore.Filters;
using TweetBook.Contracts.V1.Responses.Tag;

namespace TweetBook.SwaggerExamples.Responses.Tag
{
    public class TagResponseExample : IExamplesProvider<TagResponse>
    {
        public TagResponse GetExamples()
        {
            return new TagResponse
            {
                Name = "new tag",
            };
        }
    }
}
