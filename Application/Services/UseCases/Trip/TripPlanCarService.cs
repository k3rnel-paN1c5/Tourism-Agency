using System;
using Application.IServices.UseCase;
using Application.DTOs.TripPlanCar;
namespace Application.Services.UseCases;

public class TripPlanCarService : ITripPlanCarService
{
    public Task<GetTripPlanCarDTO> CreateTripPlanCarAsync(CreateTripPlanCarDTO dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTripPlanAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GetTripPlanCarDTO>> GetAllTripsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<GetTripPlanCarDTO> GetTripByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTripPlanAsync(UpdateTripPlanCarDTO dto)
    {
        throw new NotImplementedException();
    }
}
