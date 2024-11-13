using FlightRoutesSystem.Domain.Base;
using FlightRoutesSystem.Domain.Entities.Airports;
using FlightRoutesSystem.Domain.Entities.Routes;
using System.Text.Json.Serialization;

namespace FlightRoutesSystem.Domain.Entities.Connections
{
    public class Connection : Entity
    {
        public Airport Airport { get; set; }
        [JsonIgnore]
        public Route Route { get; set; }
        [JsonIgnore]
        public long AirportId { get; set; }
        [JsonIgnore]
        public long RouteId { get; set; }
    }
}
