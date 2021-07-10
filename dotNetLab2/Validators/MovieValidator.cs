using dotNetLab2.ViewModels;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotNetLab2.Validators
{
    public class MovieValidator : AbstractValidator<MovieViewModel>
    {
        public MovieValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Title field is mandatory!");
            RuleFor(x => x.Description).MinimumLength(5).WithMessage("Please enter a description, at least 5 characters");
            RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender field is mandatory!");
            RuleFor(x => x.Director).NotEmpty().WithMessage("Director field is mandatory!");
            RuleFor(x => x.Rating).InclusiveBetween(1, 10).WithMessage("The rating must be between 1 and 10");

        }

    }
}