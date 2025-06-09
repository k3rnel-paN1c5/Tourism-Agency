using Application.DTOs.Payment;
using Domain.Enums;

namespace Application.IServices.UseCases
{
    /// <summary>
    /// Defines the contract for payment-related business logic operations.
    /// </summary>
    public interface IPaymentService
    {
        /// <summary>
        /// Creates a new payment asynchronously.
        /// </summary>
        /// <param name="paymentDto">The payment creation data.</param>
        /// <returns>The created payment information.</returns>
        Task<ReturnPaymentDTO> CreatePaymentAsync(CreatePaymentDTO paymentDto);

        /// <summary>
        /// Retrieves all payments asynchronously.
        /// </summary>
        /// <returns>A collection of all payments.</returns>
        Task<IEnumerable<ReturnPaymentDTO>> GetAllPaymentsAsync();

        /// <summary>
        /// Retrieves a payment by its ID asynchronously.
        /// </summary>
        /// <param name="paymentId">The ID of the payment to retrieve.</param>
        /// <returns>The payment information.</returns>
        Task<ReturnPaymentDTO> GetPaymentByIdAsync(int paymentId);

        /// <summary>
        /// Retrieves a payment by booking ID asynchronously.
        /// </summary>
        /// <param name="bookingId">The ID of the booking.</param>
        /// <returns>The payment information for the specified booking.</returns>
        Task<ReturnPaymentDTO> GetPaymentByBookingIdAsync(int bookingId);

        /// <summary>
        /// Retrieves all payments by status asynchronously.
        /// </summary>
        /// <param name="status">The payment status to filter by.</param>
        /// <returns>A collection of payments with the specified status.</returns>
        Task<IEnumerable<ReturnPaymentDTO>> GetPaymentsByStatusAsync(PaymentStatus status);

        /// <summary>
        /// Retrieves detailed information about a payment asynchronously.
        /// </summary>
        /// <param name="paymentId">The ID of the payment.</param>
        /// <returns>Detailed payment information.</returns>
        Task<PaymentDetailsDTO> GetPaymentDetailsAsync(int paymentId);

        /// <summary>
        /// Updates payment information asynchronously.
        /// </summary>
        /// <param name="paymentDto">The payment update data.</param>
        /// <returns>The updated payment information.</returns>
        Task<ReturnPaymentDTO> UpdatePaymentAsync(UpdatePaymentDTO paymentDto);

        /// <summary>
        /// Processes a payment transaction asynchronously.
        /// </summary>
        /// <param name="processPaymentDto">The payment processing data.</param>
        /// <returns>The result of the payment processing operation.</returns>
        Task<PaymentProcessResultDTO> ProcessPaymentAsync(ProcessPaymentDTO processPaymentDto);

        /// <summary>
        /// Processes a refund transaction asynchronously.
        /// </summary>
        /// <param name="refundDto">The refund processing data.</param>
        /// <returns>The result of the refund processing operation.</returns>
        Task<PaymentProcessResultDTO> ProcessRefundAsync(ProcessRefundDTO refundDto);

        /// <summary>
        /// Cancels a payment asynchronously.
        /// </summary>
        /// <param name="paymentId">The ID of the payment to cancel.</param>
        /// <returns>The updated payment information.</returns>
        Task<ReturnPaymentDTO> CancelPaymentAsync(int paymentId);
    }
}