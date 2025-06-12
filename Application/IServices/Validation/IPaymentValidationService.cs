using Domain.Entities;

namespace Application.IServices.Validation
{
    /// <summary>
    /// Defines the contract for payment validation operations.
    /// </summary>
    public interface IPaymentValidationService
    {
        /// <summary>
        /// Validates that a payment exists and retrieves it asynchronously.
        /// </summary>
        /// <param name="paymentId">The ID of the payment to validate.</param>
        /// <returns>The payment entity if it exists.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the payment is not found.</exception>
        Task<Payment> ValidatePaymentExistsAsync(int paymentId);

        /// <summary>
        /// Validates that a Transaction Method exists and is active asynchronously.
        /// </summary>
        /// <param name="TransactionMethodId">The ID of the Transaction Method to validate.</param>
        /// <returns>The Transaction Method entity if it exists and is active.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the Transaction Method is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the Transaction Method is inactive.</exception>
        Task<TransactionMethod> ValidateTransactionMethodExistsAndActiveAsync(int TransactionMethodId);

        /// <summary>
        /// Validates that a payment can receive additional payments.
        /// </summary>
        /// <param name="payment">The payment to validate.</param>
        /// <exception cref="InvalidOperationException">Thrown when the payment cannot receive payments.</exception>
        void ValidatePaymentCanReceivePayment(Payment payment, decimal paymentAmount);

        /// <summary>
        /// Validates that a payment can be refunded.
        /// </summary>
        /// <param name="payment">The payment to validate.</param>
        /// <exception cref="InvalidOperationException">Thrown when the payment cannot be refunded.</exception>
        void ValidatePaymentCanBeRefunded(Payment payment, decimal refundAmount, string reason);

    }
}