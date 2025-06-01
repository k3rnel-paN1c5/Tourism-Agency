using System;
using Application.DTOs.TripPlanCar;
using Domain.IRepositories;
using Domain.Entities;
using Application.IServices.UseCases;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;
using System.ComponentModel.DataAnnotations;

namespace Application.Services.UseCases;

/// <summary>
/// Provides business logic for managing Trip Plan Car associations.
/// This service handles operations related to cars assigned to specific trip plans.
/// </summary>
public class TripPlanCarService : ITripPlanCarService
{
    private readonly IRepository<TripPlanCar, int> _tripPlanCarRepository;
    private readonly ICarService _carService;
    private readonly IMapper _mapper;
    private readonly ILogger<TripPlanCarService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TripPlanCarService"/> class.
    /// </summary>
    /// <param name="repository">The generic repository for TripPlanCar entities.</param>
    /// <param name="carService">The service for Car-related operations, used for availability checks.</param>
    /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
    /// <param name="logger">The logger for this service.</param>
    public TripPlanCarService(
        IRepository<TripPlanCar, int> repository,
        ICarService carService,
        IMapper mapper,
        ILogger<TripPlanCarService> logger
        )
    {
        _tripPlanCarRepository = repository;
        _carService = carService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<GetTripPlanCarDTO> CreateTripPlanCarAsync(CreateTripPlanCarDTO createTripPlanCarDto)
    {
        if (createTripPlanCarDto is null)
        {
            _logger.LogError("CreateTripPlanCarAsync: Input DTO is null");
            throw new ArgumentNullException(nameof(createTripPlanCarDto), "Trip Plan Car create DTO cannot be null.");
        }
        _logger.LogInformation("Attempting to create trip plan car for TripPlanId: {TripPlanId}, CarId: {CarId}", createTripPlanCarDto.TripPlanId, createTripPlanCarDto.CarId);

        try
        {
            var car = await _carService.GetCarByIdAsync(createTripPlanCarDto.CarId);
            if (car is null)
            {
                _logger.LogError("Car with ID {CarId} not found. Cannot create trip plan car.", createTripPlanCarDto.CarId);
                throw new KeyNotFoundException($"Car with ID {createTripPlanCarDto.CarId} was not found.");
            }
            var availableCars = await _carService.GetAvailableCarsAsync(createTripPlanCarDto.StartDate, createTripPlanCarDto.EndDate);
             if (!availableCars.Any(a => a.Id == car.Id)) 
            {
                _logger.LogWarning("Car '{CarId}' is not available for the period {StartDate} to {EndDate}. Creation failed.", createTripPlanCarDto.CarId, createTripPlanCarDto.StartDate, createTripPlanCarDto.EndDate);
                throw new ValidationException("Car is not available for the specified dates.");
            }

            var tripPlanCarEntity = _mapper.Map<TripPlanCar>(createTripPlanCarDto);

            await _tripPlanCarRepository.AddAsync(tripPlanCarEntity).ConfigureAwait(false);
            await _tripPlanCarRepository.SaveAsync().ConfigureAwait(false);
            _logger.LogInformation("Trip plan car '{Id}' created successfully for TripPlanId: {TripPlanId}, CarId: {CarId}.", tripPlanCarEntity.Id, createTripPlanCarDto.TripPlanId, createTripPlanCarDto.CarId);

            return _mapper.Map<GetTripPlanCarDTO>(tripPlanCarEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while creating a trip plan car. Error: {Message}", ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetTripPlanCarDTO> GetTripPlanCarByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve trip plan car with ID: {TripPlanCarId}", id);
        try
        {
            var tripPlanCar = await _tripPlanCarRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (tripPlanCar is null) {
                _logger.LogError("Trip plan car with ID {id} was not found.", id);
                throw new KeyNotFoundException($"Trip plan car with ID {id} was not found.");
            }

            _logger.LogInformation("Trip plan car '{Id}' retrieved successfully.", id);
            return _mapper.Map<GetTripPlanCarDTO>(tripPlanCar);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while retrieving trip plan car with ID {Id}. Error: {Message}", id, ex.Message  );
            throw;
        }
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<GetTripPlanCarDTO>> GetAllTripPlanCarsAsync()
    {
        _logger.LogInformation("Attempting to retrieve all trip plan cars.");
        try
        {
            var tripPlanCars = await _tripPlanCarRepository.GetAllAsync().ConfigureAwait(false);
            var count = tripPlanCars?.Count() ?? 0;
            _logger.LogInformation("{Count} trip plan cars retrieved.", count);
            return _mapper.Map<IEnumerable<GetTripPlanCarDTO>>(tripPlanCars);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while retrieving all trip plan cars. Error: {Message}", ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateTripPlanCarAsync(UpdateTripPlanCarDTO updateTripPlanCarDto)
    {   
        if (updateTripPlanCarDto is null)
        {
            _logger.LogError("UpdateTripPlanCarAsync: Input DTO is null");
            throw new ArgumentNullException(nameof(updateTripPlanCarDto), "Trip Plan Car update DTO cannot be null.");
        }
        _logger.LogInformation("Attempting to create trip plan car for TripPlanId: {TripPlanId}, CarId: {CarId}", updateTripPlanCarDto.TripPlanId, updateTripPlanCarDto.CarId);

        try
        {
            var existingTripPlanCar = await _tripPlanCarRepository.GetByIdAsync(updateTripPlanCarDto.Id).ConfigureAwait(false);
            if (existingTripPlanCar is null) {
                _logger.LogError("UpdateTripPlanCarAsync: Trip plan car with ID {updateTripPlanCarDto.Id} was not found.", updateTripPlanCarDto.Id);
                throw new KeyNotFoundException($"Trip plan car with ID {updateTripPlanCarDto.Id} was not found.");
            }

            _mapper.Map(updateTripPlanCarDto, existingTripPlanCar); 

            _tripPlanCarRepository.Update(existingTripPlanCar);
            await _tripPlanCarRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip plan car '{Id}' updated successfully.", updateTripPlanCarDto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while updating trip plan car with ID {Id}. Message: {ErrorMessage}", updateTripPlanCarDto.Id, ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteTripPlanCarAsync(int id)
    {
        _logger.LogInformation("Attempting to delete trip plan car with ID: {TripPlanCarId}", id);
        try
        {
            var tripPlanCar = await _tripPlanCarRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (tripPlanCar is null) {
                _logger.LogError("Trip plan car with ID {id} was not found.", id);
                throw new KeyNotFoundException($"Trip plan car with ID {id} was not found.");
                
            }

            _tripPlanCarRepository.Delete(tripPlanCar);
            await _tripPlanCarRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip plan car '{Id}' deleted successfully.", id);
        }
        catch (Exception ex)
        {
           _logger.LogError("Error occurred while deleting trip plan car with ID {Id}. Message: {ErrorMessage}", id, ex.Message);
            throw;
        }
    }




    
}
