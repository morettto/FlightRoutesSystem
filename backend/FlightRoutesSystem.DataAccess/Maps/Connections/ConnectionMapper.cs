using FlightRoutesSystem.Domain.Entities.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightRoutesSystem.DataAccess.Maps.Connections
{
    public class ConnectionMapper : IEntityTypeConfiguration<Connection>
    {
        public void Configure(EntityTypeBuilder<Connection> builder)
        {
            builder.ToTable("connections");
            builder.HasKey(c => c.Id);

            builder.Property(c => c.RouteId)
                .IsRequired();

            builder.Property(c => c.AirportId)
                .IsRequired();

            builder.HasOne(x=> x.Route)
                .WithMany()
                .HasForeignKey(c => c.RouteId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x=> x.Airport)
                .WithMany()
                .HasForeignKey(c => c.AirportId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
