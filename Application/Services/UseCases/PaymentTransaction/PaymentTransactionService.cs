using Application.DTOs.PaymentTransaction;
using Application.IServices.UseCases;
using Application.IServices.Validation;
using Application.Services.Validation;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Microsoft.Extensions.Logging;

namespace Application.Services.UseCases
{
    public class PaymentTransactionService : IPaymentTransactionService
     {
        private readonly IRepository<PaymentTransaction, int> _paymentTransactionRepository;
        private readonly IRepository<PaymentMethod, int> _paymentMethodRepository;
        private readonly IPaymentValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentTransactionService> _logger;

        public PaymentTransactionService(
            IRepository<PaymentTransaction, int> paymentTransactionRepository,
            IRepository<PaymentMethod, int> paymentMethodRepository,
            IPaymentValidationService validationService,
            IMapper mapper,
            ILogger<PaymentTransactionService> logger)
        {
            _paymentTransactionRepository = paymentTransactionRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _validationService = validationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReturnPaymentTransactionDTO> CreatePaymentTransactionAsync(CreatePaymentTransactionDTO transactionDto)
        {
            // Apply transaction-specific validations
            if (transactionDto.TransactionType == TransactionType.Payment)
            {
                _validationService.ValidatePaymentCanReceivePayment(payment, transactionDto.Amount);
            }
            else if (transactionDto.TransactionType == TransactionType.Refund)
            {
                _validationService.ValidatePaymentCanBeRefunded(payment, transactionDto.Amount);
            }

            // Create the transaction
            var transaction = _mapper.Map<PaymentTransaction>(transactionDto);
            transaction.TransactionDate = DateTime.UtcNow;

            await _paymentTransactionRepository.AddAsync(transaction);
            await _paymentTransactionRepository.SaveAsync();

            _logger.LogInformation("Created {TransactionType} transaction {TransactionId} for payment {PaymentId}", 
                transactionDto.TransactionType, transaction.Id, transactionDto.PaymentId);

            var result = _mapper.Map<ReturnPaymentTransactionDTO>(transaction);
            result.PaymentMethodName = paymentMethod.Method;
            return result;
        }

        public async Task<IEnumerable<ReturnPaymentTransactionDTO>> GetAllPaymentTransactionsAsync()
        {
            var transactions = await _paymentTransactionRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<ReturnPaymentTransactionDTO>>(transactions);
            
            await PopulatePaymentMethodNamesAsync(result);
            
            return result;
        }

        public async Task<ReturnPaymentTransactionDTO> GetPaymentTransactionByIdAsync(int transactionId)
        {
            var transaction = await _paymentTransactionRepository.GetByIdAsync(transactionId);
            if (transaction == null)
            {
                _logger.LogWarning("Payment transaction {TransactionId} not found", transactionId);
                throw new KeyNotFoundException($"Payment transaction with ID {transactionId} not found");
            }

            var result = _mapper.Map<ReturnPaymentTransactionDTO>(transaction);
            
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(transaction.PaymentMethodId);
            result.PaymentMethodName = paymentMethod?.Method;

            return result;
        }

        public async Task<IEnumerable<ReturnPaymentTransactionDTO>> GetTransactionsByPaymentIdAsync(int paymentId)
        {
            var transactions = await _paymentTransactionRepository.GetAllByPredicateAsync(t => t.PaymentId == paymentId);
            var result = _mapper.Map<IEnumerable<ReturnPaymentTransactionDTO>>(transactions);
            
            await PopulatePaymentMethodNamesAsync(result);
            
            return result;
        }

        public async Task<IEnumerable<ReturnPaymentTransactionDTO>> GetTransactionsByTypeAsync(TransactionType transactionType)
        {
            var transactions = await _paymentTransactionRepository.GetAllByPredicateAsync(t => t.TransactionType == transactionType);
            var result = _mapper.Map<IEnumerable<ReturnPaymentTransactionDTO>>(transactions);
            
            await PopulatePaymentMethodNamesAsync(result);
            
            return result;
        }

        public async Task<IEnumerable<ReturnPaymentTransactionDTO>> GetTransactionsByPaymentMethodAsync(int paymentMethodId)
        {
            var transactions = await _paymentTransactionRepository.GetAllByPredicateAsync(t => t.PaymentMethodId == paymentMethodId);
            var result = _mapper.Map<IEnumerable<ReturnPaymentTransactionDTO>>(transactions);
            
            await PopulatePaymentMethodNamesAsync(result);
            
            return result;
        }

        public async Task<IEnumerable<ReturnPaymentTransactionDTO>> GetTransactionsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            if (startDate >= endDate)
            {
                throw new ArgumentException("Start date must be before end date");
            }

            var transactions = await _paymentTransactionRepository.GetAllByPredicateAsync(
                t => t.TransactionDate >= startDate && t.TransactionDate <= endDate);
            
            var result = _mapper.Map<IEnumerable<ReturnPaymentTransactionDTO>>(transactions);
            
            await PopulatePaymentMethodNamesAsync(result);
            
            return result;
        }

        public async Task<ReturnPaymentTransactionDTO> UpdatePaymentTransactionAsync(UpdatePaymentTransactionDTO transactionDto)
        {
            var transaction = await _paymentTransactionRepository.GetByIdAsync(transactionDto.Id);
            if (transaction == null)
            {
                throw new KeyNotFoundException($"Payment transaction with ID {transactionDto.Id} not found");
            }

            // Only allow updating notes, not core transaction data
            //transaction.Notes = transactionDto.Notes;

            _paymentTransactionRepository.Update(transaction);
            await _paymentTransactionRepository.SaveAsync();

            _logger.LogInformation("Updated payment transaction {TransactionId}", transactionDto.Id);

            var result = _mapper.Map<ReturnPaymentTransactionDTO>(transaction);
            
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(transaction.PaymentMethodId);
            result.PaymentMethodName = paymentMethod?.Method;

            return result;
        }

        public async Task<PaymentTransactionDetailsDTO> GetPaymentTransactionDetailsAsync(int transactionId)
        {
            var transaction = await _paymentTransactionRepository.GetByIdAsync(transactionId);
            if (transaction == null)
            {
                throw new KeyNotFoundException($"Payment transaction with ID {transactionId} not found");
            }

            var result = _mapper.Map<PaymentTransactionDetailsDTO>(transaction);

            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(transaction.PaymentMethodId);
            if (paymentMethod != null)
            {
                result.PaymentMethodName = paymentMethod.Method;
                result.PaymentMethodIcon = paymentMethod.Icon;
            }

            var payment = await _validationService.ValidatePaymentExistsAsync(transaction.PaymentId);
            result.BookingId = payment.BookingId;
            result.PaymentStatus = payment.Status;

            return result;
        }

        public async Task<decimal> GetTotalTransactionAmountByPaymentAsync(int paymentId)
        {
            var transactions = await _paymentTransactionRepository.GetAllByPredicateAsync(t => t.PaymentId == paymentId);
            
            var paymentAmount = transactions
                .Where(t => t.TransactionType == TransactionType.Payment)
                .Sum(t => t.Amount);
                
            var refundAmount = transactions
                .Where(t => t.TransactionType == TransactionType.Refund)
                .Sum(t => t.Amount);
                
            return paymentAmount - refundAmount;
        }

        private async Task PopulatePaymentMethodNamesAsync(IEnumerable<ReturnPaymentTransactionDTO> transactions)
        {
            var paymentMethodIds = transactions.Select(t => t.PaymentMethodId).Distinct();
            var paymentMethods = new Dictionary<int, string>();

            foreach (var id in paymentMethodIds)
            {
                var method = await _paymentMethodRepository.GetByIdAsync(id);
                if (method != null)
                {
                    paymentMethods[id] = method.Method ?? string.Empty;
                }
            }

            foreach (var transaction in transactions)
            {
                if (paymentMethods.TryGetValue(transaction.PaymentMethodId, out var methodName))
                {
                    transaction.PaymentMethodName = methodName;
                }
            }
        }
    }
}