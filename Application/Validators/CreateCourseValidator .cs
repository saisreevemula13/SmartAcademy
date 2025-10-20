using Application.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class CreateCourseValidator : AbstractValidator<CreateCourseDTO>
    {
        public CreateCourseValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Course name is required")
                .MinimumLength(3).WithMessage("Course name must be at least 3 characters")
                .NotEqual("string").WithMessage("Invalid placeholder value");

            RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MinimumLength(5).WithMessage("Description must be at least 5 characters")
            .NotEqual("string").WithMessage("Invalid placeholder value");
        }

    }
}
