namespace CarPark.Domain;

public class ParkingSpace
{
    public Guid Id { get; init; }
    public int Number { get; init; }
    public bool IsOccupied { get; private set; }

    public ParkingSpace Start()
    {
        IsOccupied = true;
        return this;
    }

    public ParkingSpace Exit()
    {
        IsOccupied = false;
        return this;
    }
}