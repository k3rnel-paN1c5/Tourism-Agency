using Application.IServices.Validation;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Microsoft.Extensions.Logging;

namespace Application.Services.Validation
{
    /// <summary>
    /// Provides validation services for payment-related operations.
    /// </summary>
    public class PaymentValidationService : IPaymentValidationService
    {
        private readonly IRepository<Payment, int> _paymentRepository;
        private readonly IRepository<TransactionMethod, int> _TransactionMethodRepository;
        private readonly ILogger<PaymentValidationService> _logger;

        public PaymentValidationService(
            IRepository<Payment, int> paymentRepository,
            IRepository<TransactionMethod, int> TransactionMethodRepository,
            ILogger<PaymentValidationService> logger)
        {
            _paymentRepository = paymentRepository;
            _TransactionMethodRepository = TransactionMethodRepository;
            _logger = logger;
        }

        public async Task<Payment> ValidatePaymentExistsAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                _logger.LogWarning("Payment {PaymentId} not found", paymentId);
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found");
            }
            return payment;
        }

        public async Task<TransactionMethod> ValidateTransactionMethodExistsAndActiveAsync(int TransactionMethodId)
        {
            var TransactionMethod = await _TransactionMethodRepository.GetByIdAsync(TransactionMethodId);
            if (TransactionMethod == null)
            {
                _logger.LogWarning("Transaction Method {TransactionMethodId} not found", TransactionMethodId);
                throw new KeyNotFoundException($"Transaction Method with ID {TransactionMethodId} not found");
            }

            if (!TransactionMethod.IsActive)
            {
                _logger.LogWarning("Transaction Method {TransactionMethodId} is inactive", TransactionMethodId);
                throw new InvalidOperationException($"Transaction Method '{TransactionMethod.Method}' is not active");
            }

            return TransactionMethod;
        }

        public void ValidatePaymentCanReceivePayment(Payment payment, decimal paymentAmount)
        {
            // 1. Status Validation
            if (payment.Status == PaymentStatus.Refunded)
                throw new InvalidOperationException("Cannot process payment for fully refunded payments");

            if (payment.Status == PaymentStatus.Paid)
            {
                _logger.LogWarning("Attempting to process payment for already paid payment {PaymentId}", payment.Id);
                throw new InvalidOperationException("Payment is already fully paid");
            }

            if (payment.Status == PaymentStatus.Failed)
                throw new InvalidOperationException("Failed payments must be retried explicitly");

            // 2. Amount Validation
            if (paymentAmount <= 0)
                throw new ArgumentException("Payment amount must be positive", nameof(paymentAmount));

            if (paymentAmount > payment.AmountDue * 1.1m) // Allow 10% overpayment (e.g., for tips/fees)
                throw new InvalidOperationException($"Payment amount exceeds allowed limit (Max: {payment.AmountDue * 1.1m:C})");

            // 3. Temporal Validation
            if (payment.PaymentDate < DateTime.UtcNow.AddYears(-1))
                throw new InvalidOperationException("Payments older than 1 year cannot be processed");

            // 4. Partial Payment Logic
            if (payment.Status == PaymentStatus.PartiallyPaid && paymentAmount > (payment.AmountDue - payment.AmountPaid))
                throw new InvalidOperationException(
                    $"Partial payment exceeds remaining balance. Remaining: {payment.AmountDue - payment.AmountPaid:C}");
        }

        public void ValidatePaymentCanBeRefunded(Payment payment, decimal refundAmount, string reason)
        {
            //  Status Validation 
            if (payment.Status == PaymentStatus.Pending)
                throw new InvalidOperationException("Cannot refund pending payments - no payments received");
        
            if (payment.Status == PaymentStatus.Failed)
                throw new InvalidOperationException("Failed payments cannot be refunded");
        
            if (payment.Status == PaymentStatus.Refunded)
                throw new InvalidOperationException("Payment is already fully refunded");
        
            //   Amount Validation 
            if (payment.AmountPaid <= 0)
                throw new InvalidOperationException("No payments available to refund");
        
            if (refundAmount <= 0)
                throw new ArgumentException("Refund amount must be positive", nameof(refundAmount));
        
            
            //  Refund Reason Validation 
            if (string.IsNullOrWhiteSpace(reason) || reason.Length < 20)
                throw new ArgumentException(
                    "Refund reason must be provided with at least 20 characters of explanation", 
                    nameof(reason));
        
            //  Partial Refund Rules 
            decimal minimumPartialRefund = payment.AmountDue * 0.1m; // 10% of original amount
            if (refundAmount < minimumPartialRefund && refundAmount != payment.AmountPaid)
                throw new InvalidOperationException(
                    $"Partial refunds must be ≥ {minimumPartialRefund:C} or match full paid amount");
        
            decimal maxRefundAllowed = payment.AmountPaid * 0.8m; // 80% limit
            
            if (refundAmount > maxRefundAllowed)
                throw new InvalidOperationException(
                    $"Refund amount ({refundAmount:C}) exceeds maximum allowed ({maxRefundAllowed:C} = 80% of paid amount)");
        
            //  Temporal Constraints 
            var refundDeadline = payment.PaymentDate!.Value.AddDays(14);
            if (DateTime.UtcNow > refundDeadline)
                throw new InvalidOperationException(
                    $"Refunds allowed within 14 days only. Deadline passed on {refundDeadline:yyyy-MM-dd}");
        
            //  Idempotency Check 
            if (payment.Notes?.Contains("REFUND:") == true)
                throw new InvalidOperationException("Duplicate refund attempt detected");
        
        }

    }
}