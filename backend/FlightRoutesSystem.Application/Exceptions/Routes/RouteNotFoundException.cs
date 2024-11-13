using System;

namespace FlightRoutesSystem.Application.Exceptions.Routes
{
    public class RouteNotFoundException : Exception
    {
        public RouteNotFoundException() : base("Route not found")
        {
        }
    }
}
