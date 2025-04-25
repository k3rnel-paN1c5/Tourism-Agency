using Application.DTOs.CarBooking;

namespace Application.IServices.UseCases{
    public interface ICarBookingService
    {
        Task<ReturnCarBookingDTO> CreateBookingAsync(CreateCarBookingDTO carBookingDto);

    }

}
