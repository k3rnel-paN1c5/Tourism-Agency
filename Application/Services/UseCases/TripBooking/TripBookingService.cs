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
            // Check if Trip Plan exists
            var tripPlan = await _tripPlanService.GetTripPlanByIdAsync(createTripBookingDto.TripPlanId).ConfigureAwait(false);
            if (tripPlan is null)
            {
                _logger.LogError("Trip plan with ID {TripPlanId} was not found.", createTripBookingDto.TripPlanId);
                throw new KeyNotFoundException($"Trip plan with ID {createTripBookingDto.TripPlanId} was not found.");
            }

            // Validate dates and passengers
            if (createTripBookingDto.StartDate < DateTime.UtcNow.Date)
            {
                _logger.LogWarning("Attempted to create trip booking with a start date in the past: {StartDate}", createTripBookingDto.StartDate);
                throw new ValidationException("Start Date must be in the future.");
            }

            if (createTripBookingDto.EndDate <= createTripBookingDto.StartDate)
            {
                _logger.LogWarning("Attempted to create trip booking where End Date ({EndDate}) is not after Start Date ({StartDate}).", createTripBookingDto.EndDate, createTripBookingDto.StartDate);
                throw new ValidationException("End Date must be after Start Date.");
            }

            if (createTripBookingDto.NumOfPassengers <= 0)
            {
                _logger.LogWarning("Attempted to create trip booking with invalid number of passengers: {NumOfPassengers}", createTripBookingDto.NumOfPassengers);
                throw new ValidationException("Number of passengers must be greater than zero.");
            }

            // Create base booking
            var bookingDto = _mapper.Map<CreateBookingDTO>(createTripBookingDto);

            _logger.LogInformation("Creating base booking for trip booking.");
            var createdBooking = await _bookingService.CreateBookingAsync(bookingDto).ConfigureAwait(false);


            // Map and save trip booking
            var tripBookingEntity = _mapper.Map<TripBooking>(createTripBookingDto);
            tripBookingEntity.BookingId = createdBooking.Id; // Link to booking

            await _tripBookingRepository.AddAsync(tripBookingEntity).ConfigureAwait(false);
            await _tripBookingRepository.SaveAsync().ConfigureAwait(false);

             _logger.LogInformation("Trip booking '{BookingId}' created successfully for TripPlanId: {TripPlanId}.", tripBookingEntity.BookingId, createTripBookingDto.TripPlanId);


            return _mapper.Map<GetTripBookingDTO>(tripBookingEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while creating a trip booking. Error: {Message}", ex.Message);
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
            _logger.LogError("Error occurred while retrieving trip booking with ID {Id}.  Error: {Message}", id, ex.Message);
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
                tripBookings = await _tripBookingRepository.GetAllByPredicateAsync(tb => tb.Booking!.CustomerId == userIdClaim).ConfigureAwait(false);
            else
                tripBookings = await _tripBookingRepository.GetAllAsync().ConfigureAwait(false);

            _logger.LogDebug("{Count} trip bookings retrieved.", tripBookings?.Count() ?? 0);
            if (tripBookings is not null)
                foreach (var tb in tripBookings)
                {
                    tb.Booking = _mapper.Map<Booking>(await _bookingService.GetBookingByIdAsync(tb.BookingId));
                }
            return _mapper.Map<IEnumerable<GetTripBookingDTO>>(tripBookings);
        }
        catch (Exception ex)
        {
            _logger.LogError("Error occurred while retrieving all trip bookings. Error: {Mesage}", ex.Message);
            throw;
        }
    }

    public async Task ConfirmTripBookingAsync(int id)
    {
        try
        {
            await _bookingService.ConfirmBookingAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while Confirming trip booking with ID {Id}.", id);
            throw;
        }
    }


    public async Task UpdateTripBookingAsync(UpdateTripBookingDTO dto)
    {

        ArgumentNullException.ThrowIfNull(dto);

        try
        {
            var existingTripBooking = await _tripBookingRepository.GetByIdAsync(dto.Id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Trip booking with ID {dto.Id} was not found.");

            // Validate dates and passengers
            if (dto.StartDate < DateTime.UtcNow.Date)
                throw new ArgumentException("Start Date must be in the future.");

            if (dto.EndDate <= dto.StartDate)
                throw new ArgumentException("End Date must be after Start Date.");

            if (dto.NumOfPassengers <= 0)
                throw new ArgumentException("Number of passengers must be greater than zero.");

            // Update linked booking
            var updateBookingDto = new UpdateBookingDTO
            {
                Id = dto.Id,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
                NumOfPassengers = dto.NumOfPassengers
            };
            await _bookingService.UpdateBookingAsync(updateBookingDto).ConfigureAwait(false);

            // Update trip booking entity
            _mapper.Map(dto, existingTripBooking);
            _tripBookingRepository.Update(existingTripBooking);
            await _tripBookingRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip booking '{Id}' updated successfully.", dto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating trip booking with ID {Id}.", dto.Id);
            throw;
        }
    }
    
    public async Task DeleteTripBookingAsync(int id)
    {
        try
        {
            var tripBooking = await _tripBookingRepository.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Trip booking with ID {id} was not found.");

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
