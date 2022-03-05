namespace TweetBook.Contracts.V1
{
    public static class PolicyClaims
    {
        public const string Policy = "Policy";

        public static class Tags
        {
            public const string PolicyName = $"{Policy}.TagViewer";
            public const string Claim = "tags.view";
            public const string Value = "true";
        }

        public static class WorksForCompany
        {
            public const string PolicyName = $"{Policy}.WorksForCompany";
            public const string Claim = "gmail.com";
            public const string Value = "gmail.com";
        }
    }
}
