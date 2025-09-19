namespace CarPark.Domain;

public class ParkingSession
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; }

    public DateTime TimeIn { get; init; }

    public DateTime? TimeOut { get; private set; }
    public double? Charge { get; private set; }
    public ParkingSpace ParkingSpace { get; init; }
    public Guid ParkingSpaceId { get; init; }

    public void Exit()
    {
        TimeOut = DateTime.UtcNow;
        ParkingSpace.Exit();
    }

    public void Complete(double charge)
    {
        Charge = charge;
    }
}