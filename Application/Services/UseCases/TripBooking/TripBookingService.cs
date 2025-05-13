using System;
using Application.DTOs.TripBooking;
using Application.IServices.UseCases;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services.UseCases;

public class TripBookingService : ITripBookingService
{
    readonly IRepository<TripBooking ,int> _repo;
    //todo: add booking service
    // readonly IBookingService _bookingService;
    public TripBookingService(IRepository<TripBooking ,int> repository){
        _repo = repository;
    }
    public Task<GetTripBookingDTO> CreateTripBookingAsync(CreateTripBookingDTO dto)
    {
        throw new NotImplementedException();
    }

    public Task DeleteTripBookingAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<GetTripBookingDTO>> GetAllTripBookingsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<GetTripBookingDTO> GetTripBookingByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateTripBookingAsync(UpdateTripBookingDTO dto)
    {
        throw new NotImplementedException();
    }
}
