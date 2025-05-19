using Domain.Entities;
using Domain.Enums;
using Application.DTOs.Payment;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IServices.UseCases
{
    public interface IPaymentService
    {
        // Payment Creation
        Task<ReturnPaymentDTO> CreatePaymentAsync(CreatePaymentDTO paymentDto);

        // Payment Retrieval
        Task<IEnumerable<ReturnPaymentDTO>> GetAllPaymentsAsync();
        Task<ReturnPaymentDTO> GetPaymentByIdAsync(int paymentId);
        Task<ReturnPaymentDTO> GetPaymentByBookingIdAsync(int bookingId);
        Task<IEnumerable<ReturnPaymentDTO>> GetPaymentsByStatusAsync(PaymentStatus status);
        
        // Payment Operations
        Task<ReturnPaymentDTO> UpdatePaymentStatusAsync(UpdatePaymentStatusDTO statusDto);
        Task<ReturnPaymentDTO> ProcessRefundAsync(ProcessRefundDTO refundDto);
        
        // Additional Business Methods
        Task<PaymentDetailsDTO> GetPaymentDetailsAsync(int paymentId);
    }
}