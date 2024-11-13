using FlightRoutesSystem.DataAccess.Abstracts;
using FlightRoutesSystem.DataAccess.Contexts;
using FlightRoutesSystem.Domain.Entities.Routes;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace FlightRoutesSystem.DataAccess.Repositories.Routes
{
    public class RouteRepository : BaseRepository<Route>
    {
        public RouteRepository(FlightRoutesSystemContext context) : base(context)
        {
        }

        public virtual Route GetCheapestRoute(long originId, long destinyId)
        {
            return _context.Routes.AsNoTracking().Include(x => x.Origin).Include(x => x.Destiny).Include(x => x.Connections).ThenInclude(x => x.Airport)
                    .Where(x => x.OriginId == originId && x.DestinyId == destinyId)
                    .OrderBy(x => x.Price)
                    .FirstOrDefault();
        }

        public virtual List<Route> GetAllWithOriginAndDestinyAndConnections()
        {
            return _context.Routes.AsNoTracking().Include(x=> x.Origin).Include(x=> x.Destiny).Include(x=> x.Connections).ThenInclude(x=> x.Airport).ToList();
        }
    }
}
