using FlightRoutesSystem.Domain.Entities.Routes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlightRoutesSystem.DataAccess.Maps.Routes
{
    public class RouteMapper : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder.ToTable("routes");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.OriginId)
                .IsRequired();

            builder.Property(r => r.DestinyId)
                .IsRequired();

            builder.HasOne(x=> x.Origin)
                .WithMany()
                .HasForeignKey(r => r.OriginId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x=> x.Destiny)
                .WithMany()
                .HasForeignKey(r => r.DestinyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x=> x.Connections)
                .WithOne()
                .HasForeignKey(c => c.RouteId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
