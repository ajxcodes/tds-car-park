namespace CarPark.Application.Requests;

public record ParkingExitRequest
{
    public required string VehicleReg { get; init; }
}