namespace TweetBook.Contracts.V1.Request.Auth
{
    public class UserLoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
