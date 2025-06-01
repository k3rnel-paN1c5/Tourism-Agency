using System;
using System.ComponentModel.DataAnnotations;
using Application.DTOs.TripPlanCar;

namespace Application.IServices.UseCases;

/// <summary>
/// Defines the contract for Trip Plan Car-related business logic operations.
/// This service manages the association of cars with specific trip plans.
/// </summary>
public interface ITripPlanCarService
{
    /// <summary>
    /// Creates a new trip plan car entry asynchronously.
    /// </summary>
    /// <param name="createTripPlanCarDto">The DTO containing information for the new trip plan car.</param>
    /// <returns>A <see cref="GetTripPlanCarDTO"/> representing the newly created trip plan car entry.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="createTripPlanCarDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the associated Car is not found.</exception>
    /// <exception cref="ValidationException">Potentially thrown if the car is not available for the specified dates.</exception>
    Task<GetTripPlanCarDTO> CreateTripPlanCarAsync(CreateTripPlanCarDTO createTripPlanCarDto);
    /// <summary>
    /// Retrieves a trip plan car entry by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the trip plan car entry.</param>
    /// <returns>A <see cref="GetTripPlanCarDTO"/> representing the found trip plan car entry.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the trip plan car with the specified ID is not found.</exception>
    Task<GetTripPlanCarDTO> GetTripPlanCarByIdAsync(int id);
    /// <summary>
    /// Retrieves all trip plan car entries asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="GetTripPlanCarDTO"/> representing all trip plan car entries.</returns>
    Task<IEnumerable<GetTripPlanCarDTO>> GetAllTripPlanCarsAsync();
    /// <summary>
    /// Updates an existing trip plan car entry asynchronously.
    /// </summary>
    /// <param name="updateTripPlanCarDto">The DTO containing updated information for the trip plan car.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="updateTripPlanCarDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the trip plan car with the specified ID or the associated Car is not found.</exception>
    /// <exception cref="ValidationException">Potentially thrown if the car is not available for the specified dates (if availability check is re-enabled).</exception>
    Task UpdateTripPlanCarAsync(UpdateTripPlanCarDTO updateTripPlanCarDto);
    /// <summary>
    /// Deletes a trip plan car entry by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the trip plan car entry to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the trip plan car with the specified ID is not found.</exception>
    Task DeleteTripPlanCarAsync(int id);

}
