using System;
using Application.DTOs.TripPlanCar;
using Domain.IRepositories;
using Domain.Entities;
using Application.IServices.UseCases;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Application.IServices.UseCases.Car;
namespace Application.Services.UseCases;

public class TripPlanCarService : ITripPlanCarService
{
    IRepository<TripPlanCar, int> _repo;
    ICarService _carService;
    private readonly ILogger<TripPlanCarService> _logger;
    IMapper _mapper;

    public TripPlanCarService(IRepository<TripPlanCar, int> repository, ICarService carService, IMapper mapper, ILogger<TripPlanCarService> logger)
    {
        _repo = repository;
        _carService = carService;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<GetTripPlanCarDTO> CreateTripPlanCarAsync(CreateTripPlanCarDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        try
        {
            var car = await _carService.GetCarByIdAsync(dto.TripPlanId); 
            var tripPlanCarEntity = _mapper.Map<TripPlanCar>(dto);
            var tripPlan = dto.TripPlanCarDTO ?? throw new Exception("Trip plan associated with this car cannot be null");
            var availableCars = await _carService.GetAvailableCarsAsync(tripPlan.StartDate , tripPlan.EndDate);
            if(!availableCars.Contains(car)){
                throw new InvalidDataException("Car is not available");
            }
            await _repo.AddAsync(tripPlanCarEntity).ConfigureAwait(false);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip plan car '{Id}' created successfully.", tripPlanCarEntity.Id);

            return _mapper.Map<GetTripPlanCarDTO>(tripPlanCarEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a trip plan car.");
            throw;
        }
    }

    public async Task DeleteTripPlanCarAsync(int id)
    {
        try
        {
            var tripPlanCar = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip plan car with ID {id} was not found.");

            await _repo.DeleteByIdAsync(id).ConfigureAwait(false);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip plan car '{Id}' deleted successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting trip plan car with ID {Id}.", id);
            throw;
        }
    }

    public async Task<IEnumerable<GetTripPlanCarDTO>> GetAllTripPlanCarsAsync()
    {
         try
        {
            var tripPlanCars = await _repo.GetAllAsync().ConfigureAwait(false);
            _logger.LogDebug("{Count} trip plan cars retrieved.", tripPlanCars?.Count() ?? 0);
            return _mapper.Map<IEnumerable<GetTripPlanCarDTO>>(tripPlanCars);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all trip plan cars.");
            throw;
        }
    }

    public async Task<GetTripPlanCarDTO> GetTripPlanCarByIdAsync(int id)
    {
        try
        {
            var tripPlanCar = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip plan car with ID {id} was not found.");

            _logger.LogDebug("Trip plan car '{Id}' retrieved successfully.", id);
            return _mapper.Map<GetTripPlanCarDTO>(tripPlanCar);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving trip plan car with ID {Id}.", id);
            throw;
        }
    }

    public async Task UpdateTripPlanCarAsync(UpdateTripPlanCarDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        try
        {   
            var existingTripPlanCar = await _repo.GetByIdAsync(dto.Id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip plan car with ID {dto.Id} was not found.");
            var car = await _carService.GetCarByIdAsync(dto.TripPlanId);

            var tripPlan = dto.TripPlanDTO ?? throw new Exception("Updating a trip Plan Car for a trip Plan that doesn't exist");
            var availableCars = await _carService.GetAvailableCarsAsync(tripPlan.StartDate , tripPlan.EndDate);
            if(!availableCars.Contains(car)){
                throw new InvalidDataException("Car is not available");
            }
            existingTripPlanCar = _mapper.Map<TripPlanCar>(dto);

            existingTripPlanCar = _mapper.Map<TripPlanCar>(dto);

            _repo.Update(existingTripPlanCar);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip plan car '{Id}' updated successfully.", dto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating trip plan car with ID {Id}.", dto.Id);
            throw;
        }
    }
}
