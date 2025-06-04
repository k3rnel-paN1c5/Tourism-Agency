using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Security.Claims;
using Application.DTOs.Booking;
using Application.DTOs.TripBooking;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services.UseCases;

/// <summary>
/// Provides business logic for managing Trip Bookings.
/// </summary>
public class TripBookingService : ITripBookingService
{
    private readonly IRepository<TripBooking, int> _tripBookingRepository;
    private readonly IBookingService _bookingService;
    private readonly ITripPlanService _tripPlanService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly ILogger<TripBookingService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="TripBookingService"/> class.
    /// </summary>
    /// <param name="repository">The generic repository for TripBooking entities.</param>
    /// <param name="bookingService">The service for general booking operations.</param>
    /// <param name="tripPlanService">The service for trip plan operations.</param>
    /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
    /// <param name="logger">The logger for this service.</param>
    /// <param name="httpContextAccessor">The accessor for the current HTTP context.</param>
    public TripBookingService(

        IRepository<TripBooking, int> repository,
        IBookingService bookingService,
        ITripPlanService tripPlanService,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        ILogger<TripBookingService> logger
        )
    {
        _tripBookingRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        _tripPlanService = tripPlanService ?? throw new ArgumentNullException(nameof(tripPlanService));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _mapper = mapper;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<GetTripBookingDTO> CreateTripBookingAsync(CreateTripBookingDTO createTripBookingDto)
    {
        if(createTripBookingDto is null){
            _logger.LogError("CreateTripBookingAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(createTripBookingDto), "Trip Booking creation DTO cannot be null.");
        }

        _logger.LogInformation("Attempting to create trip booking for TripPlanId: {TripPlanId}", createTripBookingDto.TripPlanId);
        try
        {
            var tripPlan = await _tripPlanService.GetTripPlanByIdAsync(createTripBookingDto.TripPlanId).ConfigureAwait(false);
            if (tripPlan is null)
            {
                _logger.LogError("Trip plan with ID {TripPlanId} was not found.", createTripBookingDto.TripPlanId);
                throw new KeyNotFoundException($"Trip plan with ID {createTripBookingDto.TripPlanId} was not found.");
            }

            // Create base booking
            var bookingDto = _mapper.Map<CreateBookingDTO>(createTripBookingDto);

            _logger.LogInformation("Creating base booking for trip booking.");
            var createdBooking = await _bookingService.CreateBookingAsync(bookingDto).ConfigureAwait(false);


            var tripBookingEntity = _mapper.Map<TripBooking>(createTripBookingDto);
            tripBookingEntity.BookingId = createdBooking.Id; // Link to booking

            await _tripBookingRepository.AddAsync(tripBookingEntity).ConfigureAwait(false);
            await _tripBookingRepository.SaveAsync().ConfigureAwait(false);

             _logger.LogInformation("Trip booking '{BookingId}' created successfully for TripPlanId: {TripPlanId}.", tripBookingEntity.BookingId, createTripBookingDto.TripPlanId);


            return _mapper.Map<GetTripBookingDTO>(tripBookingEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a trip booking.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetTripBookingDTO> GetTripBookingByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve trip booking with ID: {Id}", id);
        try
        {
            var tripBooking = await _tripBookingRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (tripBooking == null)
            {
                _logger.LogWarning("Trip booking with ID {Id} was not found.", id);
                throw new KeyNotFoundException($"Trip booking with ID {id} was not found.");
            }

            _logger.LogInformation("Trip booking '{Id}' retrieved successfully.", id);
            return _mapper.Map<GetTripBookingDTO>(tripBooking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving trip booking with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetTripBookingDTO>> GetAllTripBookingsAsync()
    {
        _logger.LogInformation("Attempting to retrieve all trip bookings.");
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                _logger.LogError("HTTP context is unavailable during GetAllTripBookingsAsync.");
                throw new InvalidOperationException("HTTP context is unavailable.");
            }

            var userIdClaim = httpContext.User.Claims
               .FirstOrDefault(c => c.Type == "UserId" ||
                                    c.Type == "sub" ||
                                    c.Type == ClaimTypes.NameIdentifier)?.Value;
            var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

            IEnumerable<TripBooking> tripBookings;

            if (role == "Customer")
            {
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    _logger.LogWarning("Customer role detected but UserId claim is missing.");
                    throw new UnauthorizedAccessException("Customer ID not found for retrieving bookings.");
                }
                _logger.LogDebug("Retrieving trip bookings for customer ID: {CustomerId}", userIdClaim);
                tripBookings = await _tripBookingRepository.GetAllByPredicateAsync(tb => tb.Booking!.CustomerId == userIdClaim).ConfigureAwait(false);
            }
            else
            {
                _logger.LogDebug("Retrieving all trip bookings for employees.");
                tripBookings = await _tripBookingRepository.GetAllAsync().ConfigureAwait(false); 
            }
        
            _logger.LogDebug("{Count} trip bookings retrieved.", tripBookings?.Count() ?? 0);

            if (tripBookings is not null)
            {
                foreach (var tb in tripBookings)
                {
                    tb.Booking = _mapper.Map<Booking>(await _bookingService.GetBookingByIdAsync(tb.BookingId));
                }
            }
                
            return _mapper.Map<IEnumerable<GetTripBookingDTO>>(tripBookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all trip bookings.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task ConfirmTripBookingAsync(int id)
    {
        _logger.LogInformation("Attempting to Confirm a trip booking with ID: {id}.", id);
        try
        {
            var tripBooking = await GetTripBookingByIdAsync(id);
            if (tripBooking is null)
            {
                _logger.LogWarning("Trip booking with ID {Id} was not found for update.", id);
                throw new KeyNotFoundException($"Trip booking with ID {id} was not found.");
            }
            await _bookingService.ConfirmBookingAsync(id).ConfigureAwait(false);
            _logger.LogInformation("Trip booking '{Id}' confirmed successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while Confirming trip booking with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task CancelTripBookingAsync(int id)
    {
        try
        {
            await _bookingService.CancelBookingAsync(id).ConfigureAwait(false);
            _logger.LogInformation("Trip booking '{Id}' confirmed successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while Confirming trip booking with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateTripBookingAsync(UpdateTripBookingDTO updateTripBookingDto)
    {
        if (updateTripBookingDto is null)
        {
            _logger.LogError("UpdateTripBookingAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(updateTripBookingDto), "Trip Booking Update DTO cannot be null.");
        }
        _logger.LogInformation("Attempting to update trip booking with ID: {Id}", updateTripBookingDto.Id);

        try
        {
            var existingTripBooking = await _tripBookingRepository.GetByIdAsync(updateTripBookingDto.Id).ConfigureAwait(false);

            if (existingTripBooking is null)
            {
                _logger.LogWarning("Trip booking with ID {Id} was not found for update.", updateTripBookingDto.Id);
                throw new KeyNotFoundException($"Trip booking with ID {updateTripBookingDto.Id} was not found.");
            }

            // Validate dates and passengers
            if (updateTripBookingDto.StartDate < DateTime.UtcNow.Date)
            {
                _logger.LogWarning("Attempted to update trip booking ({Id}) with a start date in the past: {StartDate}", updateTripBookingDto.Id, updateTripBookingDto.StartDate);
                throw new ValidationException("Start Date must be in the future.");
            }

            if (updateTripBookingDto.EndDate <= updateTripBookingDto.StartDate)
            {
                _logger.LogWarning("Attempted to update trip booking ({Id}) where End Date ({EndDate}) is not after Start Date ({StartDate}).", updateTripBookingDto.Id, updateTripBookingDto.EndDate, updateTripBookingDto.StartDate);
                throw new ValidationException("End Date must be after Start Date.");
            }

            if (updateTripBookingDto.NumOfPassengers <= 0)
            {
                _logger.LogWarning("Attempted to update trip booking ({Id}) with invalid number of passengers: {NumOfPassengers}", updateTripBookingDto.Id, updateTripBookingDto.NumOfPassengers);
                throw new ValidationException("Number of passengers must be greater than zero.");
            }

            // Update linked booking
            var updateBookingDto = _mapper.Map<UpdateBookingDTO>(updateTripBookingDto);


            // Calling Base Booking service to handle its update
            _logger.LogInformation("Updating linked booking for trip booking '{Id}'.", updateBookingDto.Id);
            await _bookingService.UpdateBookingAsync(updateBookingDto).ConfigureAwait(false);

            // Update trip booking entity
            _mapper.Map(updateTripBookingDto, existingTripBooking);
            _tripBookingRepository.Update(existingTripBooking);
            await _tripBookingRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip booking '{Id}' updated successfully.", updateTripBookingDto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating trip booking with ID {Id}.", updateTripBookingDto.Id);
            throw;
        }
    }
    
    public async Task DeleteTripBookingAsync(int id)
    {
        _logger.LogInformation("Attempting to delete trip booking with ID: {Id}", id);
        try
        {
            var tripBooking = await _tripBookingRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (tripBooking is null) {
                _logger.LogWarning("Trip booking with ID {Id} was not found for deletion.", id);
                throw new KeyNotFoundException($"Trip booking with ID {id} was not found.");
            }

            await _tripBookingRepository.DeleteByIdAsync(id).ConfigureAwait(false);
            await _bookingService.DeleteBookingAsync(id).ConfigureAwait(false); // Delete linked booking
            await _tripBookingRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip booking '{Id}' deleted successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting trip booking with ID {Id}.", id);
            throw;
        }
    }
}
