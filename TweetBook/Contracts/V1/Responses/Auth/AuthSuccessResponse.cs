namespace TweetBook.Contracts.V1.Responses.Auth
{
    public class AuthSuccessResponse
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
    }
}
