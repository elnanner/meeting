using Challenge.Core.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Challenge.Infrastructure.Validators
{
    public class UserLoginValidator: AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(u => u.Username)
                .NotEmpty()
                .MaximumLength(50)
                .MinimumLength(10);
            RuleFor(u => u.Password)
                .NotEmpty()
                .Length(8);
        }
    }
}
