using System.Collections.Generic;

namespace FlightRoutesSystem.Domain.Entities.Routes.dto
{
    public class RouteDTO
    {
        public List<long> AirportConnectionIds { get; set; }
        public double Price { get; set; }
        public long OriginId { get; set; }
        public long DestinyId { get; set; }
    }
}
