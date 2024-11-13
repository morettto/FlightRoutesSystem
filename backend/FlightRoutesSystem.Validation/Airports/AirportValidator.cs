using FlightRoutesSystem.Domain.Entities.Airports;
using FluentValidation;
using FluentValidation.Results;

namespace FlightRoutesSystem.Validation.Airports
{
    public class AirportValidator : AbstractValidator<Airport>, IValidator<Airport>
    {
        public AirportValidator()
        {
            RuleFor(route => route.Name)
                .NotEmpty()
                    .WithMessage("Name should be set.")
                .Length(3, 3)
                    .WithMessage("Name should be 3 characters");
        }

        public virtual ValidationResult PublicValidade(Airport airport)
        {
            return Validate(airport);
        }
    }
}
