using System;
using Application.DTOs.Booking;
using Application.IServices.UseCases;
using AutoMapper;
using Domain.Entities;
using Domain.IRepositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Application.Services.UseCases;

public class BookingService : IBookingService
{
    private readonly IRepository<Booking, int> _repo;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<BookingService> _logger;

    public BookingService(IRepository<Booking, int> repository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<BookingService> logger)
    {
        _repo = repository;
        _mapper = mapper;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<GetBookingDTO> CreateBookingAsync(CreateBookingDTO dto)
    {
        ArgumentNullException.ThrowIfNull(dto);
        try
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.User == null || !httpContext.User.Identity!.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var userIdClaim = httpContext.User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
                throw new UnauthorizedAccessException("User ID claim not found.");

            // Validate dates and passengers
            if (dto.StartDate < DateTime.UtcNow.Date)
                throw new ArgumentException("Start Date must be in the future.");

            if (dto.EndDate <= dto.StartDate)
                throw new ArgumentException("End Date must be after Start Date.");

            if (dto.NumOfPassengers <= 0)
                throw new ArgumentException("Number of passengers must be greater than zero.");

            var bookingEntity = _mapper.Map<Booking>(dto);
            bookingEntity.CustomerId = userIdClaim;
            await _repo.AddAsync(bookingEntity).ConfigureAwait(false);
            await _repo.SaveAsync().ConfigureAwait(false);
            _logger.LogInformation("Booking '{Id}' created successfully.", bookingEntity.Id);
            return _mapper.Map<GetBookingDTO>(bookingEntity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a booking.");
            throw;
        }
    }

    public async Task DeleteBookingAsync(int id)
    {
        try
        {
            var booking = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Booking with ID {id} was not found.");

            await _repo.DeleteByIdAsync(id).ConfigureAwait(false);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Booking '{Id}' deleted successfully.", id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting booking with ID {Id}.", id);
            throw;
        }
    }

    public async Task<IEnumerable<GetBookingDTO>> GetAllBookingsAsync()
    {
         try
        {
            var bookings = await _repo.GetAllAsync().ConfigureAwait(false);
            _logger.LogDebug("{Count} bookings retrieved.", bookings?.Count() ?? 0);
            return _mapper.Map<IEnumerable<GetBookingDTO>>(bookings);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all bookings.");
            throw;
        }
    }

    public async Task<GetBookingDTO> GetBookingByIdAsync(int id)
    {
        try
        {
            var booking = await _repo.GetByIdAsync(id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Booking with ID {id} was not found.");

            _logger.LogDebug("Booking '{Id}' retrieved successfully.", id);
            return _mapper.Map<GetBookingDTO>(booking);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving booking with ID {Id}.", id);
            throw;
        }
    }

    public async Task UpdateBookingAsync(UpdateBookingDTO dto)
    {
       if (dto == null) throw new ArgumentNullException(nameof(dto));

        try
        {
            var existingBooking = await _repo.GetByIdAsync(dto.Id).ConfigureAwait(false)
                ?? throw new KeyNotFoundException($"Booking with ID {dto.Id} was not found.");

            if (dto.StartDate < DateTime.UtcNow.Date)
                throw new ArgumentException("Start Date must be in the future.");

            if (dto.EndDate <= dto.StartDate)
                throw new ArgumentException("End Date must be after Start Date.");

            if (dto.NumOfPassengers <= 0)
                throw new ArgumentException("Number of passengers must be greater than zero.");

            _mapper.Map(dto, existingBooking);
            _repo.Update(existingBooking);
            await _repo.SaveAsync().ConfigureAwait(false);

            _logger.LogInformation("Booking '{Id}' updated successfully.", dto.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating booking with ID {Id}.", dto.Id);
            throw;
        }
    }
}
