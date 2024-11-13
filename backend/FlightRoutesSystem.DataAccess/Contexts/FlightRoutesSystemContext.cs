using FlightRoutesSystem.DataAccess.Maps.Airports;
using FlightRoutesSystem.DataAccess.Maps.Connections;
using FlightRoutesSystem.DataAccess.Maps.Routes;
using FlightRoutesSystem.Domain.Entities.Airports;
using FlightRoutesSystem.Domain.Entities.Connections;
using FlightRoutesSystem.Domain.Entities.Routes;
using Microsoft.EntityFrameworkCore;

namespace FlightRoutesSystem.DataAccess.Contexts
{
    public class FlightRoutesSystemContext : DbContext
    {
        public FlightRoutesSystemContext(DbContextOptions<FlightRoutesSystemContext> options) : base(options){
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
        public FlightRoutesSystemContext(){}

        public DbSet<Airport> Airports { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Route> Routes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AirportMapper());
            modelBuilder.ApplyConfiguration(new ConnectionMapper());
            modelBuilder.ApplyConfiguration(new RouteMapper());

            base.OnModelCreating(modelBuilder);
        }
    }
}
