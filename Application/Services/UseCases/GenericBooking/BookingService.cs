using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Application.DTOs.Booking;
using Application.DTOs.Payment;
using Application.IServices.UseCases;
using Application.Utilities;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services.UseCases;

/// <summary>
/// Provides business logic for managing Bookings.
/// </summary>
public class BookingService : IBookingService
{
    private readonly IRepository<Booking, int> _bookingRepository;
    private readonly IPaymentService _paymentService;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BookingService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BookingService"/> class.
    /// </summary>
    /// <param name="repository">The generic repository for TripBooking entities.</param>
    /// <param name="paymentService">The service for payment operations.</param>
    /// <param name="mapper">The AutoMapper instance for DTO-entity mapping.</param>
    /// <param name="logger">The logger for this service.</param>
    /// <param name="httpContextAccessor">The accessor for the current HTTP context.</param>
    public BookingService(
        IRepository<Booking, int> repository,
        IPaymentService paymentService,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<BookingService> logger)
    {
        _bookingRepository = repository;
        _paymentService = paymentService;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <inheritdoc />
    public async Task<GetBookingDTO> CreateBookingAsync(CreateBookingDTO createBookingDto)
    {
        if(createBookingDto is null){
            _logger.LogError("CreateBookingAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(createBookingDto), "Booking creation DTO cannot be null.");
        }
        _logger.LogInformation("Attempting to create base booking for a {type} booking", createBookingDto.BookingType? "Trip" : "Car");

        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            var userIdClaim = httpContext.User.Claims
               .FirstOrDefault(c => c.Type == "UserId" ||
                                    c.Type == "sub" ||
                                    c.Type == ClaimTypes.NameIdentifier)?.Value;


            // Validate dates and passengers
            if (createBookingDto.StartDate < DateTime.UtcNow.Date)
            {
                _logger.LogWarning("Attempted to create booking with a start date in the past: {StartDate}", createBookingDto.StartDate);
                throw new ValidationException("Start Date must be in the future.");
            }

            if (createBookingDto.EndDate <= createBookingDto.StartDate)
            {
                _logger.LogWarning("Attempted to create booking where End Date ({EndDate}) is not after Start Date ({StartDate}).", createBookingDto.EndDate, createBookingDto.StartDate);
                throw new ValidationException("End Date must be after Start Date.");
            }

            if (createBookingDto.NumOfPassengers <= 0)
            {
                _logger.LogWarning("Attempted to create booking with invalid number of passengers: {NumOfPassengers}", createBookingDto.NumOfPassengers);
                throw new ValidationException("Number of passengers must be greater than zero.");
            }

            var bookingEntity = _mapper.Map<Booking>(createBookingDto);
            bookingEntity.CustomerId = userIdClaim;
            await _bookingRepository.AddAsync(bookingEntity).ConfigureAwait(false);
            await _bookingRepository.SaveAsync().ConfigureAwait(false);

            // The mapper calculates the amount
            var newPayment = _mapper.Map<CreatePaymentDTO>(bookingEntity);
        
            await _paymentService.CreatePaymentAsync(newPayment);
            _logger.LogInformation("Booking '{Id}' created successfully.", bookingEntity.Id);
            return _mapper.Map<GetBookingDTO>(bookingEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a booking.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<GetBookingDTO> GetBookingByIdAsync(int id)
    {
        _logger.LogInformation("Attempting to retrieve booking with ID: {Id}", id);
        try
        {
            var booking = await _bookingRepository.GetByIdAsync(id).ConfigureAwait(false);
            if (booking == null)
            {
                _logger.LogWarning("Booking with ID {Id} was not found.", id);
                throw new KeyNotFoundException($"Booking with ID {id} was not found.");
            }

            _logger.LogDebug("Booking '{Id}' retrieved successfully.", id);
            return _mapper.Map<GetBookingDTO>(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving booking with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<IEnumerable<GetBookingDTO>> GetAllBookingsAsync()
    {
        _logger.LogInformation("Attempting to retrieve all bookings.");
        try
        {
            var bookings = await _bookingRepository.GetAllAsync().ConfigureAwait(false);
            _logger.LogDebug("{Count} bookings retrieved.", bookings?.Count() ?? 0);
            return _mapper.Map<IEnumerable<GetBookingDTO>>(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all bookings.");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task ConfirmBookingAsync(int id)
    {
        try
        {
            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is unavailable.");

            if (!httpContext.User.Identity!.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var userIdClaim = httpContext.User.Claims
                .FirstOrDefault(c => c.Type == "UserId" ||
                                     c.Type == "sub" ||
                                     c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                var claimsList = httpContext.User.Claims.Select(c => new { c.Type, c.Value });
                _logger.LogWarning("Missing UserId claim. Available claims: {@Claims}", claimsList);
                throw new UnauthorizedAccessException("User ID claim not found.");
            }

            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking!.Status == BookingStatus.Pending)
            {
                booking.Status = BookingStatus.Confirmed;
                booking.EmployeeId = userIdClaim;
                _bookingRepository.Update(booking);
                await _bookingRepository.SaveAsync();
                return;
            }
            throw new Exception($"can't confirm a {booking.Status} booking");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while confirming booking with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task CancelBookingAsync(int id)
    {
        await _bookingRepository.SaveAsync();
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public async Task UpdateBookingAsync(UpdateBookingDTO dto)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));

        try
        {
            var existingBooking = await _bookingRepository.GetByIdAsync(dto.Id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Booking with ID {dto.Id} was not found.");

            if (dto.StartDate < DateTime.UtcNow.Date)
                throw new ArgumentException("Start Date must be in the future.");

            if (dto.EndDate <= dto.StartDate)
                throw new ArgumentException("End Date must be after Start Date.");

            if (dto.NumOfPassengers <= 0)
                throw new ArgumentException("Number of passengers must be greater than zero.");

            _mapper.Map(dto, existingBooking);
            _bookingRepository.Update(existingBooking);
            await _bookingRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Booking '{Id}' updated successfully.", dto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating booking with ID {Id}.", dto.Id);
            throw;
        }
    }


    /// <inheritdoc />
    public async Task DeleteBookingAsync(int id)
    {
        try
        {
            var booking = await _bookingRepository.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Booking with ID {id} was not found.");

            await _bookingRepository.DeleteByIdAsync(id).ConfigureAwait(false);
            await _bookingRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Booking '{Id}' deleted successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting booking with ID {Id}.", id);
            throw;
        }
    }

}
