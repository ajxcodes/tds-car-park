using CarPark.Application.Abstractions;
using CarPark.Domain;
using CarPark.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace CarPark.Infrastructure.Repositories;

public class ParkingSessionRepository(CarParkDbContext context) : IParkingSessionRepository
{
    public ParkingSession Create(ParkingSession session) => context.Add(session).Entity;
    
    public ParkingSession Update(ParkingSession session) => context.Update(session).Entity;

    public Task<ParkingSession> Get(string vehicleRegistration) =>
        context.Set<ParkingSession>().Include(ps => ps.Vehicle).Include(ps => ps.ParkingSpace)
            .FirstAsync(ps => ps.Vehicle.Registration == vehicleRegistration);
}