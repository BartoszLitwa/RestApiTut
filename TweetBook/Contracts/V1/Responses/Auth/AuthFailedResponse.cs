using System.Collections.Generic;

namespace TweetBook.Contracts.V1.Responses.Auth
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
