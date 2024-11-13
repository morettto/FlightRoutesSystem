using FlightRoutesSystem.Domain.Entities.Routes;
using FluentValidation;
using FluentValidation.Results;
using System.Linq;

namespace FlightRoutesSystem.Validation.Routes
{
    public class RouteValidator : AbstractValidator<Route>, IValidator<Route>
    {
        public RouteValidator()
        {
            RuleFor(route => route.Price).GreaterThan(0)
                .WithMessage("The price need greater than 0.");

            RuleFor(route => route.OriginId).GreaterThan(0).WithMessage("Origin should be set.");
            RuleFor(route => route.DestinyId).GreaterThan(0).WithMessage("Destiny should be set.");

            RuleFor(route => route.OriginId)
            .NotEqual(route => route.DestinyId)
            .WithMessage("OriginId and DestinyId not be equal.");
        }

        public virtual ValidationResult PublicValidate(Route route)
        {
            return base.Validate(route);
        }
    }
}
