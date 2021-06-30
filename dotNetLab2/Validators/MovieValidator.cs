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
           // RuleFor(x => x.Title).NotEmpty().WithMessage("Title field is mandatory!");
           // RuleFor(x => x.Gender).NotEmpty().WithMessage("Gender field is mandatory!");
           // RuleFor(x => x.Description).MinimumLength(5);
           // RuleFor(x => x.Director).NotEmpty().WithMessage("Director field is mandatory!");
           // RuleFor(x => x.Rating).InclusiveBetween(1, 10);
        }

    }
}