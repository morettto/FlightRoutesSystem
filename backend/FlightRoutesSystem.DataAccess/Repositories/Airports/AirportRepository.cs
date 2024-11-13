using FlightRoutesSystem.DataAccess.Abstracts;
using FlightRoutesSystem.DataAccess.Contexts;
using FlightRoutesSystem.Domain.Entities.Airports;

namespace FlightRoutesSystem.DataAccess.Repositories.Airports
{
    public class AirportRepository : BaseRepository<Airport>
    {
        public AirportRepository(FlightRoutesSystemContext context) : base(context)
        {
        }
    }
}
