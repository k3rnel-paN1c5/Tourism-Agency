using System;

using Application.DTOs.TripPlan;

namespace Application.IServices.UseCases;

public interface ITripPlanService
{
    Task<GetTripPlanDTO> CreateTripPlanAsync(CreateTripPlanDTO dto);
    Task UpdateTripPlanAsync(UpdateTripPlanDTO dto);
    Task DeleteTripPlanAsync(int id);
    Task<IEnumerable<GetTripPlanDTO>> GetAllTripsAsync();
    Task<GetTripPlanDTO> GetTripByIdAsync(int id);

}
