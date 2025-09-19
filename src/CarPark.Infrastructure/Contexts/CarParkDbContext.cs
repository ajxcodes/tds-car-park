using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace CarPark.Infrastructure.Contexts;

public class CarParkDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}