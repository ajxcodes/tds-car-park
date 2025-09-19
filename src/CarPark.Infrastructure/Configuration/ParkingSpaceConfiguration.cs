using CarPark.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarPark.Infrastructure.Configuration;

public class ParkingSpaceConfiguration : IEntityTypeConfiguration<ParkingSpace>
{
    public void Configure(EntityTypeBuilder<ParkingSpace> builder)
    {
        builder.HasKey(space => space.Id);
        builder.Property(space => space.Number).IsRequired();
        builder.Property(space => space.IsOccupied).IsRequired();

        // Seed the database with a predefined number of parking spaces.
        // This data is managed by EF Core migrations.

        var spacesToSeed = GetParkingSpaceSeedData();
        builder.HasData(spacesToSeed);
    }

    private static List<ParkingSpace> GetParkingSpaceSeedData()
    {
        var spacesToSeed = new List<ParkingSpace>();

        for (var i = 1; i <= 50; i++)
        {
            var space = new ParkingSpace
            {
                Id = Guid.Parse($"00000000-0000-0000-0000-{i:d12}"),
                Number = i
            };
            if (i <= 25)
                space.Start();
            spacesToSeed.Add(space);
        }

        return spacesToSeed;
    }
}