using Application.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class CustomStudentValidator:AbstractValidator<CreateStudDTO>
    {
        public CustomStudentValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Student name is required")
                .MinimumLength(3).WithMessage("Student name must be at least 3 characters")
                .NotEqual("string").WithMessage("Invalid placeholder value");

            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("DescrEmailiption is required")
            .MinimumLength(5).WithMessage("Email must be at least 5 characters")
            .NotEqual("string").WithMessage("Invalid placeholder value");
        }
    }
}
