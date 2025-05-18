
using Application.DTOs.CarBooking;
using System;


namespace Application.IServices.UseCases.CarBooking
{
    public interface ICarBookingService
    {
        Task<GetCarBookingDTO> CreateCarBookingAsync(CreateCarBookingDTO dto);
        Task UpdateCarBookingAsync(UpdateCarBookingDTO dto);
        Task DeleteCarBookingAsync(int id);
        Task<IEnumerable<GetCarBookingDTO>> GetAllCarBookingsAsync();
        Task<GetCarBookingDTO> GetCarBookingByIdAsync(int id);
    }
}
