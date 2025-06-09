using Application.DTOs.PaymentTransaction;
using Application.IServices.UseCases;
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
        private readonly IRepository<Payment, int> _paymentRepository;
        private readonly IRepository<PaymentMethod, int> _paymentMethodRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentTransactionService> _logger;

        public PaymentTransactionService(
            IRepository<PaymentTransaction, int> paymentTransactionRepository,
            IRepository<Payment, int> paymentRepository,
            IRepository<PaymentMethod, int> paymentMethodRepository,
            IMapper mapper,
            ILogger<PaymentTransactionService> logger)
        {
            _paymentTransactionRepository = paymentTransactionRepository;
            _paymentRepository = paymentRepository;
            _paymentMethodRepository = paymentMethodRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<ReturnPaymentTransactionDTO> CreatePaymentTransactionAsync(CreatePaymentTransactionDTO transactionDto)
        {
            // Validate payment exists
            var payment = await _paymentRepository.GetByIdAsync(transactionDto.PaymentId);
            if (payment == null)
            {
                _logger.LogWarning("Payment {PaymentId} not found", transactionDto.PaymentId);
                throw new KeyNotFoundException($"Payment with ID {transactionDto.PaymentId} not found");
            }

            // Validate payment method exists and is active
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(transactionDto.PaymentMethodId);
            if (paymentMethod == null)
            {
                _logger.LogWarning("Payment method {PaymentMethodId} not found", transactionDto.PaymentMethodId);
                throw new KeyNotFoundException($"Payment method with ID {transactionDto.PaymentMethodId} not found");
            }

            if (!paymentMethod.IsActive)
            {
                throw new InvalidOperationException($"Payment method '{paymentMethod.Method}' is not active");
            }

            // NEW: Enhanced Amount and Type Validation
            await ValidateTransactionAmountAndTypeAsync(payment, transactionDto);

            var transaction = _mapper.Map<PaymentTransaction>(transactionDto);
            transaction.TransactionDate = DateTime.UtcNow;

            await _paymentTransactionRepository.AddAsync(transaction);
            await _paymentTransactionRepository.SaveAsync();


            _logger.LogInformation("Created payment transaction {TransactionId} for payment {PaymentId}", 
                transaction.Id, transactionDto.PaymentId);

            var result = _mapper.Map<ReturnPaymentTransactionDTO>(transaction);
            result.PaymentMethodName = paymentMethod.Method;
            return result;
        }

        private async Task ValidateTransactionAmountAndTypeAsync(Payment payment, CreatePaymentTransactionDTO transactionDto)
        {
            var existingTransactions = await _paymentTransactionRepository
                .GetAllByPredicateAsync(t => t.PaymentId == payment.Id);
            
            var currentPaidAmount = existingTransactions
                .Where(t => t.TransactionType != TransactionType.Refund)
                .Sum(t => t.Amount);
            
            var currentRefundAmount = existingTransactions
                .Where(t => t.TransactionType == TransactionType.Refund)
                .Sum(t => t.Amount);
            
            var netPaidAmount = currentPaidAmount - currentRefundAmount;
            
            switch (transactionDto.TransactionType)
            {
                case TransactionType.Deposit:
                    await ValidateDepositTransaction(payment, transactionDto, netPaidAmount);
                    break;
                    
                case TransactionType.Payment:
                    await ValidateFullPaymentTransaction(payment, transactionDto, netPaidAmount);
                    break;
                    
                case TransactionType.Final:
                    await ValidateFinalPaymentTransaction(payment, transactionDto, netPaidAmount);
                    break;
                    
                case TransactionType.Refund:
                    await ValidateRefundTransaction(payment, transactionDto, netPaidAmount);
                    break;
            }
        }

        private async Task ValidateDepositTransaction(Payment payment, CreatePaymentTransactionDTO transactionDto, decimal netPaidAmount)
        {
            // Deposit should not exceed 80% of total amount (business rule example)
            var maxDepositAmount = payment.AmountDue * 0.8m;
            
            if (transactionDto.Amount > maxDepositAmount)
            {
                throw new InvalidOperationException($"Deposit amount cannot exceed {maxDepositAmount:C}");
            }
            
            if (netPaidAmount > 0)
            {
                throw new InvalidOperationException("Cannot process deposit - payment already has transactions");
            }
            
            if (transactionDto.Amount >= payment.AmountDue)
            {
                throw new InvalidOperationException("Deposit amount should be less than total amount due. Use 'Payment' type for full payment");
            }
        }

        private async Task ValidateFullPaymentTransaction(Payment payment, CreatePaymentTransactionDTO transactionDto, decimal netPaidAmount)
        {
            if (netPaidAmount > 0)
            {
                throw new InvalidOperationException("Cannot process full payment - payment already has transactions. Use 'Final' type for remaining balance");
            }
            
            if (transactionDto.Amount != payment.AmountDue)
            {
                throw new InvalidOperationException($"Full payment amount must equal total amount due ({payment.AmountDue:C})");
            }
        }

        private async Task ValidateFinalPaymentTransaction(Payment payment, CreatePaymentTransactionDTO transactionDto, decimal netPaidAmount)
        {
            if (netPaidAmount <= 0)
            {
                throw new InvalidOperationException("Final payment requires an existing deposit or partial payment");
            }
            
            var remainingBalance = payment.AmountDue - netPaidAmount;
            
            if (remainingBalance <= 0)
            {
                throw new InvalidOperationException("No remaining balance to pay");
            }
            
            if (transactionDto.Amount != remainingBalance)
            {
                throw new InvalidOperationException($"Final payment amount must equal remaining balance ({remainingBalance:C})");
            }
        }

        private async Task ValidateRefundTransaction(Payment payment, CreatePaymentTransactionDTO transactionDto, decimal netPaidAmount)
        {
            if (netPaidAmount <= 0)
            {
                throw new InvalidOperationException("Cannot refund - no payments have been made");
            }
            
            if (transactionDto.Amount > netPaidAmount)
            {
                throw new InvalidOperationException($"Refund amount cannot exceed paid amount ({netPaidAmount:C})");
            }
            
            if (payment.Status == PaymentStatus.Pending)
            {
                throw new InvalidOperationException("Cannot refund pending payments");
            }
        }

        public async Task<IEnumerable<ReturnPaymentTransactionDTO>> GetAllPaymentTransactionsAsync()
        {
            var transactions = await _paymentTransactionRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<ReturnPaymentTransactionDTO>>(transactions);
            
            // Populate payment method names
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
            
            // Populate payment method name
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(transaction.PaymentMethodId);
            result.PaymentMethodName = paymentMethod?.Method;

            return result;
        }

        public async Task<IEnumerable<ReturnPaymentTransactionDTO>> GetTransactionsByPaymentIdAsync(int paymentId)
        {
            var transactions = await _paymentTransactionRepository.GetAllByPredicateAsync(t => t.PaymentId == paymentId);
            var result = _mapper.Map<IEnumerable<ReturnPaymentTransactionDTO>>(transactions);
            
            // Populate payment method names
            await PopulatePaymentMethodNamesAsync(result);
            
            return result;
        }

        public async Task<IEnumerable<ReturnPaymentTransactionDTO>> GetTransactionsByTypeAsync(TransactionType transactionType)
        {
            var transactions = await _paymentTransactionRepository.GetAllByPredicateAsync(t => t.TransactionType == transactionType);
            var result = _mapper.Map<IEnumerable<ReturnPaymentTransactionDTO>>(transactions);
            
            // Populate payment method names
            await PopulatePaymentMethodNamesAsync(result);
            
            return result;
        }

        public async Task<IEnumerable<ReturnPaymentTransactionDTO>> GetTransactionsByPaymentMethodAsync(int paymentMethodId)
        {
            var transactions = await _paymentTransactionRepository.GetAllByPredicateAsync(t => t.PaymentMethodId == paymentMethodId);
            var result = _mapper.Map<IEnumerable<ReturnPaymentTransactionDTO>>(transactions);
            
            // Populate payment method names
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
            
            // Populate payment method names
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

            // Only allow updating reference and notes, not core transaction data
            transaction.TransactionReference = transactionDto.TransactionReference;
            transaction.Notes = transactionDto.Notes;

            _paymentTransactionRepository.Update(transaction);
            await _paymentTransactionRepository.SaveAsync();

            _logger.LogInformation("Updated payment transaction {TransactionId}", transactionDto.Id);

            var result = _mapper.Map<ReturnPaymentTransactionDTO>(transaction);
            
            // Populate payment method name
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

            // Get payment method details
            var paymentMethod = await _paymentMethodRepository.GetByIdAsync(transaction.PaymentMethodId);
            if (paymentMethod != null)
            {
                result.PaymentMethodName = paymentMethod.Method;
                result.PaymentMethodIcon = paymentMethod.Icon;
            }

            // Get payment details
            var payment = await _paymentRepository.GetByIdAsync(transaction.PaymentId);
            if (payment != null)
            {
                result.BookingId = payment.BookingId;
                result.PaymentStatus = payment.Status;
            }

            return result;
        }

        public async Task<decimal> GetTotalTransactionAmountByPaymentAsync(int paymentId)
        {
            var transactions = await _paymentTransactionRepository.GetAllByPredicateAsync(t => t.PaymentId == paymentId);
            return transactions.Where(t => t.TransactionType == TransactionType.Payment).Sum(t => t.Amount) -
                   transactions.Where(t => t.TransactionType == TransactionType.Refund).Sum(t => t.Amount);
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