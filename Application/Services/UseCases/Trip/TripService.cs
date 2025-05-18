using System;
using Application.DTOs.Trip;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.IRepositories;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Application.Services.UseCases;

public class TripService : ITripService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Trip, int> _tripRepo;
    public TripService(IMapper mapper,  IRepository<Trip, int> repo){
        _mapper = mapper;
        _tripRepo = repo;
    }
    public async Task<GetTripDTO> CreateTripAsync(CreateTripDTO dto)
    {
        if (await _tripRepo.GetByPredicateAsync(t => dto.Slug!.Equals(t.Slug) || dto.Name!.Equals(t.Name)) is not null)
            throw new ValidationException($"Slug and Name should be unique");
        
        var trip = _mapper.Map<Trip>(dto);
        await _tripRepo.AddAsync(trip);
        await _tripRepo.SaveAsync();
        return _mapper.Map<GetTripDTO>(trip);
    }

    public async Task UpdateTripAsync(UpdateTripDTO dto)
    {
        var trip = await _tripRepo.GetByIdAsync(dto.Id)
            ?? throw new Exception($"Trip {dto.Id} not found.");

        // Check slug uniqueness (exclude current ID)
        if ((dto.Slug is not null) && await _tripRepo.GetByPredicateAsync(t => dto.Slug.Equals(t.Slug)) is not null)
            throw new ValidationException($"Trip with slug '{dto.Slug}' already exists.");

        _mapper.Map(dto, trip); 
        _tripRepo.Update(trip);
        await _tripRepo.SaveAsync();
    }

    public async Task DeleteTripAsync(int id)
    {
        _tripRepo.DeleteByIdAsync(id);
        await _tripRepo.SaveAsync();
    }

    public async Task<IEnumerable<GetTripDTO>> GetAllTripsAsync()
    {
        var trips = await _tripRepo.GetAllAsync();
        return _mapper.Map<IEnumerable<GetTripDTO>>(trips);
    }

    public async Task<GetTripDTO> GetTripByIdAsync(int id)
    {
        var trip = await _tripRepo.GetByIdAsync(id)
            ?? throw new Exception($"Trip {id} not found.");
        return _mapper.Map<GetTripDTO>(trip);
    }


}
