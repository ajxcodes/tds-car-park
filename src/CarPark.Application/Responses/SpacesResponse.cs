using CarPark.Domain;

namespace CarPark.Application.Responses;

public record SpacesResponse(int AvailableSpaces, int OccupiedSpaces)
{
    public static SpacesResponse FromDomain(List<ParkingSpace> spaces)
        => new(AvailableSpaces: spaces.Count(x => !x.IsOccupied),
            OccupiedSpaces: spaces.Count(x => x.IsOccupied));
}