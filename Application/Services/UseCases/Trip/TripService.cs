using System;
using Application.DTOs.Trip;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.IRepositories;
using Domain.Entities;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Logging;
using Application.Utilities;

namespace Application.Services.UseCases;

public class TripService : ITripService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Trip, int> _repo;
     private readonly ILogger<TripService> _logger;
    public TripService(IMapper mapper,  IRepository<Trip, int> repo, ILogger<TripService> logger){
        _mapper = mapper;
        _repo = repo;
        _logger = logger;
    }
    public async Task<GetTripDTO> CreateTripAsync(CreateTripDTO dto)
    {
        if (dto == null)
        {
            _logger.LogError("CreateRegionAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(dto));
        }
        try
        {
            // Auto-generate slug if not provided
            if (string.IsNullOrWhiteSpace(dto.Slug))
            {
                dto.Slug = SlugHelper.GenerateSlug(dto.Name);
            }
            // Check for duplicate name or slug
            var existingTrip = await _repo.GetByPredicateAsync(t =>
                t.Name == dto.Name || t.Slug == dto.Slug).ConfigureAwait(false);

            if (existingTrip != null)
            {
                _logger.LogWarning("A trip with the same name or slug already exists.");
                throw new ValidationException("Trip name or slug must be unique.");
            }

            var tripEntity = _mapper.Map<Trip>(dto);
            await _repo.AddAsync(tripEntity).ConfigureAwait(false);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip '{Name}' created successfully.", tripEntity.Name);

            return _mapper.Map<GetTripDTO>(tripEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a trip.");
            throw;
        }
    }

    public async Task UpdateTripAsync(UpdateTripDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);

        try
    {
        var existingTrip = await _repo.GetByIdAsync(dto.Id).ConfigureAwait(false)
            ?? throw new ArgumentException($"Trip with ID {dto.Id} was not found.");

        // Auto-generate slug if empty
        if (string.IsNullOrWhiteSpace(dto.Slug))
        {
            dto.Slug = SlugHelper.GenerateSlug(dto.Name);
        }

        // Only check slug if it has changed
        if (dto.Slug != existingTrip.Slug &&
            await _repo.GetByPredicateAsync(t => t.Slug == dto.Slug).ConfigureAwait(false) is not null)
        {
            _logger.LogWarning("A trip with slug '{Slug}' already exists.", dto.Slug);
            throw new ValidationException($"Trip with slug '{dto.Slug}' already exists.");
        }

        _mapper.Map(dto, existingTrip); // Update entity with DTO values
        _repo.Update(existingTrip);
        await _repo.SaveAsync().ConfigureAwait(false);

        _logger.LogInformation("Trip '{Name}' updated successfully.", existingTrip.Name);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error occurred while updating trip with ID {Id}.", dto.Id);
        throw;
    }
    }

    public async Task DeleteTripAsync(int id)
    {
        try
        {
            var trip = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip with ID {id} was not found.");

             _repo.Delete(trip);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip '{Name}' deleted successfully.", trip.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting trip with ID {Id}.", id);
            throw;
        }
    }

    public async Task<IEnumerable<GetTripDTO>> GetAllTripsAsync()
    {
        try
        {
            var trips = await _repo.GetAllAsync().ConfigureAwait(false);
            _logger.LogDebug("Retrieved {Count} trips.", trips?.Count() ?? 0);
            return _mapper.Map<IEnumerable<GetTripDTO>>(trips);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all trips.");
            throw;
        }
    }

    public async Task<GetTripDTO> GetTripByIdAsync(int id)
    {
        try
        {
            var trip = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip with ID {id} was not found.");

            _logger.LogDebug("Trip '{Name}' retrieved successfully.", trip.Name);
            return _mapper.Map<GetTripDTO>(trip);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving trip with ID {Id}.", id);
            throw;
        }
    }


}
