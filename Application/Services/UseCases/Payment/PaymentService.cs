using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Application.IServices.UseCases;
using Application.DTOs.Payment;
using Application.DTOs.PaymentTransaction;
using Application.IServices.Validation;
using AutoMapper;
using Microsoft.Extensions.Logging;

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

        public async Task<PaymentProcessResultDTO> ProcessPaymentAsync(ProcessPaymentDTO processPaymentDto)
        {
            try
            {
                // Validate payment exists
                var payment = await _validationService.ValidatePaymentExistsAsync(processPaymentDto.PaymentId);

                // Validate payment method exists and is active
                var paymentMethod = await _validationService.ValidatePaymentMethodExistsAndActiveAsync(processPaymentDto.PaymentMethodId);

                // Validate payment can receive payment
                _validationService.ValidatePaymentCanReceivePayment(payment,processPaymentDto.Amount);

                // Create transaction through PaymentTransactionService
                var createTransactionDto = new CreatePaymentTransactionDTO
                {
                    PaymentId = processPaymentDto.PaymentId,
                    PaymentMethodId = processPaymentDto.PaymentMethodId,
                    Amount = processPaymentDto.Amount,
                    TransactionType = TransactionType.Payment,
                    //TransactionReference = processPaymentDto.TransactionReference,
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

        public async Task<PaymentProcessResultDTO> ProcessRefundAsync(ProcessRefundDTO refundDto)
        {
            try
            {
                // Validate payment exists
                var payment = await _validationService.ValidatePaymentExistsAsync(refundDto.PaymentId);

                // Validate payment method exists and is active
                var paymentMethod = await _validationService.ValidatePaymentMethodExistsAndActiveAsync(refundDto.PaymentMethodId);

                // Validate payment can be refunded
                _validationService.ValidatePaymentCanBeRefunded(payment, refundDto.Amount , refundDto.Reason);

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
                
                
                // Update payment notes with refund information
                payment.Notes = string.IsNullOrEmpty(payment.Notes) 
                    ? $"REFUND: {refundDto.Amount:C} - {refundDto.Reason} (Transaction: {transaction.Id})"
                    : $"{payment.Notes}; REFUND: {refundDto.Amount:C} - {refundDto.Reason} (Transaction: {transaction.Id})";
                
                
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
            var payment = await _validationService.ValidatePaymentExistsAsync(paymentId);
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
            var payment = await _validationService.ValidatePaymentExistsAsync(paymentId);

            var paymentDetails = _mapper.Map<PaymentDetailsDTO>(payment);
            
            // Get all transactions for this payment
            var transactions = await _paymentTransactionService.GetTransactionsByPaymentIdAsync(paymentId);
            paymentDetails.Transactions = transactions.ToList();

            return paymentDetails;
        }

        public async Task<ReturnPaymentDTO> CancelPaymentAsync(int paymentId)
        {
            try
            {
                var payment = await _validationService.ValidatePaymentExistsAsync(paymentId);

                // Validate that payment can be cancelled
                if (payment.Status == PaymentStatus.Paid)
                {
                    _logger.LogWarning("Attempting to cancel a paid payment {PaymentId}", paymentId);
                    throw new InvalidOperationException("Cannot cancel a payment that has been paid. Use refund instead.");
                }

                if (payment.Status == PaymentStatus.Cancelled)
                {
                    _logger.LogWarning("Attempting to cancel an already cancelled payment {PaymentId}", paymentId);
                    throw new InvalidOperationException("Payment is already cancelled.");
                }

                if (payment.Status == PaymentStatus.Refunded)
                {
                    _logger.LogWarning("Attempting to cancel a refunded payment {PaymentId}", paymentId);
                    throw new InvalidOperationException("Cannot cancel a refunded payment.");
                }

                // Update payment status to cancelled
                payment.Status = PaymentStatus.Cancelled;
                _paymentRepository.Update(payment);
                await _paymentRepository.SaveAsync();

                _logger.LogInformation("Payment {PaymentId} has been cancelled", paymentId);
                return _mapper.Map<ReturnPaymentDTO>(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling payment {PaymentId}", paymentId);
                throw;
            }
        }

        public async Task<bool> DeletePaymentAsync(int paymentId)
        {
            try
            {
                var payment = await _validationService.ValidatePaymentExistsAsync(paymentId);

                // Validate that payment can be deleted
                if (payment.Status == PaymentStatus.Paid)
                {
                    _logger.LogWarning("Attempting to delete a paid payment {PaymentId}", paymentId);
                    throw new InvalidOperationException("Cannot delete a payment that has been paid. Cancel or refund the payment first.");
                }

                if (payment.Status == PaymentStatus.PartiallyPaid)
                {
                    _logger.LogWarning("Attempting to delete a partially paid payment {PaymentId}", paymentId);
                    throw new InvalidOperationException("Cannot delete a payment that has been partially paid. Refund the payment first.");
                }

                // Check if there are any transactions associated with this payment
                var transactions = await _paymentTransactionService.GetTransactionsByPaymentIdAsync(paymentId);
                if (transactions.Any())
                {
                    _logger.LogWarning("Attempting to delete payment {PaymentId} with existing transactions", paymentId);
                    throw new InvalidOperationException("Cannot delete a payment with existing transactions. Delete transactions first or cancel the payment instead.");
                }

                await _paymentRepository.DeleteByIdAsync(paymentId);
                await _paymentRepository.SaveAsync();

                _logger.LogInformation("Payment {PaymentId} has been deleted", paymentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting payment {PaymentId}", paymentId);
                throw;
            }
        }
    }
}