using Application.DTOs.Booking;
using Application.DTOs.CarBooking;
using Application.IServices.UseCases;
using Application.IServices.UseCases.Car;
using Application.IServices.UseCases.CarBooking;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.Extensions.Logging;
using System;

namespace Application.Services.UseCases
{
    public class CarBookingService : ICarBookingService
    {
        private readonly IRepository<CarBooking, int> _repo;
        private readonly IMapper _mapper;
        private readonly ICarService _carService;
        readonly IBookingService _bookingService;

        public CarBookingService(IRepository<CarBooking, int> repo, IMapper mapper, ICarService carService, IBookingService bookingService)
        {
            _repo = repo;
            _mapper = mapper;
            _carService = carService;
            _bookingService = bookingService;
        }

        public async Task<GetCarBookingDTO> CreateCarBookingAsync(CreateCarBookingDTO dto)
        {
            var tripPlan = await _carService.GetCarByIdAsync(dto.CarId)
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

            await _bookingService.CreateBookingAsync(bookingDto);
            var carBooking = _mapper.Map<CarBooking>(dto);
            await _repo.AddAsync(carBooking);
            await _repo.SaveAsync();
            return _mapper.Map<GetCarBookingDTO>(carBooking);


        }

        public async Task DeleteCarBookingAsync(int id)
        {
            _repo.DeleteByIdAsync(id);
            await _bookingService.DeleteBookingAsync(id);
            await _repo.SaveAsync();
        }

        public async Task<IEnumerable<GetCarBookingDTO>> GetAllCarBookingsAsync()
        {
            var carBookings =await _repo.GetAllAsync();
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
                Status = dto.Status,
                NumOfPassengers = dto.NumOfPassengers,
                CustomerId = dto.CustomerId

            };
            await _bookingService.CreateBookingAsync(booking);
            var carBooking = _mapper.Map<CarBooking>(dto);
            await _repo.AddAsync(carBooking);
            await _repo.SaveAsync();
        }
    }
}
