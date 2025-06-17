using Domain.Entities;
using Domain.Enums;
using Domain.IRepositories;
using Application.IServices.UseCases;
using Application.DTOs.Payment;
using Application.DTOs.PaymentTransaction;
using Application.IServices.Validation;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Services.UseCases
{
    public class PaymentService : IPaymentService
    {
        private readonly IRepository<Payment, int> _paymentRepository;
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly IPaymentValidationService _validationService;
        private readonly IMapper _mapper;
        private readonly ILogger<PaymentService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(
            IRepository<Payment, int> paymentRepository,
            IPaymentTransactionService paymentTransactionService,
            IPaymentValidationService validationService,
            IMapper mapper,
            ILogger<PaymentService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _paymentRepository = paymentRepository ?? throw new ArgumentNullException(nameof(paymentRepository));
            _paymentTransactionService = paymentTransactionService ?? throw new ArgumentNullException(nameof(paymentTransactionService));
            _validationService = validationService ?? throw new ArgumentNullException(nameof(validationService));
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<ReturnPaymentDTO> CreatePaymentAsync(CreatePaymentDTO paymentDto)
        {
            if (paymentDto is null)
            {
                _logger.LogError("CreatePaymentAsync: Input DTO is null.");
                throw new ArgumentNullException(nameof(paymentDto), "Payment creation DTO cannot be null.");
            }

            _logger.LogInformation("Attempting to create payment for BookingId: {BookingId}", paymentDto.BookingId);
            try
            {
                var payment = _mapper.Map<Payment>(paymentDto);
                payment.AmountPaid = 0;
                payment.Status = PaymentStatus.Pending;
                payment.PaymentDate = DateTime.UtcNow;

                await _paymentRepository.AddAsync(payment);
                await _paymentRepository.SaveAsync();

                _logger.LogInformation("Payment '{Id}' created successfully for BookingId: {BookingId}.", payment.Id, paymentDto.BookingId);
                return _mapper.Map<ReturnPaymentDTO>(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating payment for BookingId: {BookingId}.", paymentDto.BookingId);
                throw;
            }
        }

        public async Task<PaymentProcessResultDTO> ProcessPaymentAsync(ProcessPaymentDTO processPaymentDto)
        {
            if (processPaymentDto is null)
            {
                _logger.LogError("ProcessPaymentAsync: Input DTO is null.");
                throw new ArgumentNullException(nameof(processPaymentDto), "Process payment DTO cannot be null.");
            }

            _logger.LogInformation("Attempting to process payment with ID: {PaymentId}", processPaymentDto.PaymentId);
            try
            {
                // Validate payment exists
                var payment = await _validationService.ValidatePaymentExistsAsync(processPaymentDto.PaymentId);

                // Validate Transaction Method exists and is active
                var TransactionMethod = await _validationService.ValidateTransactionMethodExistsAndActiveAsync(processPaymentDto.TransactionMethodId);

                // Validate payment can receive payment
                _validationService.ValidatePaymentCanReceivePayment(payment, processPaymentDto.Amount);

                // Create transaction through PaymentTransactionService
                var createTransactionDto = new CreatePaymentTransactionDTO
                {
                    PaymentId = processPaymentDto.PaymentId,
                    TransactionMethodId = processPaymentDto.TransactionMethodId,
                    Amount = processPaymentDto.Amount,
                    TransactionType = TransactionType.Payment,
                    Notes = processPaymentDto.Notes
                };

                var transaction = await _paymentTransactionService.CreatePaymentTransactionAsync(createTransactionDto);
                
                // Update payment notes with the process information
                payment.Notes = string.IsNullOrEmpty(payment.Notes) 
                    ? $"PAYMENT: {processPaymentDto.Amount:C} - {processPaymentDto.Notes} (Transaction: {transaction.Id})"
                    : $"{payment.Notes}; PAYMENT: {processPaymentDto.Amount:C} - {processPaymentDto.Notes} (Transaction: {transaction.Id})";
                
                // Update payment status based on total paid amount
                await UpdatePaymentStatusAfterTransaction(payment.Id);

                var updatedPayment = await _paymentRepository.GetByIdAsync(payment.Id);
                
                _logger.LogInformation("Payment '{PaymentId}' processed successfully with transaction '{TransactionId}' using {TransactionMethod}", 
                    payment.Id, transaction.Id, TransactionMethod.Method);

                return new PaymentProcessResultDTO
                {
                    Payment = _mapper.Map<ReturnPaymentDTO>(updatedPayment),
                    Transaction = transaction,
                    Success = true,
                    Message = $"Payment processed successfully via {TransactionMethod.Method}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing payment with ID {PaymentId}.", processPaymentDto.PaymentId);
                throw;
            }
        }

        public async Task<PaymentProcessResultDTO> ProcessRefundAsync(ProcessRefundDTO refundDto)
        {
            if (refundDto is null)
            {
                _logger.LogError("ProcessRefundAsync: Input DTO is null.");
                throw new ArgumentNullException(nameof(refundDto), "Process refund DTO cannot be null.");
            }

            _logger.LogInformation("Attempting to process refund for payment with ID: {PaymentId}", refundDto.PaymentId);
            try
            {
                // Validate payment exists
                var payment = await _validationService.ValidatePaymentExistsAsync(refundDto.PaymentId);

                // Validate Transaction Method exists and is active
                var TransactionMethod = await _validationService.ValidateTransactionMethodExistsAndActiveAsync(refundDto.TransactionMethodId);

                // Validate payment can be refunded
                _validationService.ValidatePaymentCanBeRefunded(payment, refundDto.Amount, refundDto.Reason);

                // Create refund transaction
                var createTransactionDto = new CreatePaymentTransactionDTO
                {
                    PaymentId = refundDto.PaymentId,
                    TransactionMethodId = refundDto.TransactionMethodId,
                    Amount = refundDto.Amount,
                    TransactionType = TransactionType.Refund,
                    Notes = $"REFUND: {refundDto.Reason}"
                };

                var transaction = await _paymentTransactionService.CreatePaymentTransactionAsync(createTransactionDto);
                
                // Update payment notes with refund information
                payment.Notes = string.IsNullOrEmpty(payment.Notes) 
                    ? $"REFUND: {refundDto.Amount:C} - {refundDto.Reason} (Transaction: {transaction.Id})"
                    : $"{payment.Notes}; REFUND: {refundDto.Amount:C} - {refundDto.Reason} (Transaction: {transaction.Id})";
                
                // Update payment status after refund
                await UpdatePaymentStatusAfterTransaction(payment.Id);

                var updatedPayment = await _paymentRepository.GetByIdAsync(payment.Id);

                _logger.LogInformation("Refund of '{Amount}' processed successfully for payment '{PaymentId}' via {TransactionMethod}", 
                    refundDto.Amount, refundDto.PaymentId, TransactionMethod.Method);

                return new PaymentProcessResultDTO
                {
                    Payment = _mapper.Map<ReturnPaymentDTO>(updatedPayment),
                    Transaction = transaction,
                    Success = true,
                    Message = $"Refund of {refundDto.Amount:C} processed successfully via {TransactionMethod.Method}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing refund for payment with ID {PaymentId}.", refundDto.PaymentId);
                throw;
            }
        }

        private async Task UpdatePaymentStatusAfterTransaction(int paymentId)
        {
            var payment = await _paymentRepository.GetByIdAsync(paymentId);
            var totalPaid = await _paymentTransactionService.GetTotalTransactionAmountByPaymentAsync(paymentId);

            payment!.AmountPaid = totalPaid;

            if (totalPaid <= 0)
            {
                payment.Status = totalPaid == 0 ? PaymentStatus.Pending : PaymentStatus.Refunded;
            }
            else if (totalPaid >= payment.AmountDue)
            {
                payment.Status = PaymentStatus.Paid;
                if (payment.PaymentDate == null)
                {
                    payment.PaymentDate = DateTime.UtcNow;
                }
            }
            else
            {
                payment.Status = PaymentStatus.PartiallyPaid;
            }

            _paymentRepository.Update(payment);
            await _paymentRepository.SaveAsync();
        }

        public async Task<IEnumerable<ReturnPaymentDTO>> GetAllPaymentsAsync()
        {
            _logger.LogInformation("Attempting to retrieve all payments.");
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogError("HTTP context is unavailable during GetAllPaymentsAsync.");
                    throw new InvalidOperationException("HTTP context is unavailable.");
                }

                var userIdClaim = httpContext.User.Claims
                   .FirstOrDefault(c => c.Type == "UserId" ||
                                        c.Type == "sub" ||
                                        c.Type == ClaimTypes.NameIdentifier)?.Value;
                var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

                IEnumerable<Payment> payments;

                if (role == "Customer")
                {
                    if (string.IsNullOrEmpty(userIdClaim))
                    {
                        _logger.LogWarning("Customer role detected but UserId claim is missing.");
                        throw new UnauthorizedAccessException("Customer ID not found for retrieving payments.");
                    }
                    _logger.LogDebug("Retrieving payments for customer ID: {CustomerId}", userIdClaim);
                    payments = await _paymentRepository.GetAllByPredicateAsync(p => 
                        p.Booking != null && p.Booking.CustomerId == userIdClaim).ConfigureAwait(false);
                }
                else
                {
                    _logger.LogDebug("Retrieving all payments for employees.");
                    payments = await _paymentRepository.GetAllAsync().ConfigureAwait(false);
                }

                _logger.LogDebug("{Count} payments retrieved.", payments?.Count() ?? 0);
                return _mapper.Map<IEnumerable<ReturnPaymentDTO>>(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all payments.");
                throw;
            }
        }

        public async Task<ReturnPaymentDTO> GetPaymentByIdAsync(int paymentId)
        {
            _logger.LogInformation("Attempting to retrieve payment with ID: {Id}", paymentId);
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogError("HTTP context is unavailable during GetPaymentByIdAsync.");
                    throw new InvalidOperationException("HTTP context is unavailable.");
                }

                var userIdClaim = httpContext.User.Claims
                   .FirstOrDefault(c => c.Type == "UserId" ||
                                        c.Type == "sub" ||
                                        c.Type == ClaimTypes.NameIdentifier)?.Value;
                var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

                var payment = await _validationService.ValidatePaymentExistsAsync(paymentId);

                // If user is a customer, verify they own this payment
                if (role == "Customer")
                {
                    if (string.IsNullOrEmpty(userIdClaim))
                    {
                        _logger.LogWarning("Customer role detected but UserId claim is missing.");
                        throw new UnauthorizedAccessException("Customer ID not found for accessing payment.");
                    }
                    if (payment.Booking?.CustomerId != userIdClaim)
                    {
                        _logger.LogWarning("Customer {UserId} attempted to access payment {PaymentId} they don't own", 
                            userIdClaim, paymentId);
                        throw new UnauthorizedAccessException("You are not authorized to access this payment.");
                    }
                }

                _logger.LogInformation("Payment '{Id}' retrieved successfully.", paymentId);
                return _mapper.Map<ReturnPaymentDTO>(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving payment with ID {Id}.", paymentId);
                throw;
            }
        }

        public async Task<ReturnPaymentDTO> GetPaymentByBookingIdAsync(int bookingId)
        {
            _logger.LogInformation("Attempting to retrieve payment for booking with ID: {BookingId}", bookingId);
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogError("HTTP context is unavailable during GetPaymentByBookingIdAsync.");
                    throw new InvalidOperationException("HTTP context is unavailable.");
                }

                var userIdClaim = httpContext.User.Claims
                   .FirstOrDefault(c => c.Type == "UserId" ||
                                        c.Type == "sub" ||
                                        c.Type == ClaimTypes.NameIdentifier)?.Value;
                var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

                var payment = (await _paymentRepository.GetAllByPredicateAsync(p => p.BookingId == bookingId))
                    .FirstOrDefault();

                if (payment == null)
                {
                    _logger.LogWarning("Payment for booking {BookingId} was not found.", bookingId);
                    throw new KeyNotFoundException($"Payment for booking ID {bookingId} was not found.");
                }

                // If user is a customer, verify they own this payment
                if (role == "Customer")
                {
                    if (string.IsNullOrEmpty(userIdClaim))
                    {
                        _logger.LogWarning("Customer role detected but UserId claim is missing.");
                        throw new UnauthorizedAccessException("Customer ID not found for accessing payment.");
                    }
                    if (payment.Booking?.CustomerId != userIdClaim)
                    {
                        _logger.LogWarning("Customer {UserId} attempted to access payment for booking {BookingId} they don't own", 
                            userIdClaim, bookingId);
                        throw new UnauthorizedAccessException("You are not authorized to access this payment.");
                    }
                }

                _logger.LogInformation("Payment for booking '{BookingId}' retrieved successfully.", bookingId);
                return _mapper.Map<ReturnPaymentDTO>(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving payment for booking with ID {BookingId}.", bookingId);
                throw;
            }
        }

        public async Task<IEnumerable<ReturnPaymentDTO>> GetPaymentsByStatusAsync(PaymentStatus status)
        {
            _logger.LogInformation("Attempting to retrieve payments with status: {Status}", status);
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogError("HTTP context is unavailable during GetPaymentsByStatusAsync.");
                    throw new InvalidOperationException("HTTP context is unavailable.");
                }

                var userIdClaim = httpContext.User.Claims
                   .FirstOrDefault(c => c.Type == "UserId" ||
                                        c.Type == "sub" ||
                                        c.Type == ClaimTypes.NameIdentifier)?.Value;
                var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

                IEnumerable<Payment> payments;

                if (role == "Customer")
                {
                    if (string.IsNullOrEmpty(userIdClaim))
                    {
                        _logger.LogWarning("Customer role detected but UserId claim is missing.");
                        throw new UnauthorizedAccessException("Customer ID not found for retrieving payments.");
                    }
                    _logger.LogDebug("Retrieving payments with status {Status} for customer ID: {CustomerId}", status, userIdClaim);
                    payments = await _paymentRepository.GetAllByPredicateAsync(p => 
                        p.Status == status && 
                        p.Booking != null && 
                        p.Booking.CustomerId == userIdClaim).ConfigureAwait(false);
                }
                else
                {
                    _logger.LogDebug("Retrieving all payments with status {Status} for employees.", status);
                    payments = await _paymentRepository.GetAllByPredicateAsync(p => p.Status == status).ConfigureAwait(false);
                }

                _logger.LogDebug("{Count} payments with status {Status} retrieved.", payments?.Count() ?? 0, status);
                return _mapper.Map<IEnumerable<ReturnPaymentDTO>>(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving payments with status {Status}.", status);
                throw;
            }
        }

        public async Task<PaymentDetailsDTO> GetPaymentDetailsAsync(int paymentId)
        {
            _logger.LogInformation("Attempting to retrieve payment details for ID: {PaymentId}", paymentId);
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogError("HTTP context is unavailable during GetPaymentDetailsAsync.");
                    throw new InvalidOperationException("HTTP context is unavailable.");
                }

                var userIdClaim = httpContext.User.Claims
                   .FirstOrDefault(c => c.Type == "UserId" ||
                                        c.Type == "sub" ||
                                        c.Type == ClaimTypes.NameIdentifier)?.Value;
                var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

                var payment = await _validationService.ValidatePaymentExistsAsync(paymentId);

                // If user is a customer, verify they own this payment
                if (role == "Customer")
                {
                    if (string.IsNullOrEmpty(userIdClaim))
                    {
                        _logger.LogWarning("Customer role detected but UserId claim is missing.");
                        throw new UnauthorizedAccessException("Customer ID not found for accessing payment details.");
                    }
                    if (payment.Booking?.CustomerId != userIdClaim)
                    {
                        _logger.LogWarning("Customer {UserId} attempted to access payment details {PaymentId} they don't own", 
                            userIdClaim, paymentId);
                        throw new UnauthorizedAccessException("You are not authorized to access this payment.");
                    }
                }

                var paymentDetails = _mapper.Map<PaymentDetailsDTO>(payment);
                
                // Get all transactions for this payment
                var transactions = await _paymentTransactionService.GetTransactionsByPaymentIdAsync(paymentId);
                paymentDetails.Transactions = transactions.ToList();

                _logger.LogInformation("Payment details for '{PaymentId}' retrieved successfully.", paymentId);
                return paymentDetails;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving payment details for ID {PaymentId}.", paymentId);
                throw;
            }
        }

        public async Task<ReturnPaymentDTO> CancelPaymentAsync(int paymentId)
        {
            _logger.LogInformation("Attempting to cancel payment with ID: {PaymentId}", paymentId);
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogError("HTTP context is unavailable during CancelPaymentAsync.");
                    throw new InvalidOperationException("HTTP context is unavailable.");
                }

                var userIdClaim = httpContext.User.Claims
                   .FirstOrDefault(c => c.Type == "UserId" ||
                                        c.Type == "sub" ||
                                        c.Type == ClaimTypes.NameIdentifier)?.Value;
                var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

                var payment = await _validationService.ValidatePaymentExistsAsync(paymentId);

                // If user is a customer, verify they own this payment
                if (role == "Customer")
                {
                    if (string.IsNullOrEmpty(userIdClaim))
                    {
                        _logger.LogWarning("Customer role detected but UserId claim is missing.");
                        throw new UnauthorizedAccessException("Customer ID not found for cancelling payment.");
                    }
                    if (payment.Booking?.CustomerId != userIdClaim)
                    {
                        _logger.LogWarning("Customer {UserId} attempted to cancel payment {PaymentId} they don't own", 
                            userIdClaim, paymentId);
                        throw new UnauthorizedAccessException("You are not authorized to cancel this payment.");
                    }
                }

                // Validate that payment can be cancelled
                if (payment.Status == PaymentStatus.Paid)
                {
                    _logger.LogWarning("Attempted to cancel a paid payment {PaymentId}", paymentId);
                    throw new InvalidOperationException("Cannot cancel a payment that has been paid. Use refund instead.");
                }

                if (payment.Status == PaymentStatus.Cancelled)
                {
                    _logger.LogWarning("Attempted to cancel an already cancelled payment {PaymentId}", paymentId);
                    throw new InvalidOperationException("Payment is already cancelled.");
                }

                if (payment.Status == PaymentStatus.Refunded)
                {
                    _logger.LogWarning("Attempted to cancel a refunded payment {PaymentId}", paymentId);
                    throw new InvalidOperationException("Cannot cancel a refunded payment.");
                }

                // Update payment status to cancelled
                payment.Status = PaymentStatus.Cancelled;
                _paymentRepository.Update(payment);
                await _paymentRepository.SaveAsync();

                _logger.LogInformation("Payment '{PaymentId}' cancelled successfully.", paymentId);
                return _mapper.Map<ReturnPaymentDTO>(payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while cancelling payment with ID {PaymentId}.", paymentId);
                throw;
            }
        }

        public async Task<bool> DeletePaymentAsync(int paymentId)
        {
            _logger.LogInformation("Attempting to delete payment with ID: {PaymentId}", paymentId);
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null)
                {
                    _logger.LogError("HTTP context is unavailable during DeletePaymentAsync.");
                    throw new InvalidOperationException("HTTP context is unavailable.");
                }

                var role = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;

                // Only allow admins to delete payments
                if (role != "Admin")
                {
                    _logger.LogWarning("Non-admin user attempted to delete payment {PaymentId}", paymentId);
                    throw new UnauthorizedAccessException("Only administrators can delete payments.");
                }

                var payment = await _validationService.ValidatePaymentExistsAsync(paymentId);

                // Validate that payment can be deleted
                if (payment.Status == PaymentStatus.Paid)
                {
                    _logger.LogWarning("Attempted to delete a paid payment {PaymentId}", paymentId);
                    throw new InvalidOperationException("Cannot delete a payment that has been paid. Cancel or refund the payment first.");
                }

                if (payment.Status == PaymentStatus.PartiallyPaid)
                {
                    _logger.LogWarning("Attempted to delete a partially paid payment {PaymentId}", paymentId);
                    throw new InvalidOperationException("Cannot delete a payment that has been partially paid. Refund the payment first.");
                }

                // Check if there are any transactions associated with this payment
                var transactions = await _paymentTransactionService.GetTransactionsByPaymentIdAsync(paymentId);
                if (transactions.Any())
                {
                    _logger.LogWarning("Attempted to delete payment {PaymentId} with existing transactions", paymentId);
                    throw new InvalidOperationException("Cannot delete a payment with existing transactions. Delete transactions first or cancel the payment instead.");
                }

                await _paymentRepository.DeleteByIdAsync(paymentId);
                await _paymentRepository.SaveAsync();

                _logger.LogInformation("Payment '{PaymentId}' deleted successfully.", paymentId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting payment with ID {PaymentId}.", paymentId);
                throw;
            }
        }
    }
}