using CarPark.Domain;

namespace CarPark.Application.Abstractions;

public interface IParkingSessionRepository
{
    ParkingSession Create(ParkingSession session);
    ParkingSession Update(ParkingSession session);
    Task<ParkingSession> Get(string vehicleRegistration);
}