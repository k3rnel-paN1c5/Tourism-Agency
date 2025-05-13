using System;
using Application.DTOs.Booking;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;

namespace Application.Services.UseCases;

public class BookingService : IBookingService
{
    IRepository<Booking, int> _repo;
    IMapper _mapper;
    public BookingService(IRepository<Booking, int> repository, IMapper mapper)
    {
        _repo = repository;
        _mapper = mapper;
    }
    public async Task<GetBookingDTO> CreateBookingAsync(CreateBookingDTO dto)
    {
        if (dto.StartDate < DateTime.UtcNow || dto.StartDate > dto.EndDate)
            throw new Exception("Invalid Date");
        if (dto.NumOfPassengers <= 0)
            throw new Exception("Number Of Passengers Has to be greater than 0");
        var booking = _mapper.Map<Booking>(dto);
        await _repo.AddAsync(booking);
        await _repo.SaveAsync();
        return _mapper.Map<GetBookingDTO>(booking);

    }

    public async Task DeleteBookingAsync(int id)
    {
        var booking = await _repo.GetByIdAsync(id) 
            ?? throw new Exception($"Booking {id} not found");
        _repo.Delete(booking);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<GetBookingDTO>> GetAllBookingsAsync()
    {
        var bookings = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<GetBookingDTO>>(bookings);
    }

    public async Task<GetBookingDTO> GetBookingByIdAsync(int id)
    {
        var booking = await _repo.GetByIdAsync(id);
        return _mapper.Map<GetBookingDTO>(booking);
    }

    public async Task UpdateBookingAsync(UpdateBookingDTO dto)
    {
        var booking = await _repo.GetByIdAsync(dto.Id) 
            ?? throw new Exception($"Booking {dto.Id} not found.");
        if (dto.StartDate < DateTime.UtcNow || dto.StartDate > dto.EndDate)
            throw new Exception("Invalid Date");
        if (dto.NumOfPassengers <= 0)
            throw new Exception("Number Of Passengers Has to be greater than 0");
            

        _repo.Update(_mapper.Map<Booking>(dto));
        await _repo.SaveAsync();
    }
}
