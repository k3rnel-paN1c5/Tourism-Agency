using System;
using BusinessLogic.IServices;
using DataAccess.Repositories.IRepositories;
using Microsoft.AspNetCore.Http;
using DTO.CarBooking;
using DataAccess.Repositories;
using DataAccess.Entities;
using DataAccess.Entities.Enums;
using AutoMapper;
using System.Numerics;
using Microsoft.AspNetCore.Http.Internal;

namespace BusinessLogic.Services;

public class CarBookingService : ICarBookingService
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IRepository<CarBooking, int> _carbookingRepository;
    private readonly ICarRepository _carRepository;
    // private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper; // AutoMapper instance
    public CarBookingService(IBookingRepository bookingRepository, ICarRepository carRepository, IRepository<CarBooking, int> carbookingRepository,  IMapper mapper )//,IHttpContextAccessor httpContextAccessor)
    {
        _bookingRepository = bookingRepository;
        _carRepository = carRepository;
        // _httpContextAccessor = httpContextAccessor;
        _carbookingRepository  = carbookingRepository;
        _mapper = mapper;
    }
    public async Task<ReturnCarBookingDTO> CreateBookingAsync(CreateCarBookingDTO carBookingDto)
    {
        if(carBookingDto.StartDate < DateTime.UtcNow  || carBookingDto.EndDate <= carBookingDto.StartDate)
            throw new InvalidOperationException("Invalid date range");
        
        var car = await _carRepository.GetByIdAsync(carBookingDto.CarId) ?? throw new InvalidOperationException("Car is not found.");

        var availableCars = await _carRepository.GetAvailableCarsAsync(carBookingDto.StartDate, carBookingDto.EndDate);
       
       if(!availableCars.Contains(car.Id))
            throw new InvalidOperationException("Car is not available.");
        

        // var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst("UserId");
        // if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
        // {
        //     throw new UnauthorizedAccessException("User ID not found.");
        // }
        // var customerId = userIdClaim.Value; 

        Booking booking = new Booking{
            BookingType = false,
            StartDate = carBookingDto.StartDate,
            EndDate = carBookingDto.EndDate,    
            Status = BookingStatus.Pending,
            NumOfPassengers = carBookingDto.NumOfPassengers,
            CustomerId = "1",
        };
        await _bookingRepository.AddAsync(booking);

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        // the car is not null
        CarBooking carBooking = new CarBooking{
            // Booking.BookingType = BookingStatus.Pending,
            // Booking.StartDaate = carBookingDto.
            BookingId = booking.Id,
            CarId = car.Id,
            PickUpLocation = carBookingDto.PickUpLocation,
            DropOffLocation = carBookingDto.DropOffLocation,
            WithDriver = carBookingDto.WithDriver,
            Booking = booking,
            Car = car
        };
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        await _carbookingRepository.AddAsync(carBooking);
        ReturnCarBookingDTO carBookingDTO = _mapper.Map<ReturnCarBookingDTO>(carBooking);
        // = new ReturnCarBookingDTO{
        //     CarId = carBooking.CarId,

        // };
        return carBookingDTO;
        
    }
}
