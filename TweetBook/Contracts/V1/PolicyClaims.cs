namespace TweetBook.Contracts.V1
{
    public static class PolicyClaims
    {
        public const string Policy = "Policy";

        public static class Tags
        {
            public const string TagViewer = $"{Policy}.TagViewer";
            public const string Claim = "tags.view";
            public const string Value = "true";
        }
    }
}
