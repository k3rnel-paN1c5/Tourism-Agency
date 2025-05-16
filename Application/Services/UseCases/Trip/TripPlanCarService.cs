using System;
using Application.DTOs.TripPlanCar;
using Domain.IRepositories;
using Domain.Entities;
using Application.IServices.UseCases;
using AutoMapper;
using Microsoft.Extensions.Logging;
namespace Application.Services.UseCases;

public class TripPlanCarService : ITripPlanCarService
{
    IRepository<TripPlanCar, int> _repo;
    private readonly ILogger<TripPlanCarService> _logger;
    //todo:
    // ICarService _carService;
    IMapper _mapper;

    public TripPlanCarService(IRepository<TripPlanCar, int> repository, IMapper mapper, ILogger<TripPlanCarService> logger)
    {
        _repo = repository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<GetTripPlanCarDTO> CreateTripPlanCarAsync(CreateTripPlanCarDTO dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        try
        {
            // if(await _carService.GetCarByIdAsync(dto.TripPlanId) is null){
            //     throw new Exception("Car Not Found");
            // }
            var tripPlanCarEntity = _mapper.Map<TripPlanCar>(dto);
            //todo tripPlanCarEntity.Car = Car
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

    public async Task DeleteTripPlanAsync(int id)
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

    public async Task<IEnumerable<GetTripPlanCarDTO>> GetAllTripsAsync()
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

    public async Task<GetTripPlanCarDTO> GetTripByIdAsync(int id)
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

    public async Task UpdateTripPlanAsync(UpdateTripPlanCarDTO dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        try
        {   //todo
            // if(await _carService.GetCarByIdAsync(dto.TripPlanId) is null){
            //     throw new Exception("Car Not Found");
            // }
            var existingTripPlanCar = await _repo.GetByIdAsync(dto.Id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip plan car with ID {dto.Id} was not found.");

            _mapper.Map(dto, existingTripPlanCar);

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
