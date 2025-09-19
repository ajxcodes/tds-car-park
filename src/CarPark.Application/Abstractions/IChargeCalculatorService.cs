using CarPark.Domain;

namespace CarPark.Application.Abstractions;

public interface IChargeCalculatorService
{
    double Calculate(ParkingSession session);
}