using FluentValidation;
using RentalMotorcycle.Application.Features.Motorcycles.Commands.CreateMotorcycle;
using System;
using System.Collections.Generic;
using System.Text;

namespace RentalMotorcycle.Application.Features.Motorcycles.Validators
{
    public class CreateMotorcycleCommandValidator : AbstractValidator<CreateMotorcycleCommand>
    {
        private const int MOTORCYCLES_RELEASE_DATE = 1885;

        public CreateMotorcycleCommandValidator()
        {
            RuleFor(m => m.Model)
                .NotEmpty().WithMessage("Motorcycle Model is Required.")
                .Length(2, 50).WithMessage("Motorcycle model myst be between 2 and 500 characters.");

            RuleFor(m => m.Year)
                .NotEmpty().WithMessage("Motorcycle Year is required.")
                .LessThanOrEqualTo(DateTime.UtcNow.Year + 1).WithMessage($"The motorcycle year should be less than or equal to {DateTime.UtcNow.Year + 1}")
                .GreaterThanOrEqualTo(MOTORCYCLES_RELEASE_DATE).WithMessage($"The motorcycle year should be greated than or equal to {MOTORCYCLES_RELEASE_DATE}");

            RuleFor(m => m.LicensePlate)
                .NotEmpty().WithMessage("Motorcycle license plate is required.")
                .Length(7).WithMessage("The license plate must be exactly 7 characters long.")
                .Matches(@"^[a-zA-Z]{3}[0-9][a-zA-Z0-9][0-9]{2}$")
                .WithMessage("The license plate must follow the Brazilian or Mercosul format (AAA0A00 or AAA0000).");
        }
    }
}
