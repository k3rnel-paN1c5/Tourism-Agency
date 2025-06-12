using Application.IServices.UseCases;
using Domain.Entities;
using Domain.IRepositories;
using Application.DTOs.TransactionMethod;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.UseCases
{
    public class TransactionMethodService : ITransactionMethodService
    {
        private readonly IRepository<TransactionMethod, int> _TransactionMethodRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<TransactionMethodService> _logger;

        public TransactionMethodService(
            IRepository<TransactionMethod, int> TransactionMethodRepository,
            IMapper mapper,
            ILogger<TransactionMethodService> logger)
        {
            _TransactionMethodRepository = TransactionMethodRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReturnTransactionMethodDTO> CreateTransactionMethodAsync(CreateTransactionMethodDTO TransactionMethodDto)
        {
            // Validate input
            if (string.IsNullOrWhiteSpace(TransactionMethodDto.Method))
            {
                throw new ArgumentException("Transaction Method name is required", nameof(TransactionMethodDto.Method));
            }

            // Check for duplicate method names
            var existingMethods = await _TransactionMethodRepository.GetAllByPredicateAsync(
                pm => pm.Method!.ToLower() == TransactionMethodDto.Method.Trim().ToLower());
            
            if (existingMethods.Any())
            {
                throw new InvalidOperationException($"Transaction Method '{TransactionMethodDto.Method}' already exists");
            }

            var TransactionMethod = _mapper.Map<TransactionMethod>(TransactionMethodDto);

            await _TransactionMethodRepository.AddAsync(TransactionMethod);
            await _TransactionMethodRepository.SaveAsync();

            _logger.LogInformation("Created Transaction Method: {Method} (Active: {IsActive})", 
                TransactionMethod.Method, TransactionMethod.IsActive);
            return _mapper.Map<ReturnTransactionMethodDTO>(TransactionMethod);
        }

        public async Task<ReturnTransactionMethodDTO> GetTransactionMethodByIdAsync(int id)
        {
            var TransactionMethod = await _TransactionMethodRepository.GetByIdAsync(id);
            if (TransactionMethod == null)
            {
                _logger.LogWarning("Transaction Method {Id} not found", id);
                throw new KeyNotFoundException($"Transaction Method with ID {id} not found");
            }

            return _mapper.Map<ReturnTransactionMethodDTO>(TransactionMethod);
        }

        public async Task<IEnumerable<ReturnTransactionMethodDTO>> GetAllTransactionMethodsAsync()
        {
            var TransactionMethods = await _TransactionMethodRepository.GetAllAsync();
            _logger.LogDebug("Retrieved {Count} Transaction Methods", TransactionMethods.Count());
            return _mapper.Map<IEnumerable<ReturnTransactionMethodDTO>>(TransactionMethods);
        }

        public async Task<IEnumerable<ReturnTransactionMethodDTO>> GetActiveTransactionMethodsAsync()
        {
            var activeMethods = await _TransactionMethodRepository.GetAllByPredicateAsync(pm => pm.IsActive);
            _logger.LogDebug("Retrieved {Count} active Transaction Methods", activeMethods.Count());
            return _mapper.Map<IEnumerable<ReturnTransactionMethodDTO>>(activeMethods);
        }

        public async Task<ReturnTransactionMethodDTO> UpdateTransactionMethodAsync(UpdateTransactionMethodDTO TransactionMethodDto)
        {
            var TransactionMethod = await _TransactionMethodRepository.GetByIdAsync(TransactionMethodDto.Id);
            if (TransactionMethod == null)
            {
                throw new KeyNotFoundException($"Transaction Method with ID {TransactionMethodDto.Id} not found");
            }

            // Check for duplicate method names (excluding current record)
            if (!string.IsNullOrWhiteSpace(TransactionMethodDto.Method))
            {
                var existingMethods = await _TransactionMethodRepository.GetAllByPredicateAsync(
                    pm => pm.Id != TransactionMethodDto.Id && 
                          pm.Method!.ToLower() == TransactionMethodDto.Method.Trim().ToLower());
                
                if (existingMethods.Any())
                {
                    throw new InvalidOperationException($"Transaction Method '{TransactionMethodDto.Method}' already exists");
                }
            }

            // Use AutoMapper to update the entity
            _mapper.Map(TransactionMethodDto, TransactionMethod);

            _TransactionMethodRepository.Update(TransactionMethod);
            await _TransactionMethodRepository.SaveAsync();

            _logger.LogInformation("Updated Transaction Method {Id}: {Method} (Active: {IsActive})", 
                TransactionMethod.Id, TransactionMethod.Method, TransactionMethod.IsActive);
            return _mapper.Map<ReturnTransactionMethodDTO>(TransactionMethod);
        }

        public async Task<bool> DeleteTransactionMethodAsync(int id)
        {
            var TransactionMethod = await _TransactionMethodRepository.GetByIdAsync(id);
            if (TransactionMethod == null)
            {
                _logger.LogWarning("Transaction Method {Id} not found for deletion", id);
                return false;
            }

            // Check for existing transactions
            if (TransactionMethod.PaymentTransactions?.Count > 0)
            {
                _logger.LogWarning("Cannot delete Transaction Method {Id} with existing transactions. Consider deactivating instead.", id);
                throw new InvalidOperationException("Cannot delete Transaction Method with existing transactions. Consider deactivating the Transaction Method instead.");
            }

            _TransactionMethodRepository.Delete(TransactionMethod);
            await _TransactionMethodRepository.SaveAsync();

            _logger.LogInformation("Deleted Transaction Method {Id}: {Method}", id, TransactionMethod.Method);
            return true;
        }

        public async Task<ReturnTransactionMethodDTO> ToggleTransactionMethodStatusAsync(int id)
        {
            var TransactionMethod = await _TransactionMethodRepository.GetByIdAsync(id);
            if (TransactionMethod == null)
            {
                throw new KeyNotFoundException($"Transaction Method with ID {id} not found");
            }

            TransactionMethod.IsActive = !TransactionMethod.IsActive;
            _TransactionMethodRepository.Update(TransactionMethod);
            await _TransactionMethodRepository.SaveAsync();

            _logger.LogInformation("Toggled Transaction Method {Id} status to {IsActive}", id, TransactionMethod.IsActive);
            return _mapper.Map<ReturnTransactionMethodDTO>(TransactionMethod);
        }
    }
}