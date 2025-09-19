using CarPark.Application.Abstractions;
using CarPark.Infrastructure.Contexts;

namespace CarPark.Infrastructure.UnitOfWork;

public class UnitOfWork(CarParkDbContext context) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        context.SaveChangesAsync(cancellationToken);
}