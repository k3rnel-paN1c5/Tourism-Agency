using Microsoft.AspNetCore.Mvc;
using Application.IServices.UseCases;
using Domain.Enums;
using Application.DTOs.Payment;
using Microsoft.AspNetCore.Authorization;

namespace TourismAgency.Controllers
{
    [ApiController]
    [Route("api/payments")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        /// <summary>
        /// Create a new payment
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<ReturnPaymentDTO>> CreatePayment([FromBody] CreatePaymentDTO createPaymentDTO)
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

            try
            {
                var payment = await _paymentService.CreatePaymentAsync(createPaymentDTO);
                return CreatedAtAction(nameof(GetPayment), new { id = payment.Id }, payment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating payment for booking {BookingId}", createPaymentDTO.BookingId);
                return StatusCode(500, new
                {
                    Error = "An error occurred while creating the payment",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Process a payment
        /// </summary>
        [HttpPost("{id}/process")]
        public async Task<ActionResult<PaymentProcessResultDTO>> ProcessPayment(int id, [FromBody] ProcessPaymentDTO processPaymentDto)
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

            // Ensure the payment ID in the URL matches the DTO
            if (id != processPaymentDto.PaymentId)
            {
                return BadRequest(new { Error = "Payment ID in URL does not match payment ID in request body" });
            }

            try
            {
                var result = await _paymentService.ProcessPaymentAsync(processPaymentDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment with ID {id} not found" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment {PaymentId}", id);
                return StatusCode(500, new
                {
                    Error = "An error occurred while processing the payment",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Process a refund
        /// </summary>
        [HttpPost("{id}/refunds")]
        public async Task<ActionResult<PaymentProcessResultDTO>> ProcessRefund(int id, [FromBody] ProcessRefundDTO refundDto)
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

            // Ensure the payment ID in the URL matches the DTO
            if (id != refundDto.PaymentId)
            {
                return BadRequest(new { Error = "Payment ID in URL does not match payment ID in request body" });
            }

            try
            {
                var result = await _paymentService.ProcessRefundAsync(refundDto);
                return Ok(result);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment with ID {id} not found" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing refund for payment {PaymentId}", id);
                return StatusCode(500, new
                {
                    Error = "An error occurred while processing the refund",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get payment details by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<ReturnPaymentDTO>> GetPayment(int id)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByIdAsync(id);
                return Ok(payment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment with ID {id} not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment {PaymentId}", id);
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payment details",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get payment by booking ID
        /// </summary>
        [HttpGet("bookings/{bookingId}")]
        public async Task<ActionResult<ReturnPaymentDTO>> GetPaymentByBooking(int bookingId)
        {
            try
            {
                var payment = await _paymentService.GetPaymentByBookingIdAsync(bookingId);
                return Ok(payment);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment for booking ID {bookingId} not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payment for booking {BookingId}", bookingId);
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payment details",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get payments by status or all payments
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnPaymentDTO>>> GetPayments([FromQuery] PaymentStatus? status = null)
        {
            try
            {
                var payments = status.HasValue
                    ? await _paymentService.GetPaymentsByStatusAsync(status.Value)
                    : await _paymentService.GetAllPaymentsAsync();
                
                return Ok(payments);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving payments by status {Status}", status);
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payments",
                    Details = ex.Message
                });
            }
        }

        /// <summary>
        /// Get detailed payment information
        /// </summary>
        [HttpGet("{id}/details")]
        public async Task<ActionResult<PaymentDetailsDTO>> GetPaymentDetails(int id)
        {
            try
            {
                var details = await _paymentService.GetPaymentDetailsAsync(id);
                return Ok(details);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Error = $"Payment with ID {id} not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving details for payment {PaymentId}", id);
                return StatusCode(500, new
                {
                    Error = "An error occurred while retrieving payment details",
                    Details = ex.Message
                });
            }
        }
    }
}