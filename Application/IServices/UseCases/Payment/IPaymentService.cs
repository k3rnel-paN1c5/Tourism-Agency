using Application.DTOs.Payment;
using Domain.Enums;

namespace Application.IServices.UseCases
{
    public interface IPaymentService
    {
        
        Task<ReturnPaymentDTO> CreatePaymentAsync(CreatePaymentDTO paymentDto);
        Task<IEnumerable<ReturnPaymentDTO>> GetAllPaymentsAsync();
        Task<ReturnPaymentDTO> GetPaymentByIdAsync(int paymentId);
        Task<ReturnPaymentDTO> GetPaymentByBookingIdAsync(int bookingId);
        Task<IEnumerable<ReturnPaymentDTO>> GetPaymentsByStatusAsync(PaymentStatus status);
        Task<PaymentDetailsDTO> GetPaymentDetailsAsync(int paymentId);
        
        // new integrated methods:
        Task<PaymentProcessResultDTO> ProcessPaymentAsync(ProcessPaymentDTO processPaymentDto);
        Task<PaymentProcessResultDTO> ProcessRefundAsync(ProcessRefundDTO refundDto);
    }
}