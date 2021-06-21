using dotNetLab2.ViewModels;
using FluentValidation;

namespace dotNetLab2.Validators
{
    public class CommentValidator : AbstractValidator<CommentViewModel>
    {
        public CommentValidator()
    {
            RuleFor(x => x.Text).NotEmpty().WithMessage("You must add your comment!");
           

        }

    }
}
