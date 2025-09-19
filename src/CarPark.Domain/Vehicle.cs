namespace CarPark.Domain;

public class Vehicle
{
    public Guid Id { get; init; }
    public string Registration { get; init; }
    public VehicleType Type { get; init; }
}