using FluentValidation;
using TweetBook.Contracts.V1.Request.Tag;

namespace TweetBook.Validators.V1.Tag
{
    public class CreateTagRequestValidator : AbstractValidator<CreateTagRequest>
    {
        public CreateTagRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .Matches("^[a-zA-Z0-9 ]*$");

            //RuleFor(x => x.Name)
            //    .Must(s => s.Contains("Special test"));
        }
    }
}
