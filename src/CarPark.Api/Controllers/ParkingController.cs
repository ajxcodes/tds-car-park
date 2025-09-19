using CarPark.Application.Abstractions;
using CarPark.Application.Requests;
using CarPark.Application.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CarPark.Api.Controllers;

[ApiController]
[Route("api/parking")]
public class ParkingController(IParkingService parkingService) : ControllerBase
{
    [HttpPost]
    public async Task<InitialParkingResponse> ParkAsync(ParkingRequest request) =>
        await parkingService.ParkAsync(request);

    [HttpGet]
    public async Task<SpacesResponse> GetSpacesAsync() => await parkingService.GetSpacesAsync();

    [HttpPost("exit")]
    public async Task<ParkingCompletedResponse> ExitAsync(ParkingExitRequest request) =>
        await parkingService.ExitAsync(request);
}