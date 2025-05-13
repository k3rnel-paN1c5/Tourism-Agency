using System;
using Application.DTOs.TripPlanCar;
using Domain.IRepositories;
using Domain.Entities;
using Application.IServices.UseCases;
using AutoMapper;
namespace Application.Services.UseCases;

public class TripPlanCarService : ITripPlanCarService
{
    IRepository<TripPlanCar, int> _repo;
    // ICarService _carService;
    IMapper _mapper;

    public TripPlanCarService(IRepository<TripPlanCar, int> repository, IMapper mapper)
    {
        _repo = repository;
        _mapper = mapper;
    }
    public async Task<GetTripPlanCarDTO> CreateTripPlanCarAsync(CreateTripPlanCarDTO dto)
    {
        // if(await _carService.GetCarByIdAsync(dto.TripPlanId) is null){
        //     throw new Exception("Car Not Found");
        // }
        TripPlanCar tripPlanCar = _mapper.Map<TripPlanCar>(dto);
        await _repo.AddAsync(tripPlanCar);
        await _repo.SaveAsync();
        return _mapper.Map<GetTripPlanCarDTO>(tripPlanCar);
    }

    public async Task DeleteTripPlanAsync(int id)
    {
        if(await _repo.GetByIdAsync(id) is null){
            throw new Exception("Trip Plan Car Not Found");
        }
        _repo.DeleteByIdAsync(id);
        await _repo.SaveAsync();
    }

    public async Task<IEnumerable<GetTripPlanCarDTO>> GetAllTripsAsync()
    {
        var tripPlanCars = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<GetTripPlanCarDTO>>(tripPlanCars);
    }

    public async Task<GetTripPlanCarDTO> GetTripByIdAsync(int id)
    {
        var tripPlanCar = await _repo.GetByIdAsync(id)
            ?? throw new Exception($"Trip Plan Car {id} was not found");
        return _mapper.Map<GetTripPlanCarDTO>(tripPlanCar);
    }

    public async Task UpdateTripPlanAsync(UpdateTripPlanCarDTO dto)
    {
        // if(await _carService.GetCarByIdAsync(dto.TripPlanId) is null){
        //     throw new Exception("Car Not Found");
        // }
        TripPlanCar tripPlanCar = _mapper.Map<TripPlanCar>(dto);
        _repo.Update(tripPlanCar);
        await _repo.SaveAsync();
    }
}
