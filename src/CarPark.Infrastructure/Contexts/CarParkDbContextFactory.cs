using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace CarPark.Infrastructure.Contexts;

public class CarParkDbContextFactory : IDesignTimeDbContextFactory<CarParkDbContext>
{
    public CarParkDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../CarPark.Api"))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var optionsBuilder = new DbContextOptionsBuilder<CarParkDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new CarParkDbContext(optionsBuilder.Options);
    }
}