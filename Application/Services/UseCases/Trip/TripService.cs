using Application.DTOs.Trip;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.IRepositories;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Application.Utilities;

namespace Application.Services.UseCases;

/// <summary>
/// Provides business logic for Trip management.
/// </summary>
public class TripService : ITripService
{
    private readonly IRepository<Trip, int> _tripRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<TripService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TripService"/> class.
    /// </summary>
    /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
    /// <param name="tripRepository">The repository for Trip entities.</param>
    /// <param name="logger">The logger for this service.</param>
    public TripService(IMapper mapper, IRepository<Trip, int> tripRepository, ILogger<TripService> logger)
    {
        _tripRepository = tripRepository ?? throw new ArgumentNullException(nameof(tripRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc />
    public async Task<GetTripDTO> CreateTripAsync(CreateTripDTO createTripDto)
    {
        _logger.LogInformation("Attempting to create trip: {TripName}", createTripDto.Name);

        if (createTripDto == null)
        {
            _logger.LogError("CreateRegionAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(createTripDto), "Trip creation DTO cannot be null.");
        }
        try
        {
            // Auto-generate slug if not provided
            if (string.IsNullOrWhiteSpace(createTripDto.Slug))
            {
                createTripDto.Slug = SlugHelper.GenerateSlug(createTripDto.Name);
                _logger.LogDebug("Auto-generated slug for trip '{TripName}': {TripSlug}", createTripDto.Name, createTripDto.Slug);
            }
            else
            {   // Normalizing Slug
                createTripDto.Slug = SlugHelper.GenerateSlug(createTripDto.Slug);
                _logger.LogDebug("Normalized provided slug for trip '{TripName}': {TripSlug}", createTripDto.Name, createTripDto.Slug);
            }

            // Check for duplicate name or slug
            var existingTrip = await _tripRepository.GetByPredicateAsync(t =>
                    t.Name!.Equals(createTripDto.Name, StringComparison.CurrentCultureIgnoreCase)
                || t.Slug!.Equals(createTripDto.Slug, StringComparison.CurrentCultureIgnoreCase))
                .ConfigureAwait(false);

            if (existingTrip != null)
            {
                _logger.LogWarning("A trip with the same name or slug already exists. Creation failed.");
                throw new ValidationException("Trip name or slug must be unique.");
            }

            var tripEntity = _mapper.Map<Trip>(createTripDto);
            await _tripRepository.AddAsync(tripEntity).ConfigureAwait(false);
            await _tripRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip '{Name}' (ID: {Id}) created successfully.", tripEntity.Name, tripEntity.Id);

            return _mapper.Map<GetTripDTO>(tripEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while creating a trip. Error: {Message}", ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetTripDTO> GetTripByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve trip with ID: {TripId}", id);
        try
        {
            var trip = await _tripRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (trip == null)
            {
                _logger.LogWarning("Trip with ID {TripId} was not found.", id);
                throw new ArgumentException($"Trip with ID {id} was not found.");
            }

            _logger.LogInformation("Trip '{Name}' (ID: {Id}) retrieved successfully.", trip.Name, id);
            return _mapper.Map<GetTripDTO>(trip);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while retrieving trip with ID {Id}. Error: {Message}", id, ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateTripAsync(UpdateTripDTO updateTripDto)
    {
        _logger.LogInformation("Attempting to update trip with ID: {TripId}", updateTripDto.Id);

        if (updateTripDto == null)
        {
            _logger.LogError("UpdateTripAsync: Input DTO is null for trip ID {TripId}.", updateTripDto?.Id);
            throw new ArgumentNullException(nameof(updateTripDto), "Trip update DTO cannot be null.");
        }

        try
        {
            var existingTrip = await _tripRepository.GetByIdAsync(updateTripDto.Id).ConfigureAwait(false);
            if (existingTrip is null)
            {
                _logger.LogWarning("Trip with ID {TripId} was not found for update.", updateTripDto.Id);
                throw new ArgumentException($"Trip with ID {updateTripDto.Id} was not found.");
            }

            // Auto-generate slug if empty or normalize provided slug
            if (string.IsNullOrWhiteSpace(updateTripDto.Slug))
            {
                updateTripDto.Slug = SlugHelper.GenerateSlug(updateTripDto.Name);
                _logger.LogDebug("Auto-generated slug for trip '{TripName}' during update: {TripSlug}", updateTripDto.Name, updateTripDto.Slug);
            }
            else
            {
                updateTripDto.Slug = SlugHelper.GenerateSlug(updateTripDto.Slug);
                _logger.LogDebug("Normalized provided slug for trip '{TripName}' during update: {TripSlug}", updateTripDto.Name, updateTripDto.Slug);
            }

            if (!string.Equals(updateTripDto.Slug, existingTrip.Slug, StringComparison.OrdinalIgnoreCase))
            {
                var tripWithSameSlug = await _tripRepository.GetAllByPredicateAsync(t => t.Slug!.Equals(updateTripDto.Slug, StringComparison.CurrentCultureIgnoreCase) && t.Id != updateTripDto.Id).ConfigureAwait(false);
                if (tripWithSameSlug != null)
                {
                    _logger.LogWarning("A trip with slug '{Slug}' already exists. Update failed for trip ID {TripId}.", updateTripDto.Slug, updateTripDto.Id);
                    throw new ValidationException($"Trip with slug '{updateTripDto.Slug}' already exists.");
                }
            }

            existingTrip = _mapper.Map<Trip>(updateTripDto);
            _tripRepository.Update(existingTrip);
            await _tripRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip '{Name}' (ID: {Id}) updated successfully.", existingTrip.Name, existingTrip.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while updating trip with ID {Id}. Error: {Message}", updateTripDto.Id, ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetTripDTO>> GetAllTripsAsync()
    {
        _logger.LogInformation("Attempting to retrieve all trips.");
        try
        {
            var trips = await _tripRepository.GetAllAsync().ConfigureAwait(false);
            var tripCount = trips?.Count() ?? 0;
            _logger.LogInformation("Retrieved {Count} trips.", tripCount);
            return _mapper.Map<IEnumerable<GetTripDTO>>(trips);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while retrieving all trips. Error: {Message}", ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetTripDTO>> GetAvailablePublicTripsAsync()
    {
        _logger.LogInformation("Attempting to retrieve all available trips.");
        try
        {
            var trips = await _tripRepository.GetAllByPredicateAsync(t => t.IsAvailable && !t.IsPrivate).ConfigureAwait(false);
            var tripCount = trips?.Count() ?? 0;
            _logger.LogInformation("Retrieved {Count} available trips.", tripCount);
            return _mapper.Map<IEnumerable<GetTripDTO>>(trips);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while retrieving all available trips. Error: {Message}", ex.Message);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task DeleteTripAsync(int id)
    {
        _logger.LogInformation("Attempting to delete trip with ID: {TripId}", id);
        try
        {
            var trip = await _tripRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (trip == null)
            {
                _logger.LogWarning("Trip with ID {TripId} was not found for deletion.", id);
                throw new ArgumentException($"Trip with ID {id} was not found.");
            }

            _tripRepository.Delete(trip);
            await _tripRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip '{Name}' (ID: {Id}) deleted successfully.", trip.Name, id);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while deleting trip with ID {Id}. Error: {Message}", id, ex.Message);
            throw;
        }
    }
}
