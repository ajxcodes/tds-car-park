using System.ComponentModel;
using CarPark.Application.Abstractions;
using CarPark.Domain;
using JetBrains.Annotations;
using Microsoft.Extensions.Configuration;

namespace CarPark.Application.Services;

public class ChargeCalculatorService(IConfiguration configuration) : IChargeCalculatorService
{
    public double Calculate(ParkingSession session)
    {
        if (!session.TimeOut.HasValue) throw new InvalidOperationException("Parking session is not completed");
        var timeSpan = session.TimeOut - session.TimeIn;
        var parkingSessionDurationInMinutes = timeSpan.Value.TotalMinutes;
        return Calculate(parkingSessionDurationInMinutes, session.Vehicle.Type);
    }

    private double Calculate(double parkingSessionDurationInMinutes, VehicleType vehicleType)
    {
        if (!Enum.IsDefined(vehicleType))
            throw new InvalidEnumArgumentException(nameof(vehicleType), (int)vehicleType, typeof(VehicleType));

        var carTypeChargesSection = configuration.GetSection("CarTypeCharges");
        var rateConfigValue = carTypeChargesSection[vehicleType.ToString()];

        if (!double.TryParse(rateConfigValue, out var rate))
            throw new InvalidOperationException($"Invalid {vehicleType} rate value from config");

        var baseCharge = rate * parkingSessionDurationInMinutes;

        var extraCharges = carTypeChargesSection.GetSection("ExtraChargesByTime").Get<List<ExtraChargeRule>>() ?? [];

        var totalExtraCharge = extraCharges
            .Sum(rule => (int)(parkingSessionDurationInMinutes / rule.TimeValue) * rule.Charge);

        return Math.Round(baseCharge + totalExtraCharge, 2);
    }
}

[UsedImplicitly]
internal class ExtraChargeRule
{
    public int TimeValue { get; [UsedImplicitly] set; }
    public double Charge { get; [UsedImplicitly] set; }
}