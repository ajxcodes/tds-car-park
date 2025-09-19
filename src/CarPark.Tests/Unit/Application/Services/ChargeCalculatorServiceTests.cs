using CarPark.Application.Abstractions;
using CarPark.Application.Services;
using CarPark.Domain;
using Microsoft.Extensions.Configuration;

namespace CarPark.Tests.Unit.Application.Services;

public class ChargeCalculatorServiceTests
{
    private readonly IConfiguration _configuration;
    private readonly IChargeCalculatorService _sut;

    public ChargeCalculatorServiceTests()
    {
        // Arrange: Set up a mock configuration for all tests
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "CarTypeCharges:SmallCar", "1.5" },
            { "CarTypeCharges:MediumCar", "2.0" },
            { "CarTypeCharges:LargeCar", "2.5" },
            { "CarTypeCharges:ExtraChargesByTime:0:TimeValue", "5" },
            { "CarTypeCharges:ExtraChargesByTime:0:Charge", "1.0" } // Changed to double
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _sut = new ChargeCalculatorService(_configuration);
    }

    [Theory]
    [InlineData(VehicleType.SmallCar, 60, 102.0)] // Base: 60*1.5=90, Extra: (60/5)*1=12, Total: 102
    [InlineData(VehicleType.MediumCar, 30, 66.0)]  // Base: 30*2.0=60, Extra: (30/5)*1=6,  Total: 66
    [InlineData(VehicleType.LargeCar, 10, 27.0)]   // Base: 10*2.5=25, Extra: (10/5)*1=2,  Total: 27
    public void Calculate_ShouldReturnCorrectCharge_WhenSessionIsValid(VehicleType vehicleType, int minutesParked, double expectedCharge)
    {
        // Arrange
        var timeIn = DateTime.UtcNow;
        var session = new ParkingSession
        {
            TimeIn = timeIn,
            Vehicle = new Vehicle { Type = vehicleType },
            ParkingSpace = new ParkingSpace { Number = 1 }
        };
        session.Exit(); // Sets the TimeOut
        // Manually adjust TimeOut for predictable test duration
        typeof(ParkingSession).GetProperty(nameof(ParkingSession.TimeOut))!
            .SetValue(session, timeIn.AddMinutes(minutesParked));

        // Act
        var charge = _sut.Calculate(session);

        // Assert
        charge.ShouldBe(expectedCharge);
    }

    [Fact]
    public void Calculate_ShouldThrowInvalidOperationException_WhenSessionIsNotCompleted()
    {
        // Arrange
        var session = new ParkingSession { TimeIn = DateTime.UtcNow, ParkingSpace = new ParkingSpace { Number = 1 } }; // TimeOut is null

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => _sut.Calculate(session))
            .Message.ShouldBe("Parking session is not completed");
    }

    [Fact]
    public void Calculate_ShouldThrowInvalidOperationException_WhenRateInConfigIsInvalid()
    {
        // Arrange
        var config = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?> { { "CarTypeCharges:SmallCar", "not-a-double" } }).Build();
        var service = new ChargeCalculatorService(config);
        var session = new ParkingSession { Vehicle = new Vehicle { Type = VehicleType.SmallCar }, ParkingSpace = new ParkingSpace(){Number = 1}};
        session.Exit();

        // Act & Assert
        Should.Throw<InvalidOperationException>(() => service.Calculate(session))
            .Message.ShouldBe("Invalid SmallCar rate value from config");
    }
}