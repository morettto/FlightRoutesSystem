using System;

namespace FlightRoutesSystem.Application.Exceptions.Routes
{
    public class RouteValidationException : Exception
    {
        public RouteValidationException(string message) : base(message)
        {
        }
    }
}
