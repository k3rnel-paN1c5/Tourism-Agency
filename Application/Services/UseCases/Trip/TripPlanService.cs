using System;
using Application.DTOs.TripPlan;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.IRepositories;
using Domain.Entities;
using Application.DTOs.TripPlanCar;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
namespace Application.Services.UseCases;

public class TripPlanService : ITripPlanService
{
    private readonly IMapper _mapper;
    private readonly IRepository<TripPlan, int> _repo;
    private readonly IRegionService _regionService;
    private readonly ITripService _tripService;
    private readonly ITripPlanCarService _tripPlanCarService;
    private readonly ILogger<TripPlanService> _logger;
    public TripPlanService(
        IMapper mapper, 
        IRepository<TripPlan, int> repo, 
        IRegionService regionService, 
        ITripService tripService, 
        ITripPlanCarService carService, 
        ILogger<TripPlanService> logger
    )
    {
        _mapper = mapper;
        _repo = repo;
        _regionService = regionService;
        _tripService = tripService;
        _tripPlanCarService = carService;
        _logger = logger;
    }
    public async Task<GetTripPlanDTO> CreateTripPlanAsync(CreateTripPlanDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        try
        {
            var trip = await _tripService.GetTripByIdAsync(dto.TripId).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip with ID {dto.TripId} was not found.");

            var region = await _regionService.GetRegionByIdAsync(dto.RegionId).ConfigureAwait(false)
                ?? throw new ArgumentException($"Region with ID {dto.RegionId} was not found.");

            if (dto.EndDate <= dto.StartDate)
            {
                throw new ValidationException("End Date must be after Start Date.");
            }

            if (dto.StartDate < DateTime.UtcNow.Date)
            {
                throw new ValidationException("Start Date cannot be in the past.");
            }

            var tripPlanEntity = _mapper.Map<TripPlan>(dto);

            //! Don't do that, it caueses a problem 
            // // Map related entities
            // tripPlanEntity.Trip = _mapper.Map<Trip>(trip);
            // tripPlanEntity.Region = _mapper.Map<Region>(region);
            
            await _repo.AddAsync(tripPlanEntity).ConfigureAwait(false);
            await _repo.SaveAsync().ConfigureAwait(false);
            if (dto.TripPlanCars != null && dto.TripPlanCars.Count > 0)
            {
                tripPlanEntity.PlanCars = new HashSet<TripPlanCar>();
                foreach (var carDto in dto.TripPlanCars)
                {
                    // carDto.TripPlanCarDTO = _mapper.Map<GetTripPlanDTO>(tripPlanEntity);
                    carDto.StartDate = tripPlanEntity.StartDate;
                    carDto.EndDate = tripPlanEntity.EndDate;
                    carDto.TripPlanId = tripPlanEntity.Id;
                    var carResult = await _tripPlanCarService.CreateTripPlanCarAsync(carDto);
                    tripPlanEntity.PlanCars.Add(_mapper.Map<TripPlanCar>(carResult));
                }
            }


            _logger.LogInformation("Trip plan '{Id}' created successfully.", tripPlanEntity.Id);

            return _mapper.Map<GetTripPlanDTO>(tripPlanEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating trip plan.");
            throw;
        }      
    }
    public async Task UpdateTripPlanAsync(UpdateTripPlanDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        try
        {
            var existingTripPlan = await _repo.GetByIdAsync(dto.Id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip plan with ID {dto.Id} was not found.");

            var trip = await _tripService.GetTripByIdAsync(dto.TripId).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip with ID {dto.TripId} was not found.");

            var region = await _regionService.GetRegionByIdAsync(dto.RegionId).ConfigureAwait(false)
                ?? throw new ArgumentException($"Region with ID {dto.RegionId} was not found.");

            if (dto.EndDate <= dto.StartDate)
            {
                throw new ValidationException("End Date must be after Start Date.");
            }

            if (dto.StartDate < DateTime.UtcNow.Date)
            {
                throw new ValidationException("Start Date cannot be in the past.");
            }

            _mapper.Map(dto, existingTripPlan);


            _repo.Update(existingTripPlan);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip plan '{Id}' updated successfully.", dto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating trip plan with ID {Id}.", dto.Id);
            throw;
        }

    }

    public async Task UpdateCarOfTripPlanAsync(UpdateTripPlanCarDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        try
        {
            var tripPlan = await _repo.GetByIdAsync(dto.TripPlanId)  ?? throw new ArgumentException($"Trip Plan with ID {dto.TripPlanId} was not found.");
            dto.StartDate = tripPlan.StartDate;
            dto.EndDate = tripPlan.EndDate;
            await _tripPlanCarService.UpdateTripPlanCarAsync(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating a car in trip plan with ID {Id}.", dto.TripPlanId);
            throw;
        }
    }
    public async Task DeleteTripPlanAsync(int id)
    {
        try
        {
            var tripPlan = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip Plan with ID {id} was not found.");

            _repo.Delete(tripPlan);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip Plan '{Id}' deleted successfully.", tripPlan.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting Trip Plan with ID {Id}.", id);
            throw;
        }
    }

    public async Task<IEnumerable<GetTripPlanDTO>> GetAllTripPlansAsync()
    {
        try
        {
            var plans = await _repo.GetAllAsync().ConfigureAwait(false);
            _logger.LogDebug("{Count} trip plans retrieved.", plans?.Count() ?? 0);
            return _mapper.Map<IEnumerable<GetTripPlanDTO>>(plans);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all trip plans.");
            throw;
        }
    }

    public async Task<GetTripPlanDTO> GetTripPlanByIdAsync(int id)
    {
        try
        {
            var tripPlan = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip plan with ID {id} was not found.");

            _logger.LogDebug("Trip plan '{Id}' retrieved successfully.", id);
            return _mapper.Map<GetTripPlanDTO>(tripPlan);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving trip plan with ID {Id}.", id);
            throw;
        }
    }

    public async Task<GetTripPlanCarDTO> AddCarToTripPlanAsync(CreateTripPlanCarDTO dto)
    {
        try
        {
            var tripPlan = await _repo.GetByIdAsync(dto.TripPlanId).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip plan with ID {dto.TripPlanId} was not found.");
            dto.StartDate = tripPlan.StartDate;
            dto.EndDate = tripPlan.EndDate;
            var car = await _tripPlanCarService.CreateTripPlanCarAsync(dto).ConfigureAwait(false);
            tripPlan.PlanCars ??= new HashSet<TripPlanCar>();
            tripPlan.PlanCars.Add(_mapper.Map<TripPlanCar>(car));
            _logger.LogInformation("Car added to trip plan '{TripPlanId}'.", dto.TripPlanId);
            return car;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while adding a car to trip plan '{TripPlanId}'.", dto.TripPlanId);
            throw;
        }
    }
    public async Task RemoveCarFromTripPlanAsync(int id)
    {
        try
        {
            var tripPlanCar = await _tripPlanCarService.GetTripPlanCarByIdAsync(id).ConfigureAwait(false);
            var tripPlan = await _repo.GetByIdAsync(tripPlanCar.TripPlanId) ?? throw new InvalidOperationException("Trip Plan Car doesn't belong to a valid trip plan");
            if (tripPlan.PlanCars is null)
                throw new InvalidOperationException("Deleting a Car from a Trip Plan That does not have cars");
            tripPlan.PlanCars.Remove(_mapper.Map<TripPlanCar>(tripPlanCar));
            await _tripPlanCarService.DeleteTripPlanCarAsync(id).ConfigureAwait(false);
            _logger.LogInformation("Car removed from trip plan '{TripPlanId}'.", tripPlan.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while removing trip plan car with id {id}", id);
            throw;
        }
    }

}
