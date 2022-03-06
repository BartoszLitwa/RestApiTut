using Swashbuckle.AspNetCore.Filters;
using TweetBook.Contracts.V1.Request.Tag;

namespace TweetBook.SwaggerExamples.Requests.Tag
{
    public class CreateTagRequestsExample : IExamplesProvider<CreateTagRequest>
    {
        public CreateTagRequest GetExamples()
        {
            return new CreateTagRequest
            {
                Name = "new tag"
            };
        }
    }
}
