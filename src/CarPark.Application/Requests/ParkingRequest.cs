using CarPark.Domain;

namespace CarPark.Application.Requests;

public record ParkingRequest
{
    public required string VehicleReg { get; init; }
    public required VehicleType VehicleType { get; init; }
}