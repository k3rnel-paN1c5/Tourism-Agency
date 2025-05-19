using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Application.IServices.UseCases;
using Application.DTOs.Payment;
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
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(
            IRepository<Payment, int> paymentRepository,
            IMapper mapper,
            ILogger<PaymentService> logger)
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReturnPaymentDTO> CreatePaymentAsync(CreatePaymentDTO paymentDto)
        {
            var payment = _mapper.Map<Payment>(paymentDto);
            payment.AmountPaid = 0;
            payment.Status = PaymentStatus.Pending;

            await _paymentRepository.AddAsync(payment);
            await _paymentRepository.SaveAsync();

            _logger.LogInformation("Created payment for booking {BookingId}", paymentDto.BookingId);
            return _mapper.Map<ReturnPaymentDTO>(payment);
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

        public async Task<ReturnPaymentDTO> UpdatePaymentStatusAsync(UpdatePaymentStatusDTO statusDto)
        {
            var payment = await _paymentRepository.GetByIdAsync(statusDto.PaymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {statusDto.PaymentId} not found");
            }

            if (payment.Status == PaymentStatus.Refunded)
            {
                throw new InvalidOperationException("Cannot modify status of refunded payments");
            }

            payment.Status = statusDto.NewStatus;

            if (statusDto.NewStatus == PaymentStatus.Paid && payment.PaymentDate == null)
            {
                payment.PaymentDate = DateTime.UtcNow;
            }

            _paymentRepository.Update(payment);
            await _paymentRepository.SaveAsync();

            _logger.LogInformation("Updated status for payment {PaymentId} to {Status}", 
                statusDto.PaymentId, statusDto.NewStatus);
            
            return _mapper.Map<ReturnPaymentDTO>(payment);
        }

        public async Task<ReturnPaymentDTO> ProcessRefundAsync(ProcessRefundDTO refundDto)
        {
            var payment = await _paymentRepository.GetByIdAsync(refundDto.PaymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {refundDto.PaymentId} not found");
            }

            if (payment.Status != PaymentStatus.Paid)
            {
                throw new InvalidOperationException("Only completed payments can be refunded");
            }

            if (refundDto.Amount <= 0 || refundDto.Amount > payment.AmountPaid)
            {
                throw new ArgumentException("Invalid refund amount");
            }

            payment.AmountPaid -= refundDto.Amount;
            payment.Notes = $"REFUND: {refundDto.Reason}";
            payment.Status = payment.AmountPaid == 0 ? 
                PaymentStatus.Refunded : 
                PaymentStatus.PartiallyRefunded;

            _paymentRepository.Update(payment);
            await _paymentRepository.SaveAsync();

            _logger.LogInformation("Processed refund of {Amount} for payment {PaymentId}", 
                refundDto.Amount, refundDto.PaymentId);
            
            return _mapper.Map<ReturnPaymentDTO>(payment);
        }

        public async Task<PaymentDetailsDTO> GetPaymentDetailsAsync(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            if (payment == null)
            {
                throw new KeyNotFoundException($"Payment with ID {paymentId} not found");
            }

            return _mapper.Map<PaymentDetailsDTO>(payment);
        }
    }
}