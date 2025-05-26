using System;
using Application.DTOs.Booking;
using Domain.Enums;

namespace Application.IServices.UseCases;

public interface IBookingService
{
    public Task<GetBookingDTO> CreateBookingAsync(CreateBookingDTO dto);
    public Task UpdateBookingAsync(UpdateBookingDTO dto);
    public Task ConfirmBookingAsync(int id);
    public Task DeleteBookingAsync(int id);
    Task<IEnumerable<GetBookingDTO>> GetAllBookingsAsync();
    Task<GetBookingDTO> GetBookingByIdAsync(int id);
}
