using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Http;
using AutoMapper;
using Domain.IRepositories;
using Domain.Entities;
using Domain.Enums;
using Application.DTOs.CarBooking;
using Application.IServices.UseCases;


namespace Application.Services.UseCases{

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
                CustomerId = "user1", //todo: change later
                EmployeeId = "emp1" //todo: change later
            };
            try{
                await _bookingRepository.AddAsync(booking);
            }
            catch(Exception ex){
                throw new InvalidOperationException("Error while creating booking " + ex.Message);
            }

    #pragma warning disable CS8602 // Dereference of a possibly null reference.
            // the car is not null
            CarBooking carBooking = new CarBooking{
                BookingId = booking.Id,
                CarId = car.Id,
                PickUpLocation = carBookingDto.PickUpLocation!,
                DropOffLocation = carBookingDto.DropOffLocation!,
                WithDriver = carBookingDto.WithDriver,
                Booking = booking,
                Car = car
            };
    #pragma warning restore CS8602 // Dereference of a possibly null reference.
            try{
                await _carbookingRepository.AddAsync(carBooking);
                await _bookingRepository.SaveAsync();
                await _carbookingRepository.SaveAsync();
            }
            catch(Exception ex){
                throw new InvalidOperationException("Error while creating carbooking " + ex.Message);
            }
            ReturnCarBookingDTO carBookingDTO = _mapper.Map<ReturnCarBookingDTO>(carBooking);
            // = new ReturnCarBookingDTO{
            //     CarId = carBooking.CarId,

            // };
            return carBookingDTO;
            
        }
    }
}