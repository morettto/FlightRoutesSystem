using System;

namespace FlightRoutesSystem.Application.Exceptions.Airports
{
    public class AirportValidationException : Exception
    {
        public AirportValidationException(string message) : base(message)
        {
        }
    }
}
