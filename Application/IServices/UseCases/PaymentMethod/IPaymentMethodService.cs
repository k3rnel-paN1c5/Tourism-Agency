using Application.DTOs.PaymentMethod;

namespace Application.IServices.UseCases
{
    /// <summary>
    /// Interface for payment method-related operations.
    /// </summary>
    public interface IPaymentMethodService
    {
        /// <summary>
        /// Creates a new payment method asynchronously.
        /// </summary>
        /// <param name="paymentMethodDto">The payment method creation data.</param>
        /// <returns>The created payment method information.</returns>
        Task<ReturnPaymentMethodDTO> CreatePaymentMethodAsync(CreatePaymentMethodDTO paymentMethodDto);

        /// <summary>
        /// Retrieves a payment method by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the payment method to retrieve.</param>
        /// <returns>The payment method information.</returns>
        Task<ReturnPaymentMethodDTO> GetPaymentMethodByIdAsync(int id);

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
        /// Updates an existing payment method asynchronously.
        /// </summary>
        /// <param name="paymentMethodDto">The payment method update data.</param>
        /// <returns>The updated payment method information.</returns>
        Task<ReturnPaymentMethodDTO> UpdatePaymentMethodAsync(UpdatePaymentMethodDTO paymentMethodDto);

        /// <summary>
        /// Deletes a payment method asynchronously.
        /// </summary>
        /// <param name="id">The ID of the payment method to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        Task<bool> DeletePaymentMethodAsync(int id);

        /// <summary>
        /// Toggles the active status of a payment method asynchronously.
        /// </summary>
        /// <param name="id">The ID of the payment method to toggle.</param>
        /// <returns>The updated payment method information.</returns>
        Task<ReturnPaymentMethodDTO> TogglePaymentMethodStatusAsync(int id);
    }
}
