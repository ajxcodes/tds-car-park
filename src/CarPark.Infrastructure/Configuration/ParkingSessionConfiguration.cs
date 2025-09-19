using CarPark.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarPark.Infrastructure.Configuration;

public class ParkingSessionConfiguration : IEntityTypeConfiguration<ParkingSession>
{
    public void Configure(EntityTypeBuilder<ParkingSession> builder)
    {
        builder.HasKey(session => session.Id);
        builder.Property(session => session.TimeIn).IsRequired();
        builder.Property(session => session.TimeOut).IsRequired(false);
        builder.Property(session => session.Charge).IsRequired(false);
    }
}
