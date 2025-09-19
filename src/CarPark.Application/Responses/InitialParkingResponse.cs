using CarPark.Domain;

namespace CarPark.Application.Responses;

public record InitialParkingResponse(string VehicleReg, int SpaceNumber, DateTime TimeIn)
{
    public static InitialParkingResponse FromDomain(ParkingSession session) =>
        new(session.Vehicle.Registration, session.ParkingSpace.Number, session.TimeIn);
}