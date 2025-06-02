using System.ComponentModel.DataAnnotations;
using Application.DTOs.TripBooking;

namespace Application.IServices.UseCases;

/// <summary>
/// Defines the contract for TripBooking-related business logic operations.
/// </summary>
public interface ITripBookingService
{
    /// <summary>
    /// Creates a new trip booking asynchronously.
    /// </summary>
    /// <param name="createTripBookingDto">The DTO containing information for the new trip booking.</param>
    /// <returns>A <see cref="GetTripBookingDTO"/> representing the newly created trip booking.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="createTripBookingDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the trip plan is not found.</exception>
    /// <exception cref="ValidationException">Thrown if date/passenger validations fail.</exception>
    public Task<GetTripBookingDTO> CreateTripBookingAsync(CreateTripBookingDTO createTripBookingDto);

    /// <summary>
    /// Retrieves a trip booking by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the trip booking.</param>
    /// <returns>A <see cref="GetTripBookingDTO"/> representing the found trip booking.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the trip booking with the specified ID is not found.</exception>
    public Task<GetTripBookingDTO> GetTripBookingByIdAsync(int id);
    
    /// <summary>
    /// Retrieves all trip bookings asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="GetTripBookingDTO"/> representing all trip bookings.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable when trying to determine user role.</exception>
    /// <exception cref="UnauthorizedAccessException">Thrown if the userId Claim is empty.</exception>
    public Task<IEnumerable<GetTripBookingDTO>> GetAllTripBookingsAsync();

    /// <summary>
    /// Confirms a trip booking by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the trip booking to confirm.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the trip booking with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the trip booking with the specified ID is confirmed, rejected, or completed.</exception>
    public Task ConfirmTripBookingAsync(int id);

    /// <summary>
    /// Cancels a trip booking by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the trip booking to cancel.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the trip booking with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the trip booking with the specified ID has already started and cannot be cancelled.</exception>
    public Task CancelTripBookingAsync(int id);

    /// <summary>
    /// Updates an existing trip booking asynchronously.
    /// </summary>
    /// <param name="updateTripBookingDto">The DTO containing updated information for the trip booking.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="updateTripBookingDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the trip booking with the specified ID is not found.</exception>
    /// <exception cref="ValidationException">Thrown if date/passenger validations fail.</exception>
    public Task UpdateTripBookingAsync(UpdateTripBookingDTO updateTripBookingDto);

    /// <summary>
    /// Deletes a trip booking by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the trip booking to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the trip booking with the specified ID is not found.</exception>
    public Task DeleteTripBookingAsync(int id);

}
