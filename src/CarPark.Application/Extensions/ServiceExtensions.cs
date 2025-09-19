using CarPark.Application.Abstractions;
using CarPark.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CarPark.Application.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection ConfigureApplication(this IServiceCollection services) =>
        services
            .AddScoped<IChargeCalculatorService, ChargeCalculatorService>()
            .AddScoped<IParkingService, ParkingService>();
}