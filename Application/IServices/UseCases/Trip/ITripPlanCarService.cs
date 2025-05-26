using System;
using Application.DTOs.TripPlanCar;
namespace Application.IServices.UseCases;

public interface ITripPlanCarService
{
    Task<GetTripPlanCarDTO> CreateTripPlanCarAsync(CreateTripPlanCarDTO dto);
    Task UpdateTripPlanCarAsync(UpdateTripPlanCarDTO dto);
    Task DeleteTripPlanCarAsync(int id);
    Task<IEnumerable<GetTripPlanCarDTO>> GetAllTripPlanCarsAsync();
    Task<GetTripPlanCarDTO> GetTripPlanCarByIdAsync(int id);

}
