using CarPark.Application.Abstractions;
using CarPark.Domain;
using CarPark.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CarPark.Infrastructure.Repositories;

public class ParkingSpaceRepository(CarParkDbContext context) : IParkingSpacesRepository
{
    public ParkingSpace Create(ParkingSpace space) => context.Add(space).Entity;

    public void Update(ParkingSpace space) => context.Update(space);

    public Task<ParkingSpace> GetFirstAvailable() =>
        context.Set<ParkingSpace>()
            .OrderBy(ps => ps.Number)
            .Where(ps => !ps.IsOccupied)
            .FirstAsync();

    public Task<List<ParkingSpace>> GetAll() =>
        context.Set<ParkingSpace>()
            .OrderBy(ps => ps.Number)
            .ToListAsync();

}