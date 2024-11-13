using FlightRoutesSystem.Domain.Entities.Connections;
using FluentValidation;

namespace FlightRoutesSystem.Validation.Connections
{
    public class ConnectionValidator : AbstractValidator<Connection>, IValidator<Connection>
    {
        public ConnectionValidator()
        {
            RuleFor(route => route.AirportId).GreaterThan(0).WithMessage("Airport should be set.");
            RuleFor(route => route.RouteId).GreaterThan(0).WithMessage("Rout should be set.");
        }
    }
}
