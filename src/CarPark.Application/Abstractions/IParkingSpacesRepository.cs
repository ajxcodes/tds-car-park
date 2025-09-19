using CarPark.Domain;

namespace CarPark.Application.Abstractions;

public interface IParkingSpacesRepository
{
    ParkingSpace Create(ParkingSpace space);
    void Update(ParkingSpace space);
    Task<ParkingSpace> GetFirstAvailable();
    Task<List<ParkingSpace>> GetAll();
}