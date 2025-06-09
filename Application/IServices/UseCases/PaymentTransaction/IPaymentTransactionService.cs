using Application.DTOs.PaymentTransaction;
using Domain.Enums;

namespace Application.IServices.UseCases
{
    /// <summary>
    /// Defines the contract for payment transaction-related business logic operations.
    /// </summary>
    public interface IPaymentTransactionService
    {
        /// <summary>
        /// Creates a new payment transaction asynchronously.
        /// </summary>
        /// <param name="transactionDto">The payment transaction creation data.</param>
        /// <returns>The created payment transaction information.</returns>
        Task<ReturnPaymentTransactionDTO> CreatePaymentTransactionAsync(CreatePaymentTransactionDTO transactionDto);

        /// <summary>
        /// Retrieves all payment transactions asynchronously.
        /// </summary>
        /// <returns>A collection of all payment transactions.</returns>
        Task<IEnumerable<ReturnPaymentTransactionDTO>> GetAllPaymentTransactionsAsync();

        /// <summary>
        /// Retrieves a payment transaction by its ID asynchronously.
        /// </summary>
        /// <param name="transactionId">The ID of the payment transaction to retrieve.</param>
        /// <returns>The payment transaction information.</returns>
        Task<ReturnPaymentTransactionDTO> GetPaymentTransactionByIdAsync(int transactionId);

        /// <summary>
        /// Retrieves all payment transactions for a specific payment asynchronously.
        /// </summary>
        /// <param name="paymentId">The ID of the payment.</param>
        /// <returns>A collection of payment transactions for the specified payment.</returns>
        Task<IEnumerable<ReturnPaymentTransactionDTO>> GetTransactionsByPaymentIdAsync(int paymentId);

        /// <summary>
        /// Retrieves all payment transactions by transaction type asynchronously.
        /// </summary>
        /// <param name="transactionType">The type of transactions to retrieve.</param>
        /// <returns>A collection of payment transactions of the specified type.</returns>
        Task<IEnumerable<ReturnPaymentTransactionDTO>> GetTransactionsByTypeAsync(TransactionType transactionType);

        /// <summary>
        /// Retrieves all payment transactions for a specific payment method asynchronously.
        /// </summary>
        /// <param name="paymentMethodId">The ID of the payment method.</param>
        /// <returns>A collection of payment transactions for the specified payment method.</returns>
        Task<IEnumerable<ReturnPaymentTransactionDTO>> GetTransactionsByPaymentMethodAsync(int paymentMethodId);

        /// <summary>
        /// Retrieves payment transactions within a date range asynchronously.
        /// </summary>
        /// <param name="startDate">The start date of the range.</param>
        /// <param name="endDate">The end date of the range.</param>
        /// <returns>A collection of payment transactions within the specified date range.</returns>
        Task<IEnumerable<ReturnPaymentTransactionDTO>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Updates payment transaction information asynchronously.
        /// </summary>
        /// <param name="transactionDto">The payment transaction update data.</param>
        /// <returns>The updated payment transaction information.</returns>
        Task<ReturnPaymentTransactionDTO> UpdatePaymentTransactionAsync(UpdatePaymentTransactionDTO transactionDto);

        /// <summary>
        /// Retrieves detailed information about a payment transaction asynchronously.
        /// </summary>
        /// <param name="transactionId">The ID of the payment transaction.</param>
        /// <returns>Detailed payment transaction information.</returns>
        Task<PaymentTransactionDetailsDTO> GetPaymentTransactionDetailsAsync(int transactionId);

        /// <summary>
        /// Calculates the total transaction amount for a specific payment asynchronously.
        /// </summary>
        /// <param name="paymentId">The ID of the payment.</param>
        /// <returns>The total transaction amount for the payment.</returns>
        Task<decimal> GetTotalTransactionAmountByPaymentAsync(int paymentId);

        /// <summary>
        /// Deletes a payment transaction asynchronously.
        /// </summary>
        /// <param name="transactionId">The ID of the payment transaction to delete.</param>
        /// <returns>True if the deletion was successful, otherwise false.</returns>
        Task<bool> DeletePaymentTransactionAsync(int transactionId);

        /// <summary>
        /// Checks if a payment transaction exists asynchronously.
        /// </summary>
        /// <param name="transactionId">The ID of the payment transaction to check.</param>
        /// <returns>True if the payment transaction exists, otherwise false.</returns>
        Task<bool> PaymentTransactionExistsAsync(int transactionId);
    }
}