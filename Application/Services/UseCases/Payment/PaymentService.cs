using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Application.IServices.UseCases;
using Application.DTOs.Payment;
using Application.DTOs.PaymentTransaction;
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
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IRepository<Payment, int> paymentRepository,
            IPaymentTransactionService paymentTransactionService,
            IMapper mapper,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _paymentTransactionService = paymentTransactionService;
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

        // Main payment processing method
        public async Task<PaymentProcessResultDTO> ProcessPaymentAsync(ProcessPaymentDTO processPaymentDto)
        {
            var payment = await _paymentRepository.GetByIdAsync(processPaymentDto.PaymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {processPaymentDto.PaymentId} not found");
            }

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
            
            _logger.LogInformation("Processed payment {PaymentId} with transaction {TransactionId}", 
                payment.Id, transaction.Id);

            return new PaymentProcessResultDTO
            {
                Payment = _mapper.Map<ReturnPaymentDTO>(updatedPayment),
                Transaction = transaction,
                Success = true,
                Message = "Payment processed successfully"
            };
        }

        // NEW: Process refund through transaction service
        public async Task<PaymentProcessResultDTO> ProcessRefundAsync(ProcessRefundDTO refundDto)
        {
            var payment = await _paymentRepository.GetByIdAsync(refundDto.PaymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {refundDto.PaymentId} not found");
            }

            // Create refund transaction
            var createTransactionDto = new CreatePaymentTransactionDTO
            {
                PaymentId = refundDto.PaymentId,
                PaymentMethodId = refundDto.PaymentMethodId, // Original payment method or refund method
                Amount = refundDto.Amount,
                TransactionType = TransactionType.Refund,
                Notes = $"REFUND: {refundDto.Reason}"
            };

            var transaction = await _paymentTransactionService.CreatePaymentTransactionAsync(createTransactionDto);

            // Update payment status after refund
            await UpdatePaymentStatusAfterTransaction(payment.Id);

            var updatedPayment = await _paymentRepository.GetByIdAsync(payment.Id);

            _logger.LogInformation("Processed refund of {Amount} for payment {PaymentId}", 
                refundDto.Amount, refundDto.PaymentId);

            return new PaymentProcessResultDTO
            {
                Payment = _mapper.Map<ReturnPaymentDTO>(updatedPayment),
                Transaction = transaction,
                Success = true,
                Message = "Refund processed successfully"
            };
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