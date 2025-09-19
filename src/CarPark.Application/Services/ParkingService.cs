using CarPark.Application.Abstractions;
using CarPark.Application.Requests;
using CarPark.Application.Responses;
using CarPark.Domain;

namespace CarPark.Application.Services;

public class ParkingService(
    IUnitOfWork unitOfWork,
    IParkingSessionRepository sessionRepository, 
    IParkingSpacesRepository spaceRepository, 
    IChargeCalculatorService chargeService) : IParkingService
{
    public async Task<InitialParkingResponse> ParkAsync(ParkingRequest request)
    {        
        var parkingSpace = await spaceRepository.GetFirstAvailable();
        if (parkingSpace is null)
        {
            throw new InvalidOperationException("No available parking spaces.");
        }

        var vehicle = new Vehicle
        {
            Id = Guid.CreateVersion7(),
            Registration = request.VehicleReg,
            Type = request.VehicleType
        };

        var parkingSession = new ParkingSession
        {
            Id = Guid.CreateVersion7(),
            VehicleId = vehicle.Id,
            Vehicle = vehicle,
            ParkingSpaceId = parkingSpace.Id,
            ParkingSpace = parkingSpace,
            TimeIn = DateTime.UtcNow
        };

        sessionRepository.Create(parkingSession);

        parkingSpace.Start();
        spaceRepository.Update(parkingSpace);

        await unitOfWork.SaveChangesAsync();

        return InitialParkingResponse.FromDomain(parkingSession);
    }

    public async Task<SpacesResponse> GetSpacesAsync()
    {
        var spaces = await spaceRepository.GetAll();
        return SpacesResponse.FromDomain(spaces);
    }

    public async Task<ParkingCompletedResponse> ExitAsync(ParkingExitRequest request)
    {
        var parkingSession = await sessionRepository.Get(request.VehicleReg);
        parkingSession.Exit();
        var charge = chargeService.Calculate(parkingSession);
        parkingSession.Complete(charge);
        sessionRepository.Update(parkingSession);
        await unitOfWork.SaveChangesAsync();
        return ParkingCompletedResponse.FromDomain(parkingSession);
    }
}