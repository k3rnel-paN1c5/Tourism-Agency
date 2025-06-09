using Domain.Entities;
using Application.DTOs.PaymentMethod;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IServices.UseCases
{
    /// <summary>
    /// Defines the contract for payment method-related business logic operations.
    /// </summary>
    public interface IPaymentMethodService
    {
        /// <summary>
        /// Retrieves all payment methods asynchronously.
        /// </summary>
        /// <returns>A collection of all payment methods.</returns>
        Task<IEnumerable<ReturnPaymentMethodDTO>> GetAllPaymentMethodsAsync();

        /// <summary>
        /// Retrieves all active payment methods asynchronously.
        /// </summary>
        /// <returns>A collection of active payment methods.</returns>
        Task<IEnumerable<ReturnPaymentMethodDTO>> GetActivePaymentMethodsAsync();

        /// <summary>
        /// Retrieves a payment method by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the payment method to retrieve.</param>
        /// <returns>The payment method information.</returns>
        Task<ReturnPaymentMethodDTO> GetPaymentMethodByIdAsync(int id);

        /// <summary>
        /// Creates a new payment method asynchronously.
        /// </summary>
        /// <param name="dto">The payment method creation data.</param>
        /// <returns>The created payment method information.</returns>
        Task<ReturnPaymentMethodDTO> CreatePaymentMethodAsync(CreatePaymentMethodDTO dto);

        /// <summary>
        /// Updates payment method information asynchronously.
        /// </summary>
        /// <param name="dto">The payment method update data.</param>
        /// <returns>The updated payment method information.</returns>
        Task<ReturnPaymentMethodDTO> UpdatePaymentMethodAsync(UpdatePaymentMethodDTO dto);

        /// <summary>
        /// Deletes a payment method asynchronously.
        /// </summary>
        /// <param name="id">The ID of the payment method to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        Task<bool> DeletePaymentMethodAsync(int id);

        /// <summary>
        /// Toggles the status of a payment method asynchronously.
        /// </summary>
        /// <param name="id">The ID of the payment method to toggle.</param>
        /// <returns>The updated payment method information.</returns>
        Task<ReturnPaymentMethodDTO> TogglePaymentMethodStatusAsync(int id);

        /// <summary>
        /// Checks if a payment method exists asynchronously.
        /// </summary>
        /// <param name="id">The ID of the payment method to check.</param>
        /// <returns>True if the payment method exists, otherwise false.</returns>
        Task<bool> PaymentMethodExistsAsync(int id);
    }
}