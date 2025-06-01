using System;
using Application.DTOs.TripPlan;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.IRepositories;
using Domain.Entities;
using Application.DTOs.TripPlanCar;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using Application.DTOs.CarBooking;

namespace Application.Services.UseCases;

/// <summary>
/// Provides business logic for managing Trip Plans.
/// </summary>
public class TripPlanService : ITripPlanService
{
    private readonly IRepository<TripPlan, int> _tripPlanRepository;
    private readonly IRegionService _regionService;
    private readonly ITripService _tripService;
    private readonly ITripPlanCarService _tripPlanCarService;
    private readonly IMapper _mapper;
    private readonly ILogger<TripPlanService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TripPlanService"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
    /// <param name="tripPlanRepository">The generic repository for TripPlan entities.</param>
    /// <param name="regionService">The service for Region-related operations.</param>
    /// <param name="tripService">The service for Trip-related operations.</param>
    /// <param name="tripPlanCarService">The service for TripPlanCar-related operations.</param>
    /// <param name="logger">The logger for this service.</param>
    public TripPlanService(
        IRepository<TripPlan, int> tripPlanRepository,
        IRegionService regionService,
        ITripService tripService,
        ITripPlanCarService tripPlanCarService,
        IMapper mapper,
        ILogger<TripPlanService> logger
    )
    {
        _tripPlanRepository = tripPlanRepository;
        _regionService = regionService;
        _tripService = tripService;
        _tripPlanCarService = tripPlanCarService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<GetTripPlanDTO> CreateTripPlanAsync(CreateTripPlanDTO createTripPlanDto)
    {

        if (createTripPlanDto is null)
        {
            _logger.LogError("CreateTripPlanAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(createTripPlanDto), "Trip Plan creation DTO cannot be null.");
        }
        _logger.LogInformation("Attempting to create trip plan with trip id: {TripId}", createTripPlanDto.TripId);
        try
        {
            var trip = await _tripService.GetTripByIdAsync(createTripPlanDto.TripId).ConfigureAwait(false);
            if (trip is null)
            {
                _logger.LogError("Trip with Id {TripId} was not found", createTripPlanDto.TripId);
                throw new KeyNotFoundException($"Trip with ID {createTripPlanDto.TripId} was not found.");
            }
            if (!trip.IsAvailable)
            {
                _logger.LogWarning("The chosen Trip is unavailable.");
                throw new ValidationException("This trip is unavailable");
            }

            var region = await _regionService.GetRegionByIdAsync(createTripPlanDto.RegionId).ConfigureAwait(false);
            if (region is null)
            {
                _logger.LogError("Region with Id {RegionId} was not found", createTripPlanDto.RegionId);
                throw new KeyNotFoundException($"Region with ID {createTripPlanDto.TripId} was not found.");
            }

            if (createTripPlanDto.EndDate <= createTripPlanDto.StartDate)
            {
                _logger.LogWarning("Invalid Date: End Date must be after Start Date.");
                throw new ValidationException("End Date must be after Start Date.");
            }

            if (createTripPlanDto.StartDate < DateTime.UtcNow.Date)
            {
                _logger.LogWarning("Invalid Date: Start Date cannot be in the past.");
                throw new ValidationException("Start Date cannot be in the past.");
            }

            var tripPlanEntity = _mapper.Map<TripPlan>(createTripPlanDto);

            await _tripPlanRepository.AddAsync(tripPlanEntity).ConfigureAwait(false);
            await _tripPlanRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip plan '{Id}' created successfully.", tripPlanEntity.Id);

            GetTripPlanDTO getTripPlanDTO = _mapper.Map<GetTripPlanDTO>(tripPlanEntity);

            if (createTripPlanDto.TripPlanCars is not null && createTripPlanDto.TripPlanCars.Count > 0)
            {
                IEnumerable<GetTripPlanCarDTO> tripPlanCars = new HashSet<GetTripPlanCarDTO>();
                _logger.LogInformation("Adding {num} Trip Plan Cars to this Trip Plan", createTripPlanDto.TripPlanCars.Count);
                foreach (var tripPlanCarDto in createTripPlanDto.TripPlanCars)
                {
                    tripPlanCarDto.TripPlanId = tripPlanEntity.Id;
                    tripPlanCarDto.TripPlan = getTripPlanDTO;      // To derive the start and end date of the trip plan car reserevation

                    var createTripPlanCarDto = _mapper.Map<CreateTripPlanCarDTO>(tripPlanCarDto);
                    var carResult = await _tripPlanCarService.CreateTripPlanCarAsync(createTripPlanCarDto);
                    tripPlanCars = tripPlanCars.Append(carResult);
                }
                getTripPlanDTO.TripPlanCars = (ICollection<GetTripPlanCarDTO>)tripPlanCars;
            }

            return getTripPlanDTO;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while creating trip plan. Error: {Message}", ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetTripPlanDTO> GetTripPlanByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve trip plan with ID: {TripPlanId}", id);
        try
        {
            var tripPlan = await _tripPlanRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (tripPlan is null)
            {
                _logger.LogWarning("Trip Plan with ID {TripPlanId} was not found.", id);
                throw new KeyNotFoundException($"Trip Plan with ID {id} was not found.");
            }
            _logger.LogInformation("Trip plan ID: {Id} retrieved successfully.", id);
            return _mapper.Map<GetTripPlanDTO>(tripPlan);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving trip plan with ID {Id}. Error: {Message}", id, ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetTripPlanDTO>> GetAllTripPlansAsync()
    {
        _logger.LogInformation("Attempting to retrieve all trip plans.");
        try
        {
            var plans = await _tripPlanRepository.GetAllAsync().ConfigureAwait(false);
            var planCount = plans?.Count() ?? 0;
            _logger.LogInformation("Retrieved {Count} trip plans.", planCount);
            return _mapper.Map<IEnumerable<GetTripPlanDTO>>(plans);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while retrieving all trip plans. Error: {Message}", ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateTripPlanAsync(UpdateTripPlanDTO updateTripPlanDto)
    {

        if (updateTripPlanDto is null)
        {
            _logger.LogError("UpdateTripPlanAsync: Input DTO is null for trip plan ID {TripId}.", updateTripPlanDto?.Id);
            throw new ArgumentNullException(nameof(updateTripPlanDto), "Trip planupdate DTO cannot be null.");
        }
        _logger.LogInformation("Attempting to update trip Plan with ID: {TripPlanId}", updateTripPlanDto.Id);

        try
        {
            var existingTripPlan = await _tripPlanRepository.GetByIdAsync(updateTripPlanDto.Id).ConfigureAwait(false);

            if (existingTripPlan is null)
            {
                _logger.LogWarning("Trip Plan with ID {TripPlanId} was not found for update.", updateTripPlanDto.Id);
                throw new KeyNotFoundException($"Trip Plan with ID {updateTripPlanDto.Id} was not found.");
            }

            var trip = await _tripService.GetTripByIdAsync(updateTripPlanDto.TripId).ConfigureAwait(false);
            if (trip is null)
            {
                _logger.LogError("Trip with id {id} was not found", updateTripPlanDto.TripId);
                throw new KeyNotFoundException($"Trip with ID {updateTripPlanDto.TripId} was not found.");
            }

            var region = await _regionService.GetRegionByIdAsync(updateTripPlanDto.RegionId).ConfigureAwait(false);
            if (region is null)
            {
                _logger.LogError("Region with id {id} was not found", updateTripPlanDto.RegionId);
                throw new KeyNotFoundException($"Region with ID {updateTripPlanDto.RegionId} was not found.");
            }

            if (updateTripPlanDto.EndDate <= updateTripPlanDto.StartDate)
            {
                _logger.LogWarning("Invalid Date: End Date must be after Start Date.");
                throw new ValidationException("End Date must be after Start Date.");
            }

            if (updateTripPlanDto.StartDate < DateTime.UtcNow.Date)
            {
                _logger.LogWarning("Invalid Date: Start Date cannot be in the past.");
                throw new ValidationException("Start Date cannot be in the past.");
            }

            _mapper.Map(updateTripPlanDto, existingTripPlan);


            _tripPlanRepository.Update(existingTripPlan);
            await _tripPlanRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip plan '{Id}' updated successfully.", existingTripPlan.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while updating trip plan with ID {Id}. Error: {Message}", updateTripPlanDto.Id, ex.Message);
            throw;
        }

    }

    /// <inheritdoc />
    public async Task DeleteTripPlanAsync(int id)
    {
        _logger.LogInformation("Attempting to delete trip Plan with ID: {TripplanId}", id);
        try
        {
            var tripPlan = await _tripPlanRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (tripPlan is null)
            {
                _logger.LogWarning("Trip Plan with ID {TripId} was not found for deletion.", id);
                throw new KeyNotFoundException($"Trip Plan with ID {id} was not found.");
            }

            _tripPlanRepository.Delete(tripPlan);
            await _tripPlanRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip Plan '{Id}' deleted successfully.", tripPlan.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while deleting Trip Plan with ID {Id}. Error: {Message}", id, ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetTripPlanCarDTO> AddCarToTripPlanAsync(CreateTripPlanCarFromTripPlanDTO createTripPlanCarDto)
    {
        if (createTripPlanCarDto is null)
        {
            _logger.LogError("AddCarToTripPlanAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(createTripPlanCarDto), "Trip Plan Car creation DTO cannot be null.");
        }
        _logger.LogInformation("Attempting to add a car {carId} to trip plan {tripPlanId}", createTripPlanCarDto.CarId, createTripPlanCarDto.TripPlanId);
        try
        {
            var tripPlan = await _tripPlanRepository.GetByIdAsync(createTripPlanCarDto.TripPlanId).ConfigureAwait(false);
            if (tripPlan is null)
            {
                _logger.LogWarning("Trip Plan with ID {TripPlanId} was not found for Adding a Car.", createTripPlanCarDto.TripPlanId);
                throw new KeyNotFoundException($"Trip Plan with ID {createTripPlanCarDto.TripPlanId} was not found.");
            }
            var dto = _mapper.Map<CreateTripPlanCarDTO>(createTripPlanCarDto);
            var car = await _tripPlanCarService.CreateTripPlanCarAsync(dto).ConfigureAwait(false);

            _logger.LogInformation("Car added to trip plan '{TripPlanId}' successfuly.", dto.TripPlanId);
            return car;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while adding a car to trip plan '{TripPlanId}'. Error: {Message}", createTripPlanCarDto.TripPlanId, ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task RemoveCarFromTripPlanAsync(int id)
    {
        _logger.LogInformation("Attempting to remove trip plan car with ID: {TripPlanCarId}", id);
        try
        {
            var tripPlanCar = await _tripPlanCarService.GetTripPlanCarByIdAsync(id).ConfigureAwait(false);
            if (tripPlanCar == null)
            {
                _logger.LogWarning("Trip Plan Car with ID {Id} was not found for removal.", id);
                throw new KeyNotFoundException($"Trip Plan Car with ID {id} was not found.");
            }
            var tripPlan = await _tripPlanRepository.GetByIdAsync(tripPlanCar.TripPlanId) ?? throw new InvalidOperationException("Trip Plan Car doesn't belong to a valid trip plan");
            if (tripPlan.PlanCars is null)
            {
                _logger.LogError("RemoveCarFromTripPlanAsync: Trip Plan Car with ID {Id} was not found for removal.", id);
                throw new InvalidOperationException("Deleting a Car from a Trip Plan That does not have cars");
            }
            var carEntityToRemove = _mapper.Map<TripPlanCar>(tripPlanCar);
            if (!tripPlan.PlanCars.Remove(carEntityToRemove))
            {
                _logger.LogWarning("Failed to remove car with ID {Id} from Trip Plan {TripPlanId}'s collection. Car might not have been in the collection.", id, tripPlan.Id);
            }
            await _tripPlanCarService.DeleteTripPlanCarAsync(id).ConfigureAwait(false);
            await _tripPlanRepository.SaveAsync().ConfigureAwait(false);
            _logger.LogInformation("Car removed from trip plan '{TripPlanId}'.", tripPlan.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while removing trip plan car with id {id}. Error: {Message}", id, ex.Message);
            throw;
        }
    }

    public async Task<IEnumerable<GetTripPlanDTO>> GetTripPlansByDateIntervalAsync(DateTime startDate, DateTime endDate)
    {
        if (startDate >= endDate)
        {
            throw new ArgumentException("Start date must be before end date.");
        }

        var tripPlans = await _tripPlanRepository.GetAllByPredicateAsync(tp =>
            
            tp.EndDate > startDate &&
            tp.StartDate < endDate
        );
        return _mapper.Map<IEnumerable<GetTripPlanDTO>>(tripPlans);
    }



}
