using Microsoft.AspNetCore.Mvc;
using Application.IServices.UseCases;
using Application.DTOs.PaymentTransaction;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace TourismAgency.Controllers
{
    [ApiController]
    [Route("api/payment-transactions")]
    public class PaymentTransactionController : ControllerBase
    {
        private readonly IPaymentTransactionService _paymentTransactionService;
        private readonly ILogger<PaymentTransactionController> _logger;

        public PaymentTransactionController(
            IPaymentTransactionService paymentTransactionService,
            ILogger<PaymentTransactionController> logger)
        {
            _paymentTransactionService = paymentTransactionService;
            _logger = logger;
        }

        /// <summary>
        /// Get all payment transactions
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Admin,TripSupervisor")]
        public async Task<ActionResult<IEnumerable<ReturnPaymentTransactionDTO>>> GetAllTransactions()
        {
            try
            {
                var transactions = await _paymentTransactionService.GetAllPaymentTransactionsAsync();
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all payment transactions");
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payment transactions",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get payment transaction by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnPaymentTransactionDTO>> GetTransactionById(int id)
        {
            try
            {
                var transaction = await _paymentTransactionService.GetPaymentTransactionByIdAsync(id);
                return Ok(transaction);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment transaction with ID {id} not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment transaction {TransactionId}", id);
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payment transaction details",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get transactions by payment ID
        /// </summary>
        [HttpGet("payments/{paymentId}")]
        public async Task<ActionResult<IEnumerable<ReturnPaymentTransactionDTO>>> GetTransactionsByPayment(int paymentId)
        {
            try
            {
                var transactions = await _paymentTransactionService.GetTransactionsByPaymentIdAsync(paymentId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for payment {PaymentId}", paymentId);
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payment transactions",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get transactions by type
        /// </summary>
        [HttpGet("type/{transactionType}")]
        [Authorize(Roles = "Admin,TripSupervisor")]
        public async Task<ActionResult<IEnumerable<ReturnPaymentTransactionDTO>>> GetTransactionsByType(TransactionType transactionType)
        {
            try
            {
                var transactions = await _paymentTransactionService.GetTransactionsByTypeAsync(transactionType);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions by type {TransactionType}", transactionType);
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payment transactions",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get transactions by payment method
        /// </summary>
        [HttpGet("payment-methods/{paymentMethodId}")]
        [Authorize(Roles = "Admin,TripSupervisor")]
        public async Task<ActionResult<IEnumerable<ReturnPaymentTransactionDTO>>> GetTransactionsByPaymentMethod(int paymentMethodId)
        {
            try
            {
                var transactions = await _paymentTransactionService.GetTransactionsByPaymentMethodAsync(paymentMethodId);
                return Ok(transactions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for payment method {PaymentMethodId}", paymentMethodId);
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payment transactions",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get transactions by date range
        /// </summary>
        [HttpGet("date-range")]
        [Authorize(Roles = "Admin,TripSupervisor")]
        public async Task<ActionResult<IEnumerable<ReturnPaymentTransactionDTO>>> GetTransactionsByDateRange(
            [FromQuery] DateTime startDate, 
            [FromQuery] DateTime endDate)
        {
            try
            {
                var transactions = await _paymentTransactionService.GetTransactionsByDateRangeAsync(startDate, endDate);
                return Ok(transactions);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving transactions for date range {StartDate} to {EndDate}", startDate, endDate);
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payment transactions",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Update payment transaction
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,TripSupervisor")]
        public async Task<ActionResult<ReturnPaymentTransactionDTO>> UpdateTransaction(int id, [FromBody] UpdatePaymentTransactionDTO updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Error = "Validation failed",
                    Details = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                });
            }

            if (id != updateDto.Id)
            {
                return BadRequest(new { Error = "Transaction ID in URL does not match transaction ID in request body" });
            }

            try
            {
                var transaction = await _paymentTransactionService.UpdatePaymentTransactionAsync(updateDto);
                return Ok(transaction);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment transaction with ID {id} not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment transaction {TransactionId}", id);
                return StatusCode(500, new
                {
                    Error = "An error occurred while updating the payment transaction",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get detailed payment transaction information
        /// </summary>
        [HttpGet("{id}/details")]
        public async Task<ActionResult<PaymentTransactionDetailsDTO>> GetTransactionDetails(int id)
        {
            try
            {
                var details = await _paymentTransactionService.GetPaymentTransactionDetailsAsync(id);
                return Ok(details);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment transaction with ID {id} not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving details for payment transaction {TransactionId}", id);
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payment transaction details",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get total transaction amount by payment ID
        /// </summary>
        [HttpGet("payments/{paymentId}/total")]
        public async Task<ActionResult<decimal>> GetTotalTransactionAmountByPayment(int paymentId)
        {
            try
            {
                var total = await _paymentTransactionService.GetTotalTransactionAmountByPaymentAsync(paymentId);
                return Ok(new { PaymentId = paymentId, TotalAmount = total });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating total transaction amount for payment {PaymentId}", paymentId);
                return StatusCode(500, new
                {
                    Error = "An error occurred while calculating total transaction amount",
                    Details = ex.Message
                });
            }
        }
    }
}