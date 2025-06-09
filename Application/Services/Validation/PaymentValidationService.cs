using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Microsoft.Extensions.Logging;

namespace Application.Services.Validation
{
    public interface IPaymentValidationService
    {
        Task<Payment> ValidatePaymentExistsAsync(int paymentId);
        Task<PaymentMethod> ValidatePaymentMethodExistsAndActiveAsync(int paymentMethodId);
        void ValidatePaymentCanReceivePayment(Payment payment);
        void ValidatePaymentCanBeRefunded(Payment payment);
        void ValidateTransactionAmount(decimal amount);
        void ValidateRefundAmount(Payment payment, decimal refundAmount);
    }

    public class PaymentValidationService : IPaymentValidationService
    {
        private readonly IRepository<Payment, int> _paymentRepository;
        private readonly IRepository<PaymentMethod, int> _paymentMethodRepository;
        private readonly ILogger<PaymentValidationService> _logger;

        public PaymentValidationService(
            IRepository<Payment, int> paymentRepository,
            IRepository<PaymentMethod, int> paymentMethodRepository,
            ILogger<PaymentValidationService> logger)
        {
            _paymentRepository = paymentRepository;
            _paymentMethodRepository = paymentMethodRepository;
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

        public async Task<PaymentMethod> ValidatePaymentMethodExistsAndActiveAsync(int paymentMethodId)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(paymentMethodId);
            if (paymentMethod == null)
            {
                _logger.LogWarning("Payment method {PaymentMethodId} not found", paymentMethodId);
                throw new KeyNotFoundException($"Payment method with ID {paymentMethodId} not found");
            }

            if (!paymentMethod.IsActive)
            {
                _logger.LogWarning("Payment method {PaymentMethodId} is inactive", paymentMethodId);
                throw new InvalidOperationException($"Payment method '{paymentMethod.Method}' is not active");
            }

            return paymentMethod;
        }

        public void ValidatePaymentCanReceivePayment(Payment payment)
        {
            if (payment.Status == PaymentStatus.Refunded)
            {
                throw new InvalidOperationException("Cannot process payment for fully refunded payments");
            }

            if (payment.Status == PaymentStatus.Paid)
            {
                _logger.LogWarning("Attempting to process payment for already paid payment {PaymentId}", payment.Id);
                throw new InvalidOperationException("Payment is already fully paid");
            }
        }

        public void ValidatePaymentCanBeRefunded(Payment payment)
        {
            if (payment.Status == PaymentStatus.Pending)
            {
                throw new InvalidOperationException("Cannot refund pending payments - no payments have been made");
            }

            if (payment.Status == PaymentStatus.Refunded)
            {
                throw new InvalidOperationException("Payment is already fully refunded");
            }

            if (payment.AmountPaid <= 0)
            {
                throw new InvalidOperationException("Cannot refund - no payments have been made");
            }
        }

        public void ValidateTransactionAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Transaction amount must be greater than zero");
            }
        }

        public void ValidateRefundAmount(Payment payment, decimal refundAmount)
        {
            ValidateTransactionAmount(refundAmount);
            
            if (refundAmount > payment.AmountPaid)
            {
                throw new InvalidOperationException($"Refund amount ({refundAmount:C}) cannot exceed paid amount ({payment.AmountPaid:C})");
            }
        }
    }
}