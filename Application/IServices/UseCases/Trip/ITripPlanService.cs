using System;

using Application.DTOs.TripPlan;
using Application.DTOs.TripPlanCar;

namespace Application.IServices.UseCases;

public interface ITripPlanService
{
    Task<GetTripPlanDTO> CreateTripPlanAsync(CreateTripPlanDTO dto);
    Task UpdateTripPlanAsync(UpdateTripPlanDTO dto);
    Task UpdateCarOfTripPlanAsync(UpdateTripPlanCarDTO dto);
    Task DeleteTripPlanAsync(int id);
    Task<IEnumerable<GetTripPlanDTO>> GetAllTripPlansAsync();
    Task<GetTripPlanDTO> GetTripPlanByIdAsync(int id);
    Task<GetTripPlanCarDTO> AddCarToTripPlanAsync(CreateTripPlanCarDTO dto);

}
