using System;
using Application.DTOs.Booking;
using Domain.Enums;

namespace Application.IServices.UseCases;

public interface IBookingService
{
    public Task<GetBookingDTO> CreateBookingAsync(CreateBookingDTO dto);
    public Task UpdateBookingAsync(UpdateBookingDTO dto);
    public Task ChangeBookingStatusAsync(int id, BookingStatus st);
    public Task ConfirmBookingAsync(int id);
    public Task DenyBookingAsync(int id);
    public Task CancelBookingAsync(int id);
    public Task DeleteBookingAsync(int id);
    Task<IEnumerable<GetBookingDTO>> GetAllBookingsAsync();
    Task<GetBookingDTO> GetBookingByIdAsync(int id);
}
