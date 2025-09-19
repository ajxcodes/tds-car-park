using CarPark.Application.Abstractions;
using CarPark.Infrastructure.Contexts;
using CarPark.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarPark.Infrastructure.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration) =>
        services.AddDbContext<CarParkDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")))
            .AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>()
            .AddScoped<IParkingSessionRepository, ParkingSessionRepository>()
            .AddScoped<IParkingSpacesRepository, ParkingSpaceRepository>();
}