using System;
using DTO.CarBooking;
namespace BusinessLogic.IServices;

public interface ICarBookingService
{
    Task<ReturnCarBookingDTO> CreateBookingAsync(CreateCarBookingDTO carBookingDto);

}
