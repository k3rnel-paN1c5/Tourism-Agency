using System.ComponentModel.DataAnnotations;
using Application.DTOs.TripPlan;
using Application.DTOs.TripPlanCar;

namespace Application.IServices.UseCases;

/// <summary>
/// Defines the contract for Trip Plan-related business logic operations.
/// </summary>
public interface ITripPlanService
{
    /// <summary>
    /// Creates a new trip plan asynchronously.
    /// </summary>
    /// <param name="createTripPlanDto">The DTO containing information for the new trip plan.</param>
    /// <returns>A <see cref="GetTripPlanDTO"/> representing the newly created trip plan.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="createTripPlanDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the associated Trip or Region is not found.</exception>
    /// <exception cref="ValidationException">Thrown if the trip is unavailable or date validations fail (e.g., End Date is not after Start Date, Start Date is in the past) Or price less than 0.</exception>
    Task<GetTripPlanDTO> CreateTripPlanAsync(CreateTripPlanDTO createTripPlanDto);
    
    /// <summary>
    /// Retrieves a trip plan by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the trip plan.</param>
    /// <returns>A <see cref="GetTripPlanDTO"/> representing the found trip plan.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the trip plan with the specified ID is not found.</exception>
    Task<GetTripPlanDTO> GetTripPlanByIdAsync(int id);

    /// <summary>
    /// Retrieves all trip plans asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="GetTripPlanDTO"/> representing all trip plans.</returns>
    Task<IEnumerable<GetTripPlanDTO>> GetAllTripPlansAsync();

    /// <summary>
    /// Retrieves all trip plans that start in the future asynchronously.
    /// </summary>
    /// <returns>A collection of <see cref="GetTripPlanDTO"/> representing all upcoming trip plans.</returns>
    Task<IEnumerable<GetTripPlanDTO>> GetUpcomingTripPlansAsync();

    /// <summary>
    /// Updates an existing trip plan asynchronously.
    /// </summary>
    /// <param name="updateTripPlanDto">The DTO containing updated information for the trip plan.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="updateTripPlanDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the trip plan, associated Trip, or Region is not found.</exception>
    /// <exception cref="ValidationException">Thrown if date validations fail (e.g., End Date is not after Start Date, Start Date is in the past).</exception>
    Task UpdateTripPlanAsync(UpdateTripPlanDTO updateTripPlanDto);

    /// <summary>
    /// Deletes a trip plan by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the trip plan to delete.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the trip plan with the specified ID is not found.</exception>
    Task DeleteTripPlanAsync(int id);

    /// <summary>
    /// Adds a car to an existing trip plan asynchronously.
    /// </summary>
    /// <param name="createTripPlanCarDto">The DTO containing information for the car to be added to the trip plan.</param>
    /// <returns>A <see cref="GetTripPlanCarDTO"/> representing the newly added car in the trip plan.</returns>
    /// <exception cref="ArgumentNullException">Thrown if <paramref name="createTripPlanCarDto"/> is null.</exception>
    /// <exception cref="KeyNotFoundException">Thrown if the associated Trip Plan is not found.</exception>
    /// <exception cref="ValidationException">Thrown if a the car is not available within the trip plan duration.</exception>
    Task<GetTripPlanCarDTO> AddCarToTripPlanAsync(CreateTripPlanCarFromTripPlanDTO createTripPlanCarDto);

     /// <summary>
    /// Removes a car from a trip plan by its unique identifier asynchronously.
    /// </summary>
    /// <param name="id">The unique identifier of the trip plan car to remove.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the trip plan car with the specified ID is not found.</exception>
    /// <exception cref="InvalidOperationException">Thrown if the trip plan car does not belong to a valid trip plan or if the trip plan has no cars to remove.</exception>
    Task RemoveCarFromTripPlanAsync(int id);

    Task<IEnumerable<GetTripPlanDTO>> GetTripPlansByDateIntervalAsync(DateTime startDate, DateTime endDate);
}
