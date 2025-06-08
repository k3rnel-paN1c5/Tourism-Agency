using Application.IServices.UseCases;
using Domain.Entities;
using Domain.IRepositories;
using DTO.PaymentMethod;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.UseCases
{
    public class PaymentMethodService : IPaymentMethodService
    {
        private readonly IRepository<PaymentMethod, int> _paymentMethodRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentMethodService> _logger;

        public PaymentMethodService(
            IRepository<PaymentMethod, int> paymentMethodRepository,
            IMapper mapper,
            ILogger<PaymentMethodService> logger)
        {
            _paymentMethodRepository = paymentMethodRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReturnPaymentMethodDTO> CreatePaymentMethodAsync(CreatePaymentMethodDTO paymentMethodDto)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(paymentMethodDto.Method))
            {
                throw new ArgumentException("Payment method name is required", nameof(paymentMethodDto.Method));
            }

            // Check for duplicate method names
            var existingMethods = await _paymentMethodRepository.GetAllByPredicateAsync(
                pm => pm.Method!.ToLower() == paymentMethodDto.Method.Trim().ToLower());
            
            if (existingMethods.Any())
            {
                throw new InvalidOperationException($"Payment method '{paymentMethodDto.Method}' already exists");
            }

            var paymentMethod = _mapper.Map<PaymentMethod>(paymentMethodDto);

            await _paymentMethodRepository.AddAsync(paymentMethod);
            await _paymentMethodRepository.SaveAsync();

            _logger.LogInformation("Created payment method: {Method} (Active: {IsActive})", 
                paymentMethod.Method, paymentMethod.IsActive);
            return _mapper.Map<ReturnPaymentMethodDTO>(paymentMethod);
        }

        public async Task<ReturnPaymentMethodDTO> GetPaymentMethodByIdAsync(int id)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id);
            if (paymentMethod == null)
            {
                _logger.LogWarning("Payment method {Id} not found", id);
                throw new KeyNotFoundException($"Payment method with ID {id} not found");
            }

            return _mapper.Map<ReturnPaymentMethodDTO>(paymentMethod);
        }

        public async Task<IEnumerable<ReturnPaymentMethodDTO>> GetAllPaymentMethodsAsync()
        {
            var paymentMethods = await _paymentMethodRepository.GetAllAsync();
            _logger.LogDebug("Retrieved {Count} payment methods", paymentMethods.Count());
            return _mapper.Map<IEnumerable<ReturnPaymentMethodDTO>>(paymentMethods);
        }

        public async Task<IEnumerable<ReturnPaymentMethodDTO>> GetActivePaymentMethodsAsync()
        {
            var activeMethods = await _paymentMethodRepository.GetAllByPredicateAsync(pm => pm.IsActive);
            _logger.LogDebug("Retrieved {Count} active payment methods", activeMethods.Count());
            return _mapper.Map<IEnumerable<ReturnPaymentMethodDTO>>(activeMethods);
        }

        public async Task<ReturnPaymentMethodDTO> UpdatePaymentMethodAsync(UpdatePaymentMethodDTO paymentMethodDto)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(paymentMethodDto.Id);
            if (paymentMethod == null)
            {
                throw new KeyNotFoundException($"Payment method with ID {paymentMethodDto.Id} not found");
            }

            // Check for duplicate method names (excluding current record)
            if (!string.IsNullOrWhiteSpace(paymentMethodDto.Method))
            {
                var existingMethods = await _paymentMethodRepository.GetAllByPredicateAsync(
                    pm => pm.Id != paymentMethodDto.Id && 
                          pm.Method!.ToLower() == paymentMethodDto.Method.Trim().ToLower());
                
                if (existingMethods.Any())
                {
                    throw new InvalidOperationException($"Payment method '{paymentMethodDto.Method}' already exists");
                }
            }

            // Use AutoMapper to update the entity
            _mapper.Map(paymentMethodDto, paymentMethod);

            _paymentMethodRepository.Update(paymentMethod);
            await _paymentMethodRepository.SaveAsync();

            _logger.LogInformation("Updated payment method {Id}: {Method} (Active: {IsActive})", 
                paymentMethod.Id, paymentMethod.Method, paymentMethod.IsActive);
            return _mapper.Map<ReturnPaymentMethodDTO>(paymentMethod);
        }

        public async Task<bool> DeletePaymentMethodAsync(int id)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id);
            if (paymentMethod == null)
            {
                _logger.LogWarning("Payment method {Id} not found for deletion", id);
                return false;
            }

            // Check for existing transactions
            if (paymentMethod.PaymentTransactions?.Count > 0)
            {
                _logger.LogWarning("Cannot delete payment method {Id} with existing transactions. Consider deactivating instead.", id);
                throw new InvalidOperationException("Cannot delete payment method with existing transactions. Consider deactivating the payment method instead.");
            }

            _paymentMethodRepository.Delete(paymentMethod);
            await _paymentMethodRepository.SaveAsync();

            _logger.LogInformation("Deleted payment method {Id}: {Method}", id, paymentMethod.Method);
            return true;
        }

        public async Task<ReturnPaymentMethodDTO> TogglePaymentMethodStatusAsync(int id)
        {
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(id);
            if (paymentMethod == null)
            {
                throw new KeyNotFoundException($"Payment method with ID {id} not found");
            }

            paymentMethod.IsActive = !paymentMethod.IsActive;
            _paymentMethodRepository.Update(paymentMethod);
            await _paymentMethodRepository.SaveAsync();

            _logger.LogInformation("Toggled payment method {Id} status to {IsActive}", id, paymentMethod.IsActive);
            return _mapper.Map<ReturnPaymentMethodDTO>(paymentMethod);
        }
    }
}