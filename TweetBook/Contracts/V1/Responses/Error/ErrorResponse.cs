using System.Collections.Generic;

namespace TweetBook.Contracts.V1.Responses.Error
{
    public class ErrorResponse
    {
        public List<ErrorModel> Errors { get; set; } = new List<ErrorModel>();
    }
}
