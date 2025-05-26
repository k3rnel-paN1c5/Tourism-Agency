
using Domain.Entities;
using DTO.PaymentMethod;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IServices.UseCases
{
    public interface IPaymentMethodService
    {
        Task<ReturnPaymentMethodDTO> CreatePaymentMethodAsync(CreatePaymentMethodDTO paymentMethodDto);
        Task<ReturnPaymentMethodDTO> GetPaymentMethodByIdAsync(int id);
        Task<IEnumerable<ReturnPaymentMethodDTO>> GetAllPaymentMethodsAsync();
        Task<ReturnPaymentMethodDTO> UpdatePaymentMethodAsync(UpdatePaymentMethodDTO paymentMethodDto);
        Task<bool> DeletePaymentMethodAsync(int id);
    }
}