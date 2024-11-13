using FlightRoutesSystem.DataAccess.Abstracts;
using FlightRoutesSystem.DataAccess.Contexts;
using FlightRoutesSystem.Domain.Entities.Connections;
using System.Collections.Generic;
using System.Linq;

namespace FlightRoutesSystem.DataAccess.Repositories.Connections
{
    public class ConnectionRepository : BaseRepository<Connection>
    {
        public ConnectionRepository(FlightRoutesSystemContext context) : base(context)
        {
        }

        public virtual List<Connection> GetByRouteId(long routeId)
        {
            return _context.Connections.Where(x => x.RouteId == routeId).ToList();
        }
    }
}
