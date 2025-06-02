using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs.Booking;
using Domain.Enums;

namespace Application.IServices.UseCases;


/// <summary>
/// Defines the contract for Booking-related business logic operations.
/// </summary>
public interface IBookingService
{
    /// <summary>
    /// Creates a new booking asynchronously.
    /// </summary>
    /// <param name="createBookingDto">The DTO containing information for the new  booking.</param>
    /// <returns>A <see cref="GetBookingDTO"/> representing the newly created  booking.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="createBookingDto"/> is null.</exception>
    /// <exception cref="ValidationException">Thrown if date/passenger validations fail.</exception>
    public Task<GetBookingDTO> CreateBookingAsync(CreateBookingDTO createBookingDto);

    /// <summary>
    /// Retrieves a booking by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the booking.</param>
    /// <returns>A <see cref="GetBookingDTO"/> representing the found booking.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the booking with the specified ID is not found.</exception>
    public Task<GetBookingDTO> GetBookingByIdAsync(int id);

    /// <summary>
    /// Retrieves all bookings asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="GetBookingDTO"/> representing all bookings.</returns>
    public Task<IEnumerable<GetBookingDTO>> GetAllBookingsAsync();

    /// <summary>
    /// Confirms a booking by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the booking to confirm.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the booking with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the booking with the specified ID is confirmed, rejected, or completed.</exception>
    public Task ConfirmBookingAsync(int id);

    /// <summary>
    /// Cancels a booking by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the booking to cancel.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the booking with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the booking with the specified ID has already started and cannot be cancelled.</exception>
    public Task CancelBookingAsync(int id);

    /// <summary>
    /// Updates an existing booking asynchronously.
    /// </summary>
    /// <param name="updateBookingDto">The DTO containing updated information for the booking.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="updateBookingDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the booking with the specified ID is not found.</exception>
    /// <exception cref="ValidationException">Thrown if date/passenger validations fail.</exception>
    public Task UpdateBookingAsync(UpdateBookingDTO updateBookingDto);
    
    /// <summary>
    /// Deletes a booking by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the booking to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the booking with the specified ID is not found.</exception>
    public Task DeleteBookingAsync(int id);
}
