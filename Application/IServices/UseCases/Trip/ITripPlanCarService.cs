using System;
using Application.DTOs.TripPlanCar;
namespace Application.IServices.UseCase;

public interface ITripPlanCarService
{
    Task<GetTripPlanCarDTO> CreateTripPlanCarAsync(CreateTripPlanCarDTO dto);
    Task UpdateTripPlanAsync(UpdateTripPlanCarDTO dto);
    Task DeleteTripPlanAsync(int id);
    Task<IEnumerable<GetTripPlanCarDTO>> GetAllTripsAsync();
    Task<GetTripPlanCarDTO> GetTripByIdAsync(int id);
}
