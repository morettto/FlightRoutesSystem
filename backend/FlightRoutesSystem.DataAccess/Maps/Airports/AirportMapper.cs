using FlightRoutesSystem.Domain.Entities.Airports;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightRoutesSystem.DataAccess.Maps.Airports
{
    public class AirportMapper : IEntityTypeConfiguration<Airport>
    {
        public void Configure(EntityTypeBuilder<Airport> builder)
        {
            builder.ToTable("airports");
            builder.HasKey(airport => airport.Id);
            builder.Property(airport => airport.Name)
                .IsRequired()
                .HasMaxLength(20);
        }
    }
}
