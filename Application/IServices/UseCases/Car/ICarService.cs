using Application.DTOs.Car;

namespace Application.IServices.UseCases
{
    /// <summary>
    /// Defines the contract for Car-related business logic operations.
    /// </summary>
    public interface ICarService
    {
        /// <summary>
        /// Creates a new car asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing information for the new car.</param>
        /// <returns>A <see cref="GetCarDTO"/> representing the newly created car.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the associated category is not found.</exception>
        Task<GetCarDTO> CreateCarAsync(CreateCarDTO dto);

        /// <summary>
        /// Updates an existing car asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing updated information for the car.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the car with the specified ID is not found, or if the associated category is not found.</exception>
        Task UpdateCarAsync(UpdateCarDTO dto);

        /// <summary>
        /// Deletes a car by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the car to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the car with the specified ID is not found.</exception>
        Task DeleteCarAsync(int id);

        /// <summary>
        /// Retrieves all cars asynchronously.
        /// </summary>
        /// <returns>A collection of <see cref="GetCarDTO"/> representing all cars.</returns>
        Task<IEnumerable<GetCarDTO>> GetAllCarsAsync();

        /// <summary>
        /// Retrieves a car by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the car.</param>
        /// <returns>A <see cref="GetCarDTO"/> representing the found car.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the car with the specified ID is not found.</exception>
        Task<GetCarDTO> GetCarByIdAsync(int id);

        /// <summary>
        /// Retrieves all cars belonging to a specific category asynchronously.
        /// </summary>
        /// <param name="categoryId">The unique identifier of the category.</param>
        /// <returns>A collection of <see cref="GetCarDTO"/> representing cars in the specified category.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if no cars are found for the specified category ID.</exception>
        Task<IEnumerable<GetCarDTO>> GetCarsByCategoryAsync(int categoryId);

        /// <summary>
        /// Retrieves all cars that are available (not booked by either car bookings or trip plans)
        /// within a specified date range asynchronously.
        /// </summary>
        /// <param name="startDate">The desired start date for car availability.</param>
        /// <param name="endDate">The desired end date for car availability.</param>
        /// <returns>A collection of <see cref="GetCarDTO"/> representing the available cars.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="startDate"/> is in the past or <paramref name="endDate"/> is not after <paramref name="startDate"/>.</exception>
        Task<IEnumerable<GetCarDTO>> GetAvailableCarsAsync(DateTime startDate, DateTime endDate);
        

    }
}
