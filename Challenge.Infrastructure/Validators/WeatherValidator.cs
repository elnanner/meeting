using Challenge.Core.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Challenge.Infrastructure.Validators
{
    public class WeatherValidator: AbstractValidator<WeatherDto>
    {
        public WeatherValidator()
        {
            RuleFor(w => w.Latitude)
                .NotEmpty();
            RuleFor(w => w.Longitude)
                .NotEmpty();
        }
    }
}
