using FluentValidation;
using TweetBook.Contracts.V1.Request.Post;

namespace TweetBook.Validators.V1.Post
{
    public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
    {
        public CreatePostRequestValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200)
                .Matches("^[a-zA-Z0-9 ]*$");

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(2000)
                .Matches("^[a-zA-Z0-9 ]*$");

            //RuleFor(x => x.Tags)
            //    .ChildRules(m => m.)
        }
    }
}
