using CarPark.Application.Requests;
using CarPark.Application.Responses;

namespace CarPark.Application.Abstractions;

public interface IParkingService
{
    Task<InitialParkingResponse> ParkAsync(ParkingRequest request);
    Task<SpacesResponse> GetSpacesAsync();
    Task<ParkingCompletedResponse> ExitAsync(ParkingExitRequest request);
}