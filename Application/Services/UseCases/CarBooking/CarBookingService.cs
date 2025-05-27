using Application.DTOs.Booking;
using Application.DTOs.CarBooking;
using Application.DTOs.TripBooking;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Application.Services.UseCases
{
    public class CarBookingService : ICarBookingService
    {
        private readonly IRepository<CarBooking, int> _repo;
        private readonly IMapper _mapper;
        private readonly ICarService _carService;
        readonly IBookingService _bookingService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CarBookingService(IRepository<CarBooking, int> repo, IMapper mapper, ICarService carService, IBookingService bookingService, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _mapper = mapper;
            _carService = carService;
            _bookingService = bookingService;
             _httpContextAccessor = httpContextAccessor;
        }

        public async Task<GetCarBookingDTO> CreateCarBookingAsync(CreateCarBookingDTO dto)
        {
            var car = await _carService.GetCarByIdAsync(dto.CarId)
                ?? throw new ArgumentException($"Car with ID {dto.CarId} was not found.");

            if (dto.StartDate < DateTime.UtcNow.Date)
                throw new ArgumentException("Start Date must be in the future.");

            if (dto.EndDate <= dto.StartDate)
                throw new ArgumentException("End Date must be after Start Date.");

            if (dto.NumOfPassengers <= 0)
                throw new ArgumentException("Number of passengers must be greater than zero.");

            CreateBookingDTO bookingDto = new CreateBookingDTO
            {
                BookingType = false,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                NumOfPassengers = dto.NumOfPassengers
            };

            var createdBooking =await _bookingService.CreateBookingAsync(bookingDto);
            var carBooking = _mapper.Map<CarBooking>(dto);

            carBooking.BookingId = createdBooking.Id;

            await _repo.AddAsync(carBooking);
            await _repo.SaveAsync();
            return _mapper.Map<GetCarBookingDTO>(carBooking);

        }

        public async Task DeleteCarBookingAsync(int id)
        {
            await _repo.DeleteByIdAsync(id);
            await _bookingService.DeleteBookingAsync(id);
            await _repo.SaveAsync();
        }

        public async Task<IEnumerable<GetCarBookingDTO>> GetAllCarBookingsAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var userIdClaim = httpContext.User.Claims
               .FirstOrDefault(c => c.Type == "UserId" ||
                                    c.Type == "sub" ||
                                    c.Type == ClaimTypes.NameIdentifier)?.Value;

            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            IEnumerable<CarBooking> carBookings;
            if (role == "Customer")
                carBookings = await _repo.GetAllByPredicateAsync(cb => cb.Booking!.CustomerId == userIdClaim).ConfigureAwait(false);
            else
                carBookings = await _repo.GetAllAsync().ConfigureAwait(false); 
            if (carBookings is not null)
                foreach (var cb in carBookings)
                {
                    cb.Booking = _mapper.Map<Booking>(await _bookingService.GetBookingByIdAsync(cb.BookingId));
                }

            return _mapper.Map<IEnumerable<GetCarBookingDTO>>(carBookings);
        }
      
        public async Task<GetCarBookingDTO> GetCarBookingByIdAsync(int id)
        {
            var carBooking = await _repo.GetByIdAsync(id)
                   ?? throw new Exception($"Car Booking {id} was not found");
            return _mapper.Map<GetCarBookingDTO>(carBooking);
        }

        public async Task UpdateCarBookingAsync(UpdateCarBookingDTO dto)
        {
            CreateBookingDTO booking = new CreateBookingDTO
            {
                BookingType = true,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                NumOfPassengers = dto.NumOfPassengers
            };
            var createdBooking = await _bookingService.CreateBookingAsync(booking);
            var carBooking = _mapper.Map<CarBooking>(dto);
            carBooking.BookingId = createdBooking.Id;
            await _repo.AddAsync(carBooking);
            await _repo.SaveAsync();
        }
    }
}
