using Application.IServices.UseCases;
using Domain.Entities;
using Domain.IRepositories;
using DTO.PaymentMethod;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services.UseCases
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IRepository<PaymentMethod, int> _repository;
        private readonly ILogger<PaymentMethodService> _logger;

        public PaymentMethodService(
            IRepository<PaymentMethod, int> repository,
            ILogger<PaymentMethodService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<ReturnPaymentMethodDTO> CreatePaymentMethodAsync(CreatePaymentMethodDTO paymentMethodDto)
        {
            var paymentMethod = new PaymentMethod
            {
                Method = paymentMethodDto.Method,
                Icon = paymentMethodDto.Icon
            };

            await _repository.AddAsync(paymentMethod);
            await _repository.SaveAsync();

            _logger.LogInformation("Created payment method: {Method}", paymentMethod.Method);
            return MapToReturnDto(paymentMethod);
        }

        public async Task<ReturnPaymentMethodDTO> GetPaymentMethodByIdAsync(int id)
        {
            var paymentMethod = await _repository.GetByIdAsync(id);
            if (paymentMethod == null)
            {
                _logger.LogWarning("Payment method {Id} not found", id);
                throw new KeyNotFoundException($"Payment method with ID {id} not found");
            }

            return MapToReturnDto(paymentMethod);
        }

        public async Task<IEnumerable<ReturnPaymentMethodDTO>> GetAllPaymentMethodsAsync()
        {
            var paymentMethods = await _repository.GetAllAsync();
            var dtos = new List<ReturnPaymentMethodDTO>();

            foreach (var pm in paymentMethods)
            {
                dtos.Add(MapToReturnDto(pm));
            }

            return dtos;
        }

        public async Task<ReturnPaymentMethodDTO> UpdatePaymentMethodAsync(UpdatePaymentMethodDTO paymentMethodDto)
        {
            var paymentMethod = await _repository.GetByIdAsync(paymentMethodDto.Id);
            if (paymentMethod == null)
            {
                _logger.LogWarning("Payment method {Id} not found for update", paymentMethodDto.Id);
                throw new KeyNotFoundException($"Payment method with ID {paymentMethodDto.Id} not found");
            }

            // Partial update - only update fields that were provided
            if (!string.IsNullOrEmpty(paymentMethodDto.Method))
            {
                paymentMethod.Method = paymentMethodDto.Method;
            }

            if (!string.IsNullOrEmpty(paymentMethodDto.Icon))
            {
                paymentMethod.Icon = paymentMethodDto.Icon;
            }

            _repository.Update(paymentMethod);
            await _repository.SaveAsync();

            _logger.LogInformation("Updated payment method {Id}", paymentMethod.Id);
            return MapToReturnDto(paymentMethod);
        }

        public async Task<bool> DeletePaymentMethodAsync(int id)
        {
            var paymentMethod = await _repository.GetByIdAsync(id);
            if (paymentMethod == null)
            {
                _logger.LogWarning("Payment method {Id} not found for deletion", id);
                return false;
            }

            // Check for existing transactions
            if (paymentMethod.PaymentTransactions?.Count > 0)
            {
                _logger.LogWarning("Cannot delete payment method {Id} with existing transactions", id);
                throw new InvalidOperationException("Cannot delete payment method with existing transactions");
            }

            _repository.Delete(paymentMethod);
            await _repository.SaveAsync();

            _logger.LogInformation("Deleted payment method {Id}", id);
            return true;
        }

        private ReturnPaymentMethodDTO MapToReturnDto(PaymentMethod paymentMethod)
        {
            return new ReturnPaymentMethodDTO
            {
                Id = paymentMethod.Id,
                Method = paymentMethod.Method!,
                Icon = paymentMethod.Icon!
            };
        }
    }
}