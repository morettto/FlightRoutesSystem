using FlightRoutesSystem.Domain.Base;
using FlightRoutesSystem.Domain.Entities.Airports;
using FlightRoutesSystem.Domain.Entities.Connections;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FlightRoutesSystem.Domain.Entities.Routes
{
    public class Route : Entity
    {
        public Airport Origin { get; set; }
        public Airport Destiny { get; set; }
        public List<Connection> Connections { get; set; }
        public double Price { get; set; }
        [JsonIgnore]
        public long OriginId { get; set; }
        [JsonIgnore]
        public long DestinyId { get; set; }
    }
}
