using System;
using Application.DTOs.TripBooking;
namespace Application.IServices.UseCases;

public interface ITripBookingService
{   
    Task<GetTripBookingDTO> CreateTripBookingAsync(CreateTripBookingDTO dto);
    Task UpdateTripBookingAsync(UpdateTripBookingDTO dto);
    Task DeleteTripBookingAsync(int id);
    Task<IEnumerable<GetTripBookingDTO>> GetAllTripBookingsAsync();
    Task<GetTripBookingDTO> GetTripBookingByIdAsync(int id);

}
