using CarPark.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CarPark.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyDatabaseMigrationsAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<CarParkDbContext>();
        await db.Database.MigrateAsync();
    }
}