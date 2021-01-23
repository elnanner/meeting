using Challenge.Core.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Challenge.Infrastructure.Validators
{
    public class MeetingValidator : AbstractValidator<MeetingDto>
    {
        public MeetingValidator()
        {
            RuleFor(m => m.Description)
                .NotNull()
                .NotEmpty()
                .MaximumLength(100)
                .MinimumLength(5);
            RuleFor(m => m.Date)
                .NotNull()
                .GreaterThan(DateTime.Now.AddDays(1));
            RuleFor(m => m.MaxPeople)
                .NotEmpty()
                .GreaterThan(1);
            RuleFor(m=>m.City.CityId)
                .NotEmpty();
        }
    }
}
