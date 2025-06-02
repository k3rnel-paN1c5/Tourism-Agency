using System.ComponentModel.DataAnnotations;
using Application.DTOs.Trip;

namespace Application.IServices.UseCases;

/// <summary>
/// Defines the contract for Trip-related business logic operations.
/// </summary>
public interface ITripService
{
    /// <summary>
    /// Creates a new trip asynchronously.
    /// </summary>
    /// <param name="createTripDto">The DTO containing information for the new trip.</param>
    /// <returns>A <see cref="GetTripDTO"/> representing the newly created trip.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="createTripDto"/> is null.</exception>
    /// <exception cref="ValidationException">Thrown if a trip with the same name or slug already exists.</exception>
    Task<GetTripDTO> CreateTripAsync(CreateTripDTO createTripDto);

    /// <summary>
    /// Retrieves a trip by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the trip.</param>
    /// <returns>A <see cref="GetTripDTO"/> representing the found trip.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the trip with the specified ID is not found.</exception>
    Task<GetTripDTO> GetTripByIdAsync(int id);

    /// <summary>
    /// Retrieves all trips asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="GetTripDTO"/> representing all trips.</returns>
    Task<IEnumerable<GetTripDTO>> GetAllTripsAsync();

    /// <summary>
    /// Retrieves all available and public trips asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="GetTripDTO"/> representing all available and public trips.</returns>
    Task<IEnumerable<GetTripDTO>> GetAvailablePublicTripsAsync();

    /// <summary>
    /// Updates an existing trip asynchronously.
    /// </summary>
    /// <param name="updateTripDto">The DTO containing updated information for the trip.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="updateTripDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the trip with the specified ID is not found.</exception>
    /// <exception cref="ValidationException">Thrown if another trip with the updated slug already exists.</exception>
    Task UpdateTripAsync(UpdateTripDTO updateTripDto);

    /// <summary>
    /// Deletes a trip by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the trip to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the trip with the specified ID is not found.</exception>
    Task DeleteTripAsync(int id);
}
