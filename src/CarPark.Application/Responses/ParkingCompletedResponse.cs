using CarPark.Domain;
using JetBrains.Annotations;

namespace CarPark.Application.Responses;

public record ParkingCompletedResponse(
    string VehicleReg,
    double VehicleCharge,
    [UsedImplicitly] DateTime TimeIn,
    DateTime TimeOut)
{
    public static ParkingCompletedResponse FromDomain(ParkingSession session) =>
        new ParkingCompletedResponse(session.Vehicle.Registration, session.Charge.GetValueOrDefault(), session.TimeIn,
            session.TimeOut.GetValueOrDefault());
}