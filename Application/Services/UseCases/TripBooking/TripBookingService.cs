using System;
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

public class TripBookingService : ITripBookingService
{
    readonly IRepository<TripBooking ,int> _repo;
    readonly IBookingService _bookingService;
    readonly ITripPlanService _tripPlanService;
    readonly IMapper _mapper;
    private readonly ILogger<TripBookingService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public TripBookingService(
        IRepository<TripBooking ,int> repository, 
        IBookingService bookingService, 
        ITripPlanService tripPlanService, 
        IMapper mapper, ILogger<TripBookingService> logger, 
        IHttpContextAccessor httpContextAccessor
        )
    {
        _repo = repository;
        _bookingService = bookingService;
        _tripPlanService = tripPlanService;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<GetTripBookingDTO> CreateTripBookingAsync(CreateTripBookingDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        try
        {
            // Check if Trip Plan exists
            var tripPlan = await _tripPlanService.GetTripPlanByIdAsync(dto.TripPlanId).ConfigureAwait(false)
                ?? throw new ArgumentException($"Trip plan with ID {dto.TripPlanId} was not found.");

            // Validate dates and passengers
            if (dto.StartDate < DateTime.UtcNow.Date)
                throw new ArgumentException("Start Date must be in the future.");

            if (dto.EndDate <= dto.StartDate)
                throw new ArgumentException("End Date must be after Start Date.");

            if (dto.NumOfPassengers <= 0)
                throw new ArgumentException("Number of passengers must be greater than zero.");

            // Create base booking
            var bookingDto = new CreateBookingDTO
            {
                BookingType = true,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                NumOfPassengers = dto.NumOfPassengers
            };

            var createdBooking = await _bookingService.CreateBookingAsync(bookingDto).ConfigureAwait(false);

            // Map and save trip booking
            var tripBookingEntity = _mapper.Map<TripBooking>(dto);
            tripBookingEntity.BookingId = createdBooking.Id; // Link to booking

            await _repo.AddAsync(tripBookingEntity).ConfigureAwait(false);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip booking '{Id}' created successfully.", tripBookingEntity.BookingId);

            return _mapper.Map<GetTripBookingDTO>(tripBookingEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a trip booking.");
            throw;
        }
    }

    public async Task DeleteTripBookingAsync(int id)
    {
        try
        {
            var tripBooking = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Trip booking with ID {id} was not found.");

            await _repo.DeleteByIdAsync(id).ConfigureAwait(false);
            await _bookingService.DeleteBookingAsync(id).ConfigureAwait(false); // Delete linked booking
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip booking '{Id}' deleted successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting trip booking with ID {Id}.", id);
            throw;
        }
    }

    public async Task<IEnumerable<GetTripBookingDTO>> GetAllTripBookingsAsync()
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext
               ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var userIdClaim = httpContext.User.Claims
               .FirstOrDefault(c => c.Type == "UserId" ||
                                    c.Type == "sub" ||
                                    c.Type == ClaimTypes.NameIdentifier)?.Value;

            var tripBookings = await _repo.GetAllByPredicateAsync(tb => tb.Booking.CustomerId == userIdClaim).ConfigureAwait(false);
            _logger.LogDebug("{Count} trip bookings retrieved.", tripBookings?.Count() ?? 0);
            if(tripBookings is not null)
                foreach(var tb in tripBookings){
                    tb.Booking = _mapper.Map<Booking>(await _bookingService.GetBookingByIdAsync(tb.BookingId));
                }
            return _mapper.Map<IEnumerable<GetTripBookingDTO>>(tripBookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all trip bookings.");
            throw;
        }
    }

    public async Task<GetTripBookingDTO> GetTripBookingByIdAsync(int id)
    {
        try
        {
            var tripBooking = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Trip booking with ID {id} was not found.");

            _logger.LogDebug("Trip booking '{Id}' retrieved successfully.", id);
            return _mapper.Map<GetTripBookingDTO>(tripBooking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving trip booking with ID {Id}.", id);
            throw;
        }
    }

    public async Task UpdateTripBookingAsync(UpdateTripBookingDTO dto)
    {

        ArgumentNullException.ThrowIfNull(dto);

        try
        {
            var existingTripBooking = await _repo.GetByIdAsync(dto.Id).ConfigureAwait(false)
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
            _repo.Update(existingTripBooking);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Trip booking '{Id}' updated successfully.", dto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating trip booking with ID {Id}.", dto.Id);
            throw;
        }
    }
}
