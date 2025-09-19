using CarPark.Application.Abstractions;
using CarPark.Application.Requests;
using CarPark.Application.Services;
using CarPark.Domain;

namespace CarPark.Tests.Unit.Application.Services;

public class ParkingServiceTests
{
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly IParkingSessionRepository _sessionRepository = Substitute.For<IParkingSessionRepository>();
    private readonly IParkingSpacesRepository _spaceRepository = Substitute.For<IParkingSpacesRepository>();
    private readonly IChargeCalculatorService _chargeService = Substitute.For<IChargeCalculatorService>();
    private readonly IParkingService _sut;

    public ParkingServiceTests()
    {
        _sut = new ParkingService(_unitOfWork, _sessionRepository, _spaceRepository, _chargeService);
    }

    [Fact]
    public async Task ParkAsync_ShouldReturnParkingResponse_WhenSpaceIsAvailable()
    {
        // Arrange
        var request = new ParkingRequest { VehicleReg = "TEST-123", VehicleType = VehicleType.SmallCar };
        var availableSpace = new ParkingSpace { Id = Guid.NewGuid(), Number = 1 };

        _spaceRepository.GetFirstAvailable().Returns(availableSpace);

        // Act
        var response = await _sut.ParkAsync(request);

        // Assert
        response.ShouldNotBeNull();
        response.VehicleReg.ShouldBe(request.VehicleReg);
        response.SpaceNumber.ShouldBe(availableSpace.Number);
        response.TimeIn.ShouldBeInRange(DateTime.UtcNow.AddSeconds(-5), DateTime.UtcNow.AddSeconds(5));

        // Verify that the space was marked as occupied
        availableSpace.IsOccupied.ShouldBeTrue();
        
        // Verify that the changes were persisted
        _sessionRepository.Received(1).Create(Arg.Is<ParkingSession>(s => s.Vehicle.Registration == request.VehicleReg));
        _spaceRepository.Received(1).Update(availableSpace);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    [Fact]
    public void ParkAsync_ShouldThrowException_WhenNoSpaceIsAvailable()
    {
        // Arrange
        var request = new ParkingRequest { VehicleReg = "TEST-123", VehicleType = VehicleType.LargeCar};
        _spaceRepository.GetFirstAvailable().Returns((ParkingSpace)null!); // Simulate no space found

        // Act & Assert
        Should.Throw<InvalidOperationException>(async () => await _sut.ParkAsync(request));
    }

    [Fact]
    public async Task GetSpacesAsync_ShouldReturnCorrectCounts()
    {
        // Arrange
        var spaces = new List<ParkingSpace>
        {
            new ParkingSpace().Start(),
            new ParkingSpace().Start(),
            new()
        };
        _spaceRepository.GetAll().Returns(spaces);

        // Act
        var response = await _sut.GetSpacesAsync();

        // Assert
        response.OccupiedSpaces.ShouldBe(2);
        response.AvailableSpaces.ShouldBe(1);
    }

    [Fact]
    public async Task ExitAsync_ShouldReturnCompletedResponse_WhenVehicleExists()
    {
        // Arrange
        var request = new ParkingExitRequest { VehicleReg = "EXIT-456" };
        var session = new ParkingSession
        {
            Vehicle = new Vehicle { Registration = request.VehicleReg, Type = VehicleType.MediumCar },
            TimeIn = DateTime.UtcNow.AddHours(-1),
            ParkingSpace = new ParkingSpace()
            {
                Number = 1
            }
        };

        _sessionRepository.Get(request.VehicleReg).Returns(session);
        _chargeService.Calculate(session).Returns(120.0); // 2.0/min * 60 mins

        // Act
        var response = await _sut.ExitAsync(request);

        // Assert
        response.ShouldNotBeNull();
        response.VehicleReg.ShouldBe(request.VehicleReg);
        response.VehicleCharge.ShouldBe(120.0);
        response.TimeOut.ShouldNotBe(DateTime.MinValue);

        // Verify domain object state changes
        session.TimeOut.ShouldNotBeNull();
        session.Charge.ShouldBe(120.0);
        
        // Verify that the changes were persisted
        _sessionRepository.Received(1).Update(session);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }
}