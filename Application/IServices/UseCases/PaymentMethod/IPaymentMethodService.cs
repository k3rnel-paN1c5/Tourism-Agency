using Domain.Entities;
using DTO.PaymentMethod;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.IServices.UseCases
{
    public interface IPaymentMethodService
    {
        Task<IEnumerable<ReturnPaymentMethodDTO>> GetAllPaymentMethodsAsync();
        Task<IEnumerable<ReturnPaymentMethodDTO>> GetActivePaymentMethodsAsync();
        Task<ReturnPaymentMethodDTO> GetPaymentMethodByIdAsync(int id);
        Task<ReturnPaymentMethodDTO> CreatePaymentMethodAsync(CreatePaymentMethodDTO dto);
        Task<ReturnPaymentMethodDTO> UpdatePaymentMethodAsync(UpdatePaymentMethodDTO dto);
        Task<bool> DeletePaymentMethodAsync(int id);
        Task<ReturnPaymentMethodDTO> TogglePaymentMethodStatusAsync(int id);
    }
}