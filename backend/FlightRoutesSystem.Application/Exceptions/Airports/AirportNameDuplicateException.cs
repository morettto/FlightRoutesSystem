using System;

namespace FlightRoutesSystem.Application.Exceptions.Airports
{
    public class AirportNameDuplicateException : Exception
    {
        public AirportNameDuplicateException(string message) : base(message)
        {
        }
    }
}
