using System;
using Application.DTOs.Booking;
using Application.DTOs.TripBooking;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services.UseCases;

public class TripBookingService : ITripBookingService
{
    readonly IRepository<TripBooking ,int> _repo;
    readonly IBookingService _bookingService;
    readonly ITripPlanService _tripPlanService;
    readonly IMapper _mapper;
    public TripBookingService(IRepository<TripBooking ,int> repository, IBookingService bookingService, ITripPlanService tripPlanService, IMapper mapper){
        _repo = repository;
        _bookingService = bookingService;
        _tripPlanService = tripPlanService;
        _mapper = mapper;
    }
    public async Task<GetTripBookingDTO> CreateTripBookingAsync(CreateTripBookingDTO dto)
    {
        try
        {
            await _tripPlanService.GetTripPlanByIdAsync(dto.TripPlanId);
        }
        catch (System.Exception)
        {
            throw new Exception("Trip Plan Was Not Found");
        }
      
        CreateBookingDTO booking = new CreateBookingDTO{
            BookingType = true,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status,
            NumOfPassengers  = dto.NumOfPassengers,
            CustomerId = dto.CustomerId,
        };
        await _bookingService.CreateBookingAsync(booking);
        var tripBooking = _mapper.Map<TripBooking>(dto);
        await _repo.AddAsync(tripBooking);
        await _repo.SaveAsync();
        return _mapper.Map<GetTripBookingDTO>(tripBooking);

    }

    public async Task DeleteTripBookingAsync(int id)
    {
        _repo.DeleteByIdAsync(id);
        await  _bookingService.DeleteBookingAsync(id);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<GetTripBookingDTO>> GetAllTripBookingsAsync()
    {
        var tripBookings = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<GetTripBookingDTO>>(tripBookings);
    }

    public async Task<GetTripBookingDTO> GetTripBookingByIdAsync(int id)
    {
        var tripBooking = await _repo.GetByIdAsync(id);
        return _mapper.Map<GetTripBookingDTO>(tripBooking);
        
    }

    public async Task UpdateTripBookingAsync(UpdateTripBookingDTO dto)
    {
      
        CreateBookingDTO booking = new CreateBookingDTO{
            BookingType = true,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Status = dto.Status,
            NumOfPassengers  = dto.NumOfPassengers,
            CustomerId = dto.CustomerId,
        };
        await _bookingService.CreateBookingAsync(booking);
        var tripBooking = _mapper.Map<TripBooking>(dto);
        await _repo.AddAsync(tripBooking);
        await _repo.SaveAsync();
    }
}
