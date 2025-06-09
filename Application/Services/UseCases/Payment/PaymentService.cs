using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Application.IServices.UseCases;
using Application.DTOs.Payment;
using Application.DTOs.PaymentTransaction;
using Application.Services.Validation;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.UseCases
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Payment, int> _paymentRepository;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly IPaymentValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IRepository<Payment, int> paymentRepository,
            IPaymentTransactionService paymentTransactionService,
            IPaymentValidationService validationService,
            IMapper mapper,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _paymentTransactionService = paymentTransactionService;
            _validationService = validationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReturnPaymentDTO> CreatePaymentAsync(CreatePaymentDTO paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            payment.AmountPaid = 0;
            payment.Status = PaymentStatus.Pending;
            payment.PaymentDate = DateTime.UtcNow;

            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveAsync();

            _logger.LogInformation("Created payment for booking {BookingId}", paymentDto.BookingId);
            return _mapper.Map<ReturnPaymentDTO>(payment);
        }

        // NEW: Comprehensive validation method
        private async Task<(Payment payment, PaymentMethod paymentMethod)> ValidatePaymentAndMethodAsync(
            int paymentId, 
            int paymentMethodId, 
            string operationType = "transaction")
        {
            // Validate payment exists
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                _logger.LogWarning("Payment {PaymentId} not found for {OperationType}", paymentId, operationType);
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found");
            }

            // Validate payment method exists and is active
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(paymentMethodId);
            if (paymentMethod == null)
            {
                _logger.LogWarning("Payment method {PaymentMethodId} not found for {OperationType}", paymentMethodId, operationType);
                throw new KeyNotFoundException($"Payment method with ID {paymentMethodId} not found");
            }

            if (!paymentMethod.IsActive)
            {
                _logger.LogWarning("Inactive payment method {PaymentMethodId} used for {OperationType}", paymentMethodId, operationType);
                throw new InvalidOperationException($"Payment method '{paymentMethod.Method}' is not active");
            }

            // Additional payment status validations
            ValidatePaymentStatusForOperation(payment, operationType);

            return (payment, paymentMethod);
        }

        // NEW: Payment status validation based on operation type
        private void ValidatePaymentStatusForOperation(Payment payment, string operationType)
        {
            switch (operationType.ToLower())
            {
                case "payment":
                case "deposit":
                case "final":
                    if (payment.Status == PaymentStatus.Refunded)
                    {
                        throw new InvalidOperationException("Cannot process payment for fully refunded payments");
                    }
                    if (payment.Status == PaymentStatus.Paid && operationType != "final")
                    {
                        _logger.LogWarning("Attempting to process payment for already paid payment {PaymentId}", payment.Id);
                        // Could be a warning instead of exception based on business rules
                    }
                    break;

                case "refund":
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
                    break;

                default:
                    // General validation for other operations
                    break;
            }
        }

        // NEW: Additional business rule validations
        private void ValidateBusinessRules(Payment payment, decimal amount, TransactionType transactionType)
        {
            switch (transactionType)
            {
                case TransactionType.Payment:
                    if (amount != payment.AmountDue)
                    {
                        throw new InvalidOperationException($"Full payment amount must equal total amount due ({payment.AmountDue:C})");
                    }
                    break;

                case TransactionType.Deposit:
                    var maxDepositAmount = payment.AmountDue * 0.8m; // 80% max deposit rule
                    if (amount > maxDepositAmount)
                    {
                        throw new InvalidOperationException($"Deposit amount cannot exceed {maxDepositAmount:C} (80% of total amount)");
                    }
                    if (amount >= payment.AmountDue)
                    {
                        throw new InvalidOperationException("Deposit amount should be less than total amount due. Use 'Payment' type for full payment");
                    }
                    break;

                case TransactionType.Final:
                    var remainingBalance = payment.AmountDue - payment.AmountPaid;
                    if (remainingBalance <= 0)
                    {
                        throw new InvalidOperationException("No remaining balance to pay");
                    }
                    if (amount != remainingBalance)
                    {
                        throw new InvalidOperationException($"Final payment amount must equal remaining balance ({remainingBalance:C})");
                    }
                    break;

                case TransactionType.Refund:
                    if (amount > payment.AmountPaid)
                    {
                        throw new InvalidOperationException($"Refund amount cannot exceed paid amount ({payment.AmountPaid:C})");
                    }
                    break;
            }

            // General amount validation
            if (amount <= 0)
            {
                throw new ArgumentException("Transaction amount must be greater than zero");
            }
        }

        // Updated ProcessPaymentAsync method
        public async Task<PaymentProcessResultDTO> ProcessPaymentAsync(ProcessPaymentDTO processPaymentDto)
        {
            try
            {
                // Validate payment and payment method
                var (payment, paymentMethod) = await ValidatePaymentAndMethodAsync(
                    processPaymentDto.PaymentId, 
                    processPaymentDto.PaymentMethodId, 
                    processPaymentDto.TransactionType.ToString());

                // Additional business rule validations
                ValidateBusinessRules(payment, processPaymentDto.Amount, processPaymentDto.TransactionType);

                // Create transaction through PaymentTransactionService
                var createTransactionDto = new CreatePaymentTransactionDTO
                {
                    PaymentId = processPaymentDto.PaymentId,
                    PaymentMethodId = processPaymentDto.PaymentMethodId,
                    Amount = processPaymentDto.Amount,
                    TransactionType = processPaymentDto.TransactionType,
                    TransactionReference = processPaymentDto.TransactionReference,
                    Notes = processPaymentDto.Notes
                };

                var transaction = await _paymentTransactionService.CreatePaymentTransactionAsync(createTransactionDto);

                // Update payment status based on total paid amount
                await UpdatePaymentStatusAfterTransaction(payment.Id);

                var updatedPayment = await _paymentRepository.GetByIdAsync(payment.Id);
                
                _logger.LogInformation("Processed payment {PaymentId} with transaction {TransactionId} using {PaymentMethod}", 
                    payment.Id, transaction.Id, paymentMethod.Method);

                return new PaymentProcessResultDTO
                {
                    Payment = _mapper.Map<ReturnPaymentDTO>(updatedPayment),
                    Transaction = transaction,
                    Success = true,
                    Message = $"Payment processed successfully via {paymentMethod.Method}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment {PaymentId}", processPaymentDto.PaymentId);
                throw;
            }
        }

        // Updated ProcessRefundAsync method
        public async Task<PaymentProcessResultDTO> ProcessRefundAsync(ProcessRefundDTO refundDto)
        {
            try
            {
                // Validate payment and payment method
                var (payment, paymentMethod) = await ValidatePaymentAndMethodAsync(
                    refundDto.PaymentId, 
                    refundDto.PaymentMethodId, 
                    "refund");

                // Additional business rule validations for refund
                ValidateBusinessRules(payment, refundDto.Amount, TransactionType.Refund);

                // Create refund transaction
                var createTransactionDto = new CreatePaymentTransactionDTO
                {
                    PaymentId = refundDto.PaymentId,
                    PaymentMethodId = refundDto.PaymentMethodId,
                    Amount = refundDto.Amount,
                    TransactionType = TransactionType.Refund,
                    Notes = $"REFUND: {refundDto.Reason}"
                };

                var transaction = await _paymentTransactionService.CreatePaymentTransactionAsync(createTransactionDto);

                // Update payment status after refund
                await UpdatePaymentStatusAfterTransaction(payment.Id);

                var updatedPayment = await _paymentRepository.GetByIdAsync(payment.Id);

                _logger.LogInformation("Processed refund of {Amount} for payment {PaymentId} via {PaymentMethod}", 
                    refundDto.Amount, refundDto.PaymentId, paymentMethod.Method);

                return new PaymentProcessResultDTO
                {
                    Payment = _mapper.Map<ReturnPaymentDTO>(updatedPayment),
                    Transaction = transaction,
                    Success = true,
                    Message = $"Refund of {refundDto.Amount:C} processed successfully via {paymentMethod.Method}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund for payment {PaymentId}", refundDto.PaymentId);
                throw;
            }
        }
        
        private async Task UpdatePaymentStatusAfterTransaction(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            var totalPaid = await _paymentTransactionService.GetTotalTransactionAmountByPaymentAsync(paymentId);

            payment.AmountPaid = totalPaid;

            if (totalPaid <= 0)
            {
                payment.Status = totalPaid == 0 ? PaymentStatus.Pending : PaymentStatus.Refunded;
            }
            else if (totalPaid >= payment.AmountDue)
            {
                payment.Status = PaymentStatus.Paid;
                if (payment.PaymentDate == null)
                {
                    payment.PaymentDate = DateTime.UtcNow;
                }
            }
            else
            {
                payment.Status = PaymentStatus.PartiallyPaid;
            }

            _paymentRepository.Update(payment);
            await _paymentRepository.SaveAsync();
        }

        public async Task<IEnumerable<ReturnPaymentDTO>> GetAllPaymentsAsync()
        {
            var payments = await _paymentRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ReturnPaymentDTO>>(payments);
        }

        public async Task<ReturnPaymentDTO> GetPaymentByIdAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                _logger.LogWarning("Payment {PaymentId} not found", paymentId);
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found");
            }

            return _mapper.Map<ReturnPaymentDTO>(payment);
        }

        public async Task<ReturnPaymentDTO> GetPaymentByBookingIdAsync(int bookingId)
        {
            var payment = (await _paymentRepository.GetAllByPredicateAsync(p => p.BookingId == bookingId))
                .FirstOrDefault();

            if (payment == null)
            {
                _logger.LogWarning("Payment for booking {BookingId} not found", bookingId);
                throw new KeyNotFoundException($"Payment for booking ID {bookingId} not found");
            }

            return _mapper.Map<ReturnPaymentDTO>(payment);
        }

        public async Task<IEnumerable<ReturnPaymentDTO>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            var payments = await _paymentRepository.GetAllByPredicateAsync(p => p.Status == status);
            return _mapper.Map<IEnumerable<ReturnPaymentDTO>>(payments);
        }

        public async Task<PaymentDetailsDTO> GetPaymentDetailsAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found");
            }

            var paymentDetails = _mapper.Map<PaymentDetailsDTO>(payment);
            
            // Get all transactions for this payment
            var transactions = await _paymentTransactionService.GetTransactionsByPaymentIdAsync(paymentId);
            paymentDetails.Transactions = transactions.ToList();

            return paymentDetails;
        }
    }
}