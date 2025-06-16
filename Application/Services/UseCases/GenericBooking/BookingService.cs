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
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
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
    private const int MINIMUM_NUMBER_OF_PASSENGERS = 1;

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
        if (createBookingDto is null)
        {
            _logger.LogError("CreateBookingAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(createBookingDto), "Booking creation DTO cannot be null.");
        }
        _logger.LogInformation("Attempting to create base booking for a {type} booking", createBookingDto.IsTripBooking ? "Trip" : "Car");

        try
        {

            string userIdClaim = GetCurrentUserIdClaim();

            ValidateDate(createBookingDto.StartDate, createBookingDto.EndDate);
            ValidateNumOfPassenger(createBookingDto.NumOfPassengers);

            var bookingEntity = _mapper.Map<Booking>(createBookingDto);

            // the customer who made the booking
            bookingEntity.CustomerId = userIdClaim;

            await _bookingRepository.AddAsync(bookingEntity).ConfigureAwait(false);
            await _bookingRepository.SaveAsync().ConfigureAwait(false);

            // The mapper calculates the amount that should be paid
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
            if (booking is null)
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
            string userIdClaim = GetCurrentUserIdClaim();

            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking!.Status != BookingStatus.Pending)
            {
                _logger.LogWarning("Trying to Confirm Booking '{id}' with status: {stat}", id, booking.Status);
                throw new InvalidOperationException($"can't confirm a {booking.Status} booking");
            }

            booking.Status = BookingStatus.Confirmed;
            booking.EmployeeId = userIdClaim;
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveAsync();

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
        try
        {
            string userIdClaim = GetCurrentUserIdClaim();

            var booking = await _bookingRepository.GetByIdAsync(id);
            if (booking!.Status != BookingStatus.Pending)
            {
                _logger.LogWarning("Trying to Cancel Booking '{id}' with status: {stat}", id, booking.Status);
                throw new InvalidOperationException($"can't confirm a {booking.Status} booking");
            }

            booking.Status = BookingStatus.Cancelled;
            if (_httpContextAccessor.HttpContext.User.IsInRole("Customer"))
                userIdClaim = null;
            booking.EmployeeId = userIdClaim;
            _bookingRepository.Update(booking);
            await _bookingRepository.SaveAsync();

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while canceling booking with ID {Id}.", id);
            throw;
        }
    }

    /// <inheritdoc />
    public async Task UpdateBookingAsync(UpdateBookingDTO updateBookingDto)
    {
        if (updateBookingDto is null)
        {
            _logger.LogError("UpdateBookingAsync: Input DTO is null.");
            throw new ArgumentNullException(nameof(updateBookingDto), "Booking Upate DTO cannot be null.");
        }
        try
        {
            var existingBooking = await GetBookingByIdAsync(updateBookingDto.Id).ConfigureAwait(false);
            var bookingEntity = _mapper.Map<Booking>(existingBooking);

            ValidateDate(updateBookingDto.StartDate, updateBookingDto.EndDate);
            ValidateNumOfPassenger(updateBookingDto.NumOfPassengers);

            _mapper.Map(updateBookingDto, bookingEntity);
            _bookingRepository.Update(bookingEntity);
            await _bookingRepository.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Booking '{Id}' updated successfully.", updateBookingDto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating booking with ID {Id}.", updateBookingDto.Id);
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


    /// <summary>
    /// Process payment for a booking
    /// </summary>
    public async Task<PaymentProcessResultDTO> ProcessBookingPaymentAsync(int bookingId, ProcessPaymentDTO processPaymentDto)
    {
        try
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found");

            // Get the payment for this booking
            var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);
            
            // Process the payment
            processPaymentDto.PaymentId = payment.Id;
            var result = await _paymentService.ProcessPaymentAsync(processPaymentDto);

            // Auto-confirm booking if payment is completed
            if (result.Payment.Status == PaymentStatus.Paid && booking.Status == BookingStatus.Pending)
            {
                await ConfirmBookingAsync(bookingId);
                _logger.LogInformation("Booking {BookingId} automatically confirmed after full payment", bookingId);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing payment for booking {BookingId}", bookingId);
            throw;
        }
    }

    /// <summary>
    /// Process refund for a booking
    /// </summary>
    public async Task<PaymentProcessResultDTO> ProcessBookingRefundAsync(int bookingId, ProcessRefundDTO refundDto)
    {
        try
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            if (booking == null)
                throw new KeyNotFoundException($"Booking with ID {bookingId} not found");

            var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);
            
            refundDto.PaymentId = payment.Id;
            var result = await _paymentService.ProcessRefundAsync(refundDto);

            // Handle booking status based on refund
            if (result.Payment.Status == PaymentStatus.Refunded)
            {
                booking.Status = BookingStatus.Cancelled;
                _bookingRepository.Update(booking);
                await _bookingRepository.SaveAsync();
                _logger.LogInformation("Booking {BookingId} cancelled after full refund", bookingId);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing refund for booking {BookingId}", bookingId);
            throw;
        }
    }


    //* Utility Functions for Booking Service *//

    /// <summary>
    /// Gets the current user Id claims to register who called a method in this service
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown if HTTP context is unavailable.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if user is nott authenticated.</exception>
    private string GetCurrentUserIdClaim()
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


        return userIdClaim;
    }

    /// <summary>
    /// Validates the date of a requeseted booking.
    /// End date should be after start date, and start date should be in the future.
    /// </summary>
    /// <param name="startDate">The start date of the requested booking.</param>
    /// <param name="endDate">The end date of the requested booking.</param>
    /// <exception cref="ValidationException">Thrown if date validations fail.</exception>
    private void ValidateDate(DateTime startDate, DateTime endDate)
    {
        if (startDate < DateTime.UtcNow.Date)
        {
            _logger.LogWarning("Attempted to create booking with a start date in the past: {StartDate}", startDate);
            throw new ValidationException("Start Date must be in the future.");
        }

        if (endDate <= startDate)
        {
            _logger.LogWarning("Attempted to create booking where End Date ({EndDate}) is not after Start Date ({StartDate}).", endDate, startDate);
            throw new ValidationException("End Date must be after Start Date.");
        }
    }

    /// <summary>
    /// Validates the number of passengers for a requested booking.
    /// The number of passengers should be greater than the minimum number of passengers;
    /// </summary>
    /// <param name="numberOfPassengers">The number of passengers for the requested booking.</param>
    /// <exception cref="ValidationException">Thrown if passengers number validations fail.</exception>
    private void ValidateNumOfPassenger(int numberOfPassengers)
    {
        if (numberOfPassengers < MINIMUM_NUMBER_OF_PASSENGERS)
        {
            _logger.LogWarning("Attempted to create booking with invalid number of passengers: {NumOfPassengers}", numberOfPassengers);
            throw new ValidationException("Number of passengers must be greater than zero.");
        }
    }

}
