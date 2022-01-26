namespace TweetBook.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Api = $"{Root}/{Version}";
        public static class Posts
        {
            public const string GetAll = $"{Api}/posts";
            public const string Create = $"{Api}/posts";
            public const string Get = $"{Api}/posts/{{postId}}";
            public const string Update = $"{Api}/posts/{{postId}}";
            public const string Delete = $"{Api}/posts/{{postId}}";
        }

        public static class Identity
        {
            public const string Login = $"{Api}/identity/login";
            public const string Register = $"{Api}/identity/register";
        }
    }
}
