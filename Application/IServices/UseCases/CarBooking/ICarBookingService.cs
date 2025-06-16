using Application.DTOs.CarBooking;

namespace Application.IServices.UseCases
{
    /// <summary>
    /// Defines the contract for CarBooking-related business logic operations.
    /// </summary>
    public interface ICarBookingService
    {

        /// <summary>
        /// Creates a new car booking asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing information for the new car booking.</param>
        /// <returns>A <see cref="GetCarBookingDTO"/> representing the newly created car booking.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the associated car is not found.</exception>
        /// <exception cref="ArgumentException">Thrown if date validations fail (e.g., Start Date is in the past, End Date is not after Start Date) or if number of passengers is invalid.</exception>
        Task<GetCarBookingDTO> CreateCarBookingAsync(CreateCarBookingDTO dto);

        /// <summary>
        /// Updates an existing car booking asynchronously.
        /// </summary>
        /// <param name="dto">The DTO containing updated information for the car booking.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="dto"/> is null.</exception>
        /// <exception cref="KeyNotFoundException">Thrown if the car booking with the specified ID is not found, or if the associated car is not found.</exception>
        /// <exception cref="ArgumentException">Thrown if date or passenger count validations fail.</exception>
        Task UpdateCarBookingAsync(UpdateCarBookingDTO dto);

        /// <summary>
        /// Deletes a car booking by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the car booking to delete.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the car booking with the specified ID is not found.</exception>
        Task DeleteCarBookingAsync(int id);

        /// <summary>
        /// Retrieves all car bookings asynchronously, with filtering based on user role.
        /// </summary>
        /// <returns>A collection of <see cref="GetCarBookingDTO"/> representing all car bookings.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the HTTP context is unavailable when trying to determine user role.</exception>
        Task<IEnumerable<GetCarBookingDTO>> GetAllCarBookingsAsync();

        /// <summary>
        /// Retrieves a car booking by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the car booking.</param>
        /// <returns>A <see cref="GetCarBookingDTO"/> representing the found car booking.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the car booking with the specified ID is not found.</exception>
        Task<GetCarBookingDTO> GetCarBookingByIdAsync(int id);

        /// <summary>
        /// Retrieves all car bookings within a specified date interval asynchronously.
        /// </summary>
        /// <param name="startDate">The start date of the interval.</param>
        /// <param name="endDate">The end date of the interval.</param>
        /// <returns>A collection of <see cref="GetCarBookingDTO"/> that fall within the specified date interval.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="endDate"/> is not after <paramref name="startDate"/>.</exception>
        Task<IEnumerable<GetCarBookingDTO>> GetCarBookingsByDateIntervalAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Confirms a car booking by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the car booking to confirm.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the car booking with the specified ID is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the car booking with the specified ID is confirmed, rejected, or completed.</exception>
        public Task ConfirmCarBookingAsync(int id);

        /// <summary>
        /// Cancels a car booking by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the car booking to cancel.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="KeyNotFoundException">Thrown if the car booking with the specified ID is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the car booking with the specified ID has already started and cannot be cancelled.</exception>
        public Task CancelCarBookingAsync(int id);

    }
}
