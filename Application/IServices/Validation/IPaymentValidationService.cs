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
        /// Validates that a payment method exists and is active asynchronously.
        /// </summary>
        /// <param name="paymentMethodId">The ID of the payment method to validate.</param>
        /// <returns>The payment method entity if it exists and is active.</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the payment method is not found.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the payment method is inactive.</exception>
        Task<PaymentMethod> ValidatePaymentMethodExistsAndActiveAsync(int paymentMethodId);

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