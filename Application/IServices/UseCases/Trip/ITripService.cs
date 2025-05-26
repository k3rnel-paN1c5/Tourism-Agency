using System;
using Application.DTOs.Trip;

namespace Application.IServices.UseCases;

public interface ITripService
{
    Task<GetTripDTO> CreateTripAsync(CreateTripDTO dto);
    Task UpdateTripAsync(UpdateTripDTO dto);
    Task DeleteTripAsync(int id);
    Task<IEnumerable<GetTripDTO>> GetAllTripsAsync();
    Task<IEnumerable<GetTripDTO>> GetAvailableTripsAsync();
    Task<GetTripDTO> GetTripByIdAsync(int id);
}
